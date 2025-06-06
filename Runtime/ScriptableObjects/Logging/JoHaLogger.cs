using System;
using UnityEngine;

namespace JohaToolkit.UnityEngine.ScriptableObjects.Logging
{
    [CreateAssetMenu(fileName = "JoHaLogger", menuName = "JoHaToolkit/Logs/JoHaLogger")]
    public class JoHaLogger : ScriptableObject
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        [SerializeField] private string prefix;
        [SerializeField] private bool logInfo = true;
        [SerializeField] private bool logWarning = true;
        [SerializeField] private bool logError = true;

        public void Log(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    LogInfo(message);
                    break;
                case LogLevel.Warning:
                    LogWarning(message);
                    break;
                case LogLevel.Error:
                    LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public void LogInfo(string message)
        {
            if (!logInfo)
                return;
            Debug.Log($"[{prefix}] {message}]");
        }
        
        public  void LogWarning(string message)
        {
            if(!logWarning)
                return;
            Debug.LogWarning($"[{prefix}] {message}]");
        }
        
        public void LogError(string message)
        {
            if (!logError)
                return;
            Debug.LogError($"[{prefix}] {message}]");
        }
    }
}





