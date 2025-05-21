using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public static class SaveGameManager
    {
        public static string SaveGameExtension = ".save";
        public static string SaveGameDirectory = Path.Combine(Application.persistentDataPath, "SaveGames");
        
        private static readonly List<ISaveGameUser> Users = new();

        public static event Action SaveStarted;
        public static event Action<bool> SaveCompleted;
        
        public static event Action LoadStarted;
        public static event Action<bool> LoadCompleted;
        
        public static void RegisterUser(ISaveGameUser user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }
        }

        public static async Task<bool> SaveAsync(string saveName)
        {
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            if(File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
            
            SaveStarted?.Invoke();

            bool result = await Task.Run(() =>
            {
                
                SaveGame saveGame = new SaveGame
                {
                    saveName = saveName
                };
                
                foreach (ISaveGameUser user in Users)
                {
                    user.Save(saveGame);
                }
                
                return ObjectSerializer.SaveObject(saveGame, saveFilePath);

            });
            
            SaveCompleted?.Invoke(result);
            
            return result;
        }

        public static async Task<bool> LoadAsync(string saveName)
        {
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            if (!File.Exists(saveFilePath))
            {
                Debug.LogError($"Error loading saveGame: {saveName} does not exist");
                return false;
            }
            
            LoadStarted?.Invoke();
            
            bool result = await Task.Run(() =>
            {
                SaveGame saveGame = (SaveGame)ObjectSerializer.LoadObject(saveFilePath);
                
                if (saveGame == null)
                {
                    LoadCompleted?.Invoke(false);
                    return false;
                }
                foreach (ISaveGameUser user in Users)
                {
                    user.Load(saveGame);
                }
                return true;
            });
            
            LoadCompleted?.Invoke(result);
            return result;
        }

        public static bool DeleteSaveGame(string saveName)
        {
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            
            if (!File.Exists(saveFilePath))
            {
                Debug.LogError($"Error deleting saveGame: {saveName} does not exist");
                return false;
            }
            
            try
            {
                File.Delete(saveFilePath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error deleting saveGame: {e.Message}");
                return false;
            }
        }

        public static SaveGame[] GetSaveGames()
        {
            string[] saveGameFiles = Directory.GetFiles(SaveGameDirectory, "*.save");
            List<SaveGame> saveGames = new();
            foreach (string saveGameFile in saveGameFiles)
            {
                SaveGame saveGame = (SaveGame) ObjectSerializer.LoadObject(saveGameFile);
                if (saveGame == null)
                {
                    Debug.LogWarning($"Error reading saveGame {saveGameFile} file might be corrupt");
                    continue;
                }
                saveGames.Add(saveGame);
            }
            
            return saveGames.ToArray();
        }
    }
}
