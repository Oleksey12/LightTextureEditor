using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(TextureImporter))]
[CanEditMultipleObjects]
public class CustomTextureImporter : Editor
{
    private static readonly Type _textureImporterInspectorType = Type.GetType("UnityEditor.TextureImporterInspector, UnityEditor");
    private static readonly MethodInfo _setAssetImporterTargetEditorMethod = _textureImporterInspectorType.GetMethod("InternalSetAssetImporterTargetEditor", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly FieldInfo _onEnableCalledField = typeof(AssetImporterEditor).GetField("m_OnEnableCalled", BindingFlags.Instance | BindingFlags.NonPublic);

    private Editor _defaultEditor;
    private List<TextureImporter> _targets;

    private const string NAME = "LightTextureEditor";
    private const string WIDTH = "Texture width";
    private const string HEIGHT = "Texture height";
    private const string AUTO_RESIZE = "Auto resizer";

    private void OnEnable()
    {
        if (_defaultEditor != null)
        {
            _onEnableCalledField.SetValue(_defaultEditor, true);
            DestroyImmediate(_defaultEditor);
            _defaultEditor = null;
        }

        _defaultEditor = (AssetImporterEditor)CreateEditor(targets, _textureImporterInspectorType);
        _setAssetImporterTargetEditorMethod.Invoke(_defaultEditor, new object[] { this });
        _targets = targets.Cast<TextureImporter>().ToList();
        Debug.LogError(_targets.Count);

    }

    public override void OnInspectorGUI()
    {
        _defaultEditor.OnInspectorGUI();

        if (_targets.Count != 1)
        {
            return;
        }
        Debug.LogError(((TextureImporter)target).assetPath);
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(((TextureImporter)target).assetPath);
        //GUILayout.BeginArea(new Rect(new Vector2(0, 0), new Vector2(100, 100)));

        EditorGUILayout.Space(20f);

        EditorGUILayout.LabelField(NAME, EditorStyles.boldLabel);
        EditorGUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(WIDTH, EditorStyles.whiteLabel);
        EditorGUILayout.TextField(texture.width.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(HEIGHT, EditorStyles.whiteLabel);
        EditorGUILayout.TextField(texture.height.ToString());
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Set size"))
        {

        }

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField(AUTO_RESIZE, EditorStyles.whiteLabel);
        //EditorGUILayout.DropdownButton()

        //GUILayout.EndArea();
        //rawImage.texture = Resize(inputtexture2D, 200, 100);

    }
    public Texture2D inputtexture2D;
    public RawImage rawImage;

    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY);
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    private void SetSize()
    {

    }
}