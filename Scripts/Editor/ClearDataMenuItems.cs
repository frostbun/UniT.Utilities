#nullable enable
namespace UniT.Utilities.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal static class ClearDataMenuItems
    {
        private const string ROOT_PATH                  = "Edit/";
        private const string CLEAR_PERSISTENT_DATA_PATH = "Clear PersistentDataPath";
        private const string CLEAR_TEMPORARY_CACHE_PATH = "Clear TemporaryCachePath";
        private const string CLEAR_EVERYTHING           = "Clear Everything";

        [MenuItem(ROOT_PATH + CLEAR_PERSISTENT_DATA_PATH, priority = 15001)]
        private static void ClearPersistentDataPathMenuItem()
        {
            if (!Confirm(CLEAR_PERSISTENT_DATA_PATH)) return;
            Directory.Delete(Application.persistentDataPath, recursive: true);
        }

        [MenuItem(ROOT_PATH + CLEAR_TEMPORARY_CACHE_PATH, priority = 15002)]
        private static void OpenLazyGitMenuItem()
        {
            if (!Confirm(CLEAR_TEMPORARY_CACHE_PATH)) return;
            Directory.Delete(Application.temporaryCachePath, recursive: true);
        }

        [MenuItem(ROOT_PATH + CLEAR_EVERYTHING, priority = 15003)]
        private static void ClearEverythingMenuItem()
        {
            if (!Confirm(CLEAR_EVERYTHING)) return;
            PlayerPrefs.DeleteAll();
            Directory.Delete(Application.persistentDataPath, recursive: true);
            Directory.Delete(Application.temporaryCachePath, recursive: true);
        }

        private static bool Confirm(string action)
        {
            return EditorDialog.DisplayDecisionDialog(
                action,
                $"Are you sure you want to {action}? This action cannot be undone.",
                "Yes",
                "No"
            );
        }
    }
}