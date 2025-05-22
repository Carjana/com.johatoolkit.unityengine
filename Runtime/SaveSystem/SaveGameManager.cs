using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public static class SaveGameManager
    {
        public enum SaveGameFormat
        {
            Binary,
            Json
        }
        
        public static string SaveGameExtension = ".save";
        public static string SaveGameDirectory = Path.Combine(Application.persistentDataPath, "SaveGames");
        public static SaveGameFormat Format = SaveGameFormat.Json;
        
        private static readonly List<ISaveGameUser> Users = new();
        public static SaveGame CurrentSaveGame { get; private set; }
        
        public static event Action SaveStarted;
        public static event Action<bool> SaveCompleted;
        
        public static event Action LoadStarted;
        public static event Action<bool> LoadCompleted;
        
        public static void RegisterSaveGameUser(this ISaveGameUser user)
        {
            if (!Users.Contains(user))
            {
                Users.Add(user);
            }
        }
        
        public static void UnregisterSaveGameUser(this ISaveGameUser user)
        {
            if (Users.Contains(user))
            {
                Users.Remove(user);
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
                SaveGame saveGame = new()
                {
                    SaveName = saveName,
                    SaveFilePath = saveFilePath
                };
                if (CurrentSaveGame != null)
                {
                    saveGame.SaveData = CurrentSaveGame.SaveData;
                }
                
                foreach (ISaveGameUser user in Users.Where(IsUserExisting))
                {
                    user.Save(saveGame);
                }
                Debug.Log(saveGame.SaveName);

                bool saveGameSucceeded = Format switch
                {
                    SaveGameFormat.Binary => ObjectSerializer.SaveObject(saveGame, saveFilePath),
                    SaveGameFormat.Json => ObjectSerializer.SaveJson(saveGame, saveFilePath),
                    _ => false
                };
                
                if (saveGameSucceeded)
                {
                    CurrentSaveGame = saveGame;
                }
                
                return saveGameSucceeded;
            });
            
            SaveCompleted?.Invoke(result);
            
            return result;
        }

        public static async Task<bool> LoadAsync(string saveName)
        {
            if (!IsSaveGameExisting(saveName))
                return false;
                
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            
            LoadStarted?.Invoke();
            
            bool result = await Task.Run(() =>
            {
                if (!LoadSaveGameFromPath(saveFilePath, out SaveGame saveGame))
                {
                    return false;
                }
                
                CurrentSaveGame = saveGame;
                
                foreach (ISaveGameUser user in Users.Where(IsUserExisting))
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
            if (!IsSaveGameExisting(saveName))
                return false;
            
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            
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
            string[] saveGameFiles = Directory.GetFiles(SaveGameDirectory, $"*{SaveGameExtension}");
            List<SaveGame> saveGames = new();
            foreach (string saveGameFile in saveGameFiles)
            {
                if(!LoadSaveGameFromPath(saveGameFile, out SaveGame saveGame))
                    continue;
                saveGames.Add(saveGame);
            }
            return saveGames.ToArray();
        }

        private static bool LoadSaveGameFromPath(string path, out SaveGame saveGame)
        {
            SaveGame save = Format switch
            {
                SaveGameFormat.Binary => (SaveGame)ObjectSerializer.LoadObject(path),
                SaveGameFormat.Json => ObjectSerializer.LoadJson<SaveGame>(path),
                _ => null
            };
            if (save == null)
            {
                Debug.LogWarning($"Error reading saveGame {path} file might be corrupt");
                saveGame = null;
                return false;
            }
            
            saveGame = save;
            return true;
        }
        
        public static bool IsSaveGameExisting(string saveName)
        {
            saveName += SaveGameExtension;
            string saveFilePath = Path.Combine(SaveGameDirectory, saveName);
            bool exists = File.Exists(saveFilePath);

            if (!exists)
            {
                Debug.LogWarning($"[SaveManager]: {saveName} does not exist");
            }
            
            return exists;
        }
        
        private static bool IsUserExisting(ISaveGameUser user)
        {
            if (user is not null) 
                return true;
            
            Debug.LogWarning($"One SaveGameUser is null, did you unregister it?");
            return false;

        }
    }
}
