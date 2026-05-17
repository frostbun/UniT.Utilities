#nullable enable
namespace UniT.Utilities.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEditor.Toolbars;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal static class SceneSelectorToolbar
    {
        private const string ELEMENT_PATH = "Scene Selector";

        private static string[] ScenePaths;

        static SceneSelectorToolbar()
        {
            ScenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);

            EditorApplication.projectChanged                += OnProjectChanged;
            EditorApplication.playModeStateChanged          += OnPlayModeStateChanged;
            SceneManager.activeSceneChanged                 += OnSceneChanged;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
        }

        [MainToolbarElement(ELEMENT_PATH)]
        private static MainToolbarElement CreateSceneSelectorDropdown()
        {
            var currentScene = SceneManager.GetActiveScene().name;
            var icon         = (Texture2D)EditorGUIUtility.IconContent("UnityLogo").image;
            return Application.isPlaying || ScenePaths.Length == 0
                ? new MainToolbarLabel(new(currentScene, icon, "Select active scene"))
                : new MainToolbarDropdown(new(currentScene, icon, "Select active scene"), dropDownRect =>
                {
                    var menu = new GenericMenu();
                    foreach (var path in ScenePaths)
                    {
                        var scene = Path.GetFileNameWithoutExtension(path);
                        if (scene == currentScene)
                        {
                            menu.AddDisabledItem(new(scene), true);
                        }
                        else
                        {
                            menu.AddItem(new(scene), false, () =>
                            {
                                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                                EditorSceneManager.OpenScene(path);
                            });
                        }
                    }
                    menu.DropDown(dropDownRect);
                });
        }

        private static void OnProjectChanged()
        {
            ScenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
            MainToolbar.Refresh(ELEMENT_PATH);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state is PlayModeStateChange.ExitingEditMode or PlayModeStateChange.ExitingPlayMode) return;
            MainToolbar.Refresh(ELEMENT_PATH);
        }

        private static void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            MainToolbar.Refresh(ELEMENT_PATH);
        }
    }
}