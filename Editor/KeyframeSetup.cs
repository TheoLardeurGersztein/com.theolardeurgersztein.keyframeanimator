using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class KeyframeEditor : EditorWindow
{
    private KeyframeConfig config;

    [MenuItem("Tools/Keyframe Animation Config")]
    public static void ShowWindow()
    {
        GetWindow<KeyframeEditor>("Keyframe Animation Config").CreateOrLoadConfig();
    }

    private void CreateOrLoadConfig()
    {
        string baseName = "KeyframeConfig";
        string path = "Assets/";
        string extension = ".asset";

        // Get all assets in the path that match the base name
        string[] existingConfigs = AssetDatabase.FindAssets("t:KeyframeConfig")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(p => Path.GetFileNameWithoutExtension(p).StartsWith(baseName))
            .ToArray();

        // Determine a unique name by checking existing ones
        int index = 1;
        string newConfigName = baseName;
        while (existingConfigs.Any(p => Path.GetFileNameWithoutExtension(p) == newConfigName))
        {
            index++;
            newConfigName = baseName + index;
        }

        // Create new KeyframeConfig
        KeyframeConfig newConfig = ScriptableObject.CreateInstance<KeyframeConfig>();
        string assetPath = path + newConfigName + extension;
        AssetDatabase.CreateAsset(newConfig, assetPath);
        AssetDatabase.SaveAssets();

        // Assign to the editor
        config = newConfig;

        Debug.Log($"Created new KeyframeConfig: {newConfigName}");
    }

    private void OnGUI()
    {
        GUILayout.Label("Keyframe Animation Configuration", EditorStyles.boldLabel);

        config = (KeyframeConfig)EditorGUILayout.ObjectField("Config Asset", config, typeof(KeyframeConfig), false);

        if (config != null)
        {

            config.targetUIObject = (RectTransform)EditorGUILayout.ObjectField("Target UI Object", config.targetUIObject, typeof(RectTransform), true);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Loaded Target UI Object", config.targetUIObjectName);
            EditorGUI.EndDisabledGroup();

            config.updateRate = EditorGUILayout.FloatField("Update Rate", config.updateRate);

            GUILayout.Label("Keyframe Input");
            config.keyframeInput = EditorGUILayout.TextArea(config.keyframeInput, GUILayout.Height(100));

            if (GUILayout.Button("Load Keyframes"))
            {
                config.LoadKeyframes();
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }

        }
    }
}
