namespace Oculus.Platform
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.IO;
  using System.Reflection;
  using UnityEditor;
  using UnityEngine;

  [CustomEditor(typeof(PlatformSettings))]
  public class OculusPlatformSettingsEditor : Editor
  {
    [UnityEditor.MenuItem("Oculus Platform/Edit Settings")]
    public static void Edit()
    {
      var settings = PlatformSettings.Instance;
      if (settings == null)
      {
        settings = ScriptableObject.CreateInstance<PlatformSettings>();
        string properPath = Path.Combine(UnityEngine.Application.dataPath, "Resources");
        if (!Directory.Exists(properPath))
        {
          AssetDatabase.CreateFolder("Assets", "Resources");
        }

        string fullPath = Path.Combine(
          Path.Combine("Assets", "Resources"),
          "OculusPlatformSettings.asset"
        );
        AssetDatabase.CreateAsset(settings, fullPath);
        PlatformSettings.Instance = settings;
      }
      UnityEditor.Selection.activeObject = settings;

      if (String.IsNullOrEmpty(PlatformSettings.MobileAppID))
      {
        PlatformSettings.MobileAppID = PlatformSettings.AppID;
      }
    }

    private bool showBuildSettings = true;
    private bool showUnityEditorSettings = true;

    public override void OnInspectorGUI()
    {
      EditorGUILayout.Separator();
      if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
      {
        if (String.IsNullOrEmpty(PlatformSettings.MobileAppID))
        {
          EditorGUILayout.HelpBox("Add your Gear VR App Id", MessageType.Error);
        }
        else
        {
          EditorGUILayout.HelpBox("Using Gear VR App Id: " + PlatformSettings.MobileAppID, MessageType.Info);
        }
      }
      else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows || EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
      {
        if (String.IsNullOrEmpty(PlatformSettings.AppID))
        {
          EditorGUILayout.HelpBox("Add your Oculus Rift App Id", MessageType.Error);
        }
        else
        {
          EditorGUILayout.HelpBox("Using Oculus Rift App Id: " + PlatformSettings.AppID, MessageType.Info);
        }
      }
      else
      {
        EditorGUILayout.HelpBox("Invalid build target: " + EditorUserBuildSettings.activeBuildTarget, MessageType.Error);
      }

      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Oculus Rift App Id");
      GUI.changed = false;
      PlatformSettings.AppID = EditorGUILayout.TextField(PlatformSettings.AppID);
      SetDirtyOnGUIChange();
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField("Gear VR App Id");
      GUI.changed = false;
      PlatformSettings.MobileAppID = EditorGUILayout.TextField(PlatformSettings.MobileAppID);
      SetDirtyOnGUIChange();
      EditorGUILayout.EndHorizontal();
      if (GUILayout.Button("Create / Find your app on https://dashboard.oculus.com"))
      {
        UnityEngine.Application.OpenURL("https://dashboard.oculus.com/");
      }
      EditorGUILayout.Separator();

      showBuildSettings = EditorGUILayout.Foldout(showBuildSettings, "Build Settings");
      if (showBuildSettings)
      {
        if (!PlayerSettings.virtualRealitySupported)
        {
          EditorGUILayout.HelpBox("VR Support isn't enabled in the Player Settings", MessageType.Warning);
        }
        else
        {
          EditorGUILayout.HelpBox("VR Support is enabled", MessageType.Info);
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Virtual Reality Support");
        PlayerSettings.virtualRealitySupported = EditorGUILayout.Toggle(PlayerSettings.virtualRealitySupported);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bundle Identifier");
        PlayerSettings.bundleIdentifier = EditorGUILayout.TextField(PlayerSettings.bundleIdentifier);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bundle Version");
        PlayerSettings.bundleVersion = EditorGUILayout.TextField(PlayerSettings.bundleVersion);
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.Separator();
      
      showUnityEditorSettings = EditorGUILayout.Foldout(showUnityEditorSettings, "Unity Editor Settings");
      if (showUnityEditorSettings)
      {
        if (String.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformAccessToken))
        {
          if (GUILayout.Button("Get User Token"))
          {
            UnityEngine.Application.OpenURL("https://developer2.oculus.com/application/" + PlatformSettings.AppID + "/api");
          }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Oculus User Token");
        StandalonePlatformSettings.OculusPlatformAccessToken = EditorGUILayout.TextField(StandalonePlatformSettings.OculusPlatformAccessToken);
        EditorGUILayout.EndHorizontal();
      }
    }

    private void SetDirtyOnGUIChange()
    {
      if (GUI.changed)
      {
        EditorUtility.SetDirty(PlatformSettings.Instance);
        GUI.changed = false;
      }
    }
  }
}
