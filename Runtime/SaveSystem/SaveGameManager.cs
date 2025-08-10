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
        public static SaveGame DefaultSaveGame { get; private set; }
        private static SaveGame _currentSaveGame;

        public static SaveGame CurrentSaveGame
        {
            get => _currentSaveGame ??= DefaultSaveGame;
            private set => _currentSaveGame = value;
        }

        public static string SaveDirectory { get; private set; } =
            Path.Combine(Application.persistentDataPath, "SaveGames");

        public static string SaveExtension { get; private set; } = ".saveGame";

        private static bool _isInitialized = false;

        public static event Action<SaveGame> SaveGameStarted;
        public static event Action<SaveGame> SaveGameEnded;
        public static event Action<string> LoadGameStarted;
        public static event Action<SaveGame> LoadGameEnded;

        public static ISaveGameSerializer<SaveGame> SaveGameSerializer { get; private set; } =
            new JsonSaveGameSerializer<SaveGame>();

        public static void Init(SaveGame defaultSaveGame)
        {
            if (_isInitialized)
            {
                Debug.LogWarning("SaveGameManager is already initialized.");
                return;
            }

            if (!CheckSaveDirectory())
                return;
            DefaultSaveGame = defaultSaveGame;
            _isInitialized = true;
            EnableMethods();
        }

        private static bool CheckSaveDirectory()
        {
            if (Directory.Exists(SaveDirectory))
                return true;
            try
            {
                Directory.CreateDirectory(SaveDirectory);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create save directory: {e.Message}");
                return false;
            }
        }

        public static void SetSaveDirectory(string directory)
        {
            if (_isInitialized)
            {
                Debug.LogWarning(
                    "SaveGameManager is already initialized. Changing the save directory will not take effect.");
                return;
            }

            if (string.IsNullOrEmpty(directory))
            {
                Debug.LogError("Save directory cannot be null or empty.");
                return;
            }

            SaveDirectory = directory;
        }

        public static void SetSaveExtension(string extension)
        {
            if (_isInitialized)
            {
                Debug.LogWarning(
                    "SaveGameManager is already initialized. Changing the save extension will not take effect.");
                return;
            }

            if (string.IsNullOrEmpty(extension) || !extension.StartsWith("."))
            {
                Debug.LogError("Save extension must be a valid file extension starting with a dot.");
                return;
            }

            SaveExtension = extension;
        }

        public static void SetSaveGameSerializer(ISaveGameSerializer<SaveGame> serializer)
        {
            if (_isInitialized)
            {
                Debug.LogWarning(
                    "SaveGameManager is already initialized. Changing the serializer will not take effect.");
                return;
            }

            if (serializer == null)
            {
                Debug.LogError("Serializer cannot be null.");
                return;
            }

            SaveGameSerializer = serializer;
        }

        private static void EnableMethods()
        {
            SaveGameAsync = ActualSaveGameAsync;
            LoadGameAsync = ActualLoadGameAsync;
            DeleteSaveGameAsync = ActualDeleteSaveGame;
        }

        public static Func<string, Task<bool>> SaveGameAsync = NotInitialized;
        public static Func<string, Task<bool>> LoadGameAsync = NotInitialized;
        public static Func<string, Task<bool>> DeleteSaveGameAsync = NotInitialized;

        private static Task<bool> NotInitialized(string s) =>
            throw new InvalidOperationException("SaveGameManager is not initialized. Call Init() before using it.");

        private static Task<bool> ActualSaveGameAsync(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                Debug.LogError("Save name cannot be null or empty.");
                return Task.FromResult(false);
            }


            CurrentSaveGame.SaveName = saveName;

            SaveGameStarted?.Invoke(CurrentSaveGame);

            string savePath = GetSavePath(saveName);

            if (File.Exists(savePath))
                File.Delete(savePath);

            bool result = SaveGameSerializer.Save(CurrentSaveGame, savePath);

            SaveGameEnded?.Invoke(CurrentSaveGame);

            return Task.FromResult(result);
        }

        private static Task<bool> ActualLoadGameAsync(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                Debug.LogError("Save name cannot be null or empty.");
                return Task.FromResult(false);
            }

            string savePath = GetSavePath(saveName);
            if (!File.Exists(savePath))
            {
                Debug.LogError($"Save file not found: {savePath}");
                return Task.FromResult(false);
            }

            LoadGameStarted?.Invoke(saveName);
            bool result = SaveGameSerializer.Load(savePath, out SaveGame loadedSaveGame);
            if (result)
            {
                CurrentSaveGame = loadedSaveGame;
                CurrentSaveGame.SaveName = saveName;
                LoadGameEnded?.Invoke(CurrentSaveGame);
            }
            else
            {
                Debug.LogError($"Failed to load save game from {savePath}");
            }

            LoadGameEnded?.Invoke(CurrentSaveGame);

            return Task.FromResult(result);
        }

        private static Task<bool> ActualDeleteSaveGame(string saveName)
        {
            if (!DoesSaveGameExist(saveName))
            {
                Debug.LogError($"Save game {saveName} does not exist.");
                return Task.FromResult(false);
            }

            if (saveName == CurrentSaveGame.SaveName)
            {
                ClearCurrentSaveGame();
            }
            
            string savePath = GetSavePath(saveName);
            try
            {
                File.Delete(savePath);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to delete save game {saveName}: {e.Message}");
                return Task.FromResult(false);
            }
        }

        private static string GetSavePath(string saveName)
        {
            if (!string.IsNullOrEmpty(saveName))
                return Path.Combine(SaveDirectory, saveName + SaveExtension);
            Debug.LogError("Save name cannot be null or empty.");
            return null;
        }
        
        public static void ClearCurrentSaveGame()
        {
            LoadGameStarted?.Invoke(DefaultSaveGame.SaveName);
            CurrentSaveGame = DefaultSaveGame;
            LoadGameEnded?.Invoke(CurrentSaveGame);
        }

        public static List<string> GetAllSaveGames()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Debug.LogWarning("Save directory does not exist.");
                return new List<string>();
            }

            return Directory.GetFiles(SaveDirectory, "*" + SaveExtension)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public static bool DoesSaveGameExist(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                Debug.LogError("Save name cannot be null or empty.");
                return false;
            }

            string savePath = GetSavePath(saveName);
            return File.Exists(savePath);
        }
    }
}
