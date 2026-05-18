#nullable enable
namespace UniT.Utilities.Editor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using UnityEditor;

    internal static class OpenAppMenuItems
    {
        private const string OpenTerminalHerePath = "Assets/Open Terminal Here";
        private const string OpenOpenCodePath     = "Assets/Open OpenCode";

        [MenuItem(OpenTerminalHerePath)]
        private static void OpenTerminalHereMenuItem()
        {
            AppExist("xdg-terminal-exec");
            Process.Start(new ProcessStartInfo
            {
                #if UNITY_EDITOR_LINUX
                FileName = "xdg-terminal-exec",
                #else
                FileName = "alacritty",
                #endif
                WorkingDirectory = GetSelectedFolder(),
            });
        }

        [MenuItem(OpenOpenCodePath)]
        private static void OpenOpenCodeMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                #if UNITY_EDITOR_LINUX
                FileName  = "xdg-terminal-exec",
                Arguments = "opencode",
                #else
                FileName  = "alacritty",
                Arguments = "-e opencode",
                #endif
            });
        }

        [MenuItem(OpenTerminalHerePath, isValidateFunction: true)]
        private static bool TerminalExist() => AppExist(
            #if UNITY_EDITOR_LINUX
            "xdg-terminal-exec"
            #else
            "alacritty"
            #endif
        );

        [MenuItem(OpenOpenCodePath, isValidateFunction: true)]
        private static bool OpenCodeExist() => TerminalExist() && AppExist("opencode");

        private static readonly Dictionary<string, bool> AppExistCache = new();

        private static bool AppExist(string name)
        {
            if (AppExistCache.TryGetValue(name, out var exist)) return exist;
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName  = "which",
                Arguments = name,
            })!;
            process.WaitForExit();
            exist = process.ExitCode is 0;
            AppExistCache.Add(name, exist);
            return exist;
        }

        private static string GetSelectedFolder()
        {
            var path = GetSelectedFileOrFolder();
            return Directory.Exists(path) ? path : Path.GetDirectoryName(path)!;
        }

        private static string GetSelectedFileOrFolder()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : Path.GetFullPath(path);
        }
    }
}