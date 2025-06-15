using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

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

        [Obsolete("Use LogInfo, LogWarning, or LogError instead.")]
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

        public void LogInfo(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
        {
            if (!logInfo)
                return;
            Debug.Log($"[{prefix}] {message}\n{GetCallerInfo(filePath, lineNumber, memberName)}");
        }

        public void LogWarning(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
        {
            if (!logWarning)
                return;
            Debug.LogWarning($"[{prefix}] {message}\n{GetCallerInfo(filePath, lineNumber, memberName)}");
        }

        public void LogError(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = "")
        {
            if (!logError)
                return;
            Debug.LogError($"[{prefix}] {message}\n{GetCallerInfo(filePath, lineNumber, memberName)}");
        }

        private string GetCallerInfo(string filePath, int lineNumber, string memberName) => $"[{Path.GetFileName(filePath)}:{lineNumber} {memberName}]";
    }
}





