#nullable enable
namespace UniT.Utilities.Editor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal static class OpenAppMenuItems
    {
        private const string ROOT_PATH               = "Assets/UniT/";
        private const string OPEN_TERMINAL_HERE_PATH = ROOT_PATH + "Open Terminal Here";
        private const string OPEN_OPENCODE_PATH      = ROOT_PATH + "Open OpenCode";
        private const string OPEN_LAZYGIT_PATH       = ROOT_PATH + "Open LazyGit";

        [MenuItem(OPEN_TERMINAL_HERE_PATH, priority = 1000)]
        private static void OpenTerminalHereMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName         = TerminalExecutable,
                WorkingDirectory = GetSelectedFolder(),
            });
        }

        [MenuItem(OPEN_LAZYGIT_PATH, priority = 1001)]
        private static void OpenLazyGitMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName  = TerminalExecutable,
                Arguments = GetArguments("lazygit"),
            });
        }

        [MenuItem(OPEN_OPENCODE_PATH, priority = 1002)]
        private static void OpenOpenCodeMenuItem()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName  = TerminalExecutable,
                Arguments = GetArguments("opencode"),
            });
        }

        [MenuItem(OPEN_TERMINAL_HERE_PATH, isValidateFunction: true)]
        private static bool TerminalExist() => AppExist(TerminalExecutable);

        [MenuItem(OPEN_LAZYGIT_PATH, isValidateFunction: true)]
        private static bool LazyGitExist() => TerminalExist() && AppExist("lazygit");

        [MenuItem(OPEN_OPENCODE_PATH, isValidateFunction: true)]
        private static bool OpenCodeExist() => TerminalExist() && AppExist("opencode");

        private static readonly string TerminalExecutable = Application.platform is RuntimePlatform.LinuxEditor
            ? "xdg-terminal-exec"
            : "alacritty";

        private static string GetArguments(string appName) => Application.platform is RuntimePlatform.LinuxEditor
            ? appName
            : $"-e {appName}";

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