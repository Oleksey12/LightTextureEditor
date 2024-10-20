namespace LightTextureEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEditor.AssetImporters;
    using UnityEngine;

    [CustomEditor(typeof(TextureImporter))]
    [CanEditMultipleObjects]
    public class TextureEditorView : Editor
    {
        private static readonly Type _textureImporterInspectorType = Type.GetType("UnityEditor.TextureImporterInspector, UnityEditor");
        private static readonly MethodInfo _setAssetImporterTargetEditorMethod = _textureImporterInspectorType.GetMethod("InternalSetAssetImporterTargetEditor", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _onEnableCalledField = typeof(AssetImporterEditor).GetField("m_OnEnableCalled", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly Lazy<TextureEditorFacade> _textureFacade = new Lazy<TextureEditorFacade>(() => new TextureEditorFacade());

        private Editor _defaultEditor = default;
        private TextureImporter _targetImporter = default;

        private List<TextureImporter> _targets = new List<TextureImporter>();

        private bool _isReplaced = false;

        private int _width = 0;
        private int _height = 0;
        private string _imageName = "";

        private const string NAME = "LightTextureEditor";
        private const string WIDTH = "New width";
        private const string HEIGHT = "New height";
        private const string SAVE_TEXT = "Replace existing texture";
        private const string IMAGE_NAME = "Resized image name";
        private const string SAVE_BUTTON = "Resize";

        protected void OnEnable()
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
        }

        public override void OnInspectorGUI()
        {
            _defaultEditor.OnInspectorGUI();

            if (_targets.Count != 1)
            {
                return;
            }

            _targetImporter = (TextureImporter)target;
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(_targetImporter.assetPath);
            
            // »нициализируем стандартные значени€ дл€ input полей
            if (_width == 0 && _height == 0)
            {
                _width = texture.width;
                _height = texture.height;
                _imageName = _targetImporter.assetPath;
            }

            EditorGUILayout.Space(20f);

            EditorGUILayout.LabelField(NAME, EditorStyles.boldLabel);
            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(WIDTH, EditorStyles.whiteLabel);
            _width = EditorGUILayout.IntField(_width);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(HEIGHT, EditorStyles.whiteLabel);
            _height = EditorGUILayout.IntField(_height);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(SAVE_TEXT, EditorStyles.whiteLabel);
            _isReplaced = EditorGUILayout.Toggle(_isReplaced);
            EditorGUILayout.EndHorizontal();
            if (!_isReplaced)
            {
                EditorGUILayout.LabelField(IMAGE_NAME, EditorStyles.whiteLabel);
                _imageName = EditorGUILayout.TextField(_imageName);
            }
            EditorGUILayout.Space(10f);

            if (GUILayout.Button(SAVE_BUTTON))
            {
                _textureFacade.Value.EditTexture(texture, _isReplaced ? _targetImporter.assetPath : _imageName, _width, _height);
            }
        }
    }
}