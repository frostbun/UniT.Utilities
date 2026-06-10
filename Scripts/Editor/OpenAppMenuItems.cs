#nullable enable
namespace UniT.Utilities.Editor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using UnityEditor;

    internal static class OpenAppMenuItems
    {
        private const string ROOT_PATH               = "Assets/UniT/";
        private const string OPEN_TERMINAL_HERE_PATH = ROOT_PATH + "Open Terminal Here";
        private const string OPEN_LAZYGIT_PATH       = ROOT_PATH + "Open LazyGit";
        private const string OPEN_OPENCODE_PATH      = ROOT_PATH + "Open OpenCode";

        #if UNITY_EDITOR_LINUX
        private const string TERMINAL = "xdg-terminal-exec";
        #else
        private const string TERMINAL = "alacritty";
        #endif
        private const string LAZYGIT  = "lazygit";
        private const string OPENCODE = "opencode";

        [MenuItem(OPEN_TERMINAL_HERE_PATH, priority = 1000)]
        private static void OpenTerminalHereMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName         = TERMINAL,
                WorkingDirectory = GetSelectedFolder(),
            });
        }

        [MenuItem(OPEN_LAZYGIT_PATH, priority = 1001)]
        private static void OpenLazyGitMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName  = TERMINAL,
                Arguments = $"-e {LAZYGIT}",
            });
        }

        [MenuItem(OPEN_OPENCODE_PATH, priority = 1002)]
        private static void OpenOpenCodeMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName  = TERMINAL,
                Arguments = $"-e {OPENCODE}",
            });
        }

        [MenuItem(OPEN_TERMINAL_HERE_PATH, isValidateFunction: true)]
        private static bool TerminalExist() => AppExist(TERMINAL);

        [MenuItem(OPEN_LAZYGIT_PATH, isValidateFunction: true)]
        private static bool LazyGitExist() => TerminalExist() && AppExist(LAZYGIT);

        [MenuItem(OPEN_OPENCODE_PATH, isValidateFunction: true)]
        private static bool OpenCodeExist() => TerminalExist() && AppExist(OPENCODE);

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