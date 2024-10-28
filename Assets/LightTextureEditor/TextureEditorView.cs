namespace LightTextureEditor
{
    using System.Collections.Generic;
    using UnityEditor.AssetImporters;
    using System.Reflection;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    using System;

    /// <summary>
    /// Добавляет в стандартный UI импортера текстур вью для редактирования размера текстур
    /// </summary>
    [CustomEditor(typeof(TextureImporter))]
    [CanEditMultipleObjects]
    public sealed class TextureEditorView : Editor
    {
        // TODO
        // Подумать есть ли смысл от Lazy типа и узнать
        // приведет ли это к большому количеству созданных экземпляров
        private static readonly Type _textureImporterInspectorType = Type.GetType("UnityEditor.TextureImporterInspector, UnityEditor");
        private static readonly MethodInfo _setAssetImporterTargetEditorMethod = _textureImporterInspectorType.GetMethod("InternalSetAssetImporterTargetEditor", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _onEnableCalledField = typeof(AssetImporterEditor).GetField("m_OnEnableCalled", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly TextureEditorFacade _textureFacade = new TextureEditorFacade();

        private List<TextureImporter> _targets = new List<TextureImporter>();
        private Action _editTextureFunction = delegate { };
        private Texture2D _texture = default;
        private TextureImporter _targetImporter = default;
        private Editor _defaultEditor = default;

        private string[] _modeNames = 
        {
            "Кратный четырем (для сжатия)",
            "Степень двойки",
            "Кастомный размер"
        };

        private string _imageName = "";
        private int _mode = 0;
        private int _width = 0;
        private int _height = 0;

        private const string NAME = "LightTextureEditor";
        private const string MODE = "Режим масштабирования";
        private const string WIDTH = "Новая ширина";
        private const string HEIGHT = "Новая высота";
        private const string IMAGE_NAME = "Название изображения";
        private const string EDIT_BUTTON = "Масштабировать";

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
        }

        /// <summary>
        /// Рисует расширенный интерфейс импортера текстур
        /// </summary>
        public override void OnInspectorGUI()
        {
            _defaultEditor.OnInspectorGUI();

            // TODO
            // Добавить масштабирование для нескольких картинок
            if (_targets.Count != 1)
            {
                return;
            }

            _targetImporter = (TextureImporter)target;
            _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(_targetImporter.assetPath);

            // Инициализация стандартных значения для input полей
            if (_width == 0 && _height == 0)
            {
                _width = _texture.width;
                _height = _texture.height;
                _imageName = _targetImporter.assetPath;
            }

            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField(NAME, EditorStyles.boldLabel);
            EditorGUILayout.Space(10f);

            EditorGUILayout.LabelField(IMAGE_NAME, EditorStyles.whiteLabel);
            _imageName = EditorGUILayout.TextField(_imageName, GUILayout.MaxWidth(300f));
            EditorGUILayout.Space(5f);

            EditorGUILayout.LabelField(MODE, EditorStyles.whiteLabel);
            _mode = EditorGUILayout.Popup(_mode, _modeNames, GUILayout.MaxWidth(300f));
            SelectModeLogic(_mode);

            EditorGUILayout.Space(10f);
            if (GUILayout.Button(EDIT_BUTTON, GUILayout.MaxWidth(175f)))
            {
                _editTextureFunction();
            }
        }

        private void SelectModeLogic(int selectedMode)
        {
            if (selectedMode == 0)
            { 
                _editTextureFunction = delegate { _textureFacade.EditeTextureToDivisorOf4(_texture, _imageName); };
            }
            else if (selectedMode == 1)
            {
                _editTextureFunction = delegate { _textureFacade.EditTextureToPowerOf2(_texture, _imageName); };
            }
            else if (selectedMode == 2)
            {
                _editTextureFunction = delegate { _textureFacade.EditTexture(_texture, _imageName, _width, _height); };
                EditorGUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(WIDTH, EditorStyles.whiteLabel, GUILayout.Width(150f), GUILayout.ExpandWidth(true));
                _width = EditorGUILayout.IntField(_width, GUILayout.Width(240f), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(HEIGHT, EditorStyles.whiteLabel, GUILayout.Width(150f), GUILayout.ExpandWidth(true));
                _height = EditorGUILayout.IntField(_height, GUILayout.Width(240f), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}