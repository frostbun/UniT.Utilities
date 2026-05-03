#nullable enable
namespace UniT.Utilities.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEditor.Toolbars;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal static class SceneSelector
    {
        private const string ELEMENT_PATH = "Scene Selector";

        static SceneSelector()
        {
            RefreshSceneList();
            EditorApplication.projectChanged                += RefreshSceneList;
            EditorApplication.playModeStateChanged          += OnPlayModeStateChanged;
            SceneManager.activeSceneChanged                 += OnSceneChanged;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
        }

        private static string[] ScenePaths = null!;

        [MainToolbarElement(ELEMENT_PATH, defaultDockPosition = MainToolbarDockPosition.Right)]
        private static MainToolbarElement CreateSceneSelectorDropdown()
        {
            var icon = (Texture2D)EditorGUIUtility.IconContent("UnityLogo").image;
            return Application.isPlaying || ScenePaths.Length == 0
                ? new MainToolbarLabel(new(SceneManager.GetActiveScene().name, icon, "Select active scene"))
                : new MainToolbarDropdown(new(EditorSceneManager.GetActiveScene().name, icon, "Select active scene"), dropDownRect =>
                {
                    var menu = new GenericMenu();
                    foreach (var path in ScenePaths)
                    {
                        menu.AddItem(new(Path.GetFileNameWithoutExtension(path)), false, () =>
                        {
                            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            {
                                EditorSceneManager.OpenScene(path);
                            }
                        });
                    }
                    menu.DropDown(dropDownRect);
                });
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state is PlayModeStateChange.ExitingEditMode or PlayModeStateChange.ExitingPlayMode) return;
            RefreshElement();
        }

        private static void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            RefreshElement();
        }

        private static void RefreshSceneList()
        {
            ScenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
        }

        public static void RefreshElement()
        {
            MainToolbar.Refresh(ELEMENT_PATH);
        }
    }

    internal sealed class SceneWatcher : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in movedAssets)
            {
                if (!path.EndsWith(".unity")) continue;
                SceneSelector.RefreshElement();
                return;
            }
        }
    }
}