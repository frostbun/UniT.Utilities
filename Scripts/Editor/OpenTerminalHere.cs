#nullable enable
namespace UniT.Utilities.Editor
{
    using System.Diagnostics;
    using System.IO;
    using UnityEditor;

    internal static class OpenTerminalHere
    {
        [MenuItem("Assets/Open Terminal Here")]
        private static void OpenTerminalHereMenuItem()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            path = string.IsNullOrEmpty(path) ? Directory.GetCurrentDirectory() : Path.GetFullPath(path);
            if (!Directory.Exists(path)) path = Path.GetDirectoryName(path)!;
            Process.Start(new ProcessStartInfo
            {
                #if UNITY_EDITOR_LINUX
                FileName = "xdg-terminal-exec",
                #else
                FileName = "alacritty",
                #endif
                WorkingDirectory = path,
            });
        }
    }
}