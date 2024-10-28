namespace LightTextureEditor
{
    using UnityEngine;

    /// <summary>
    /// Простой интерфейс для разных способов редактирования текстуры
    /// </summary>
    public class TextureEditorFacade
    {
        private readonly SaveController _saveController = default;
        private readonly TextureController _textureController = default;

        private Texture2D _resultTexture = default;

        public TextureEditorFacade()
        {
            _saveController = new SaveController();
            _textureController = new TextureController();
        }

        public void EditTexture(Texture2D inputTexture, string path, int width, int height)
        {
            _resultTexture = _textureController.Resize(inputTexture, width, height);
            if (_resultTexture)
            {
                _saveController.SaveTexture(_resultTexture, path);
            }
        }
        public void EditTextureToPowerOf2(Texture2D inputTexture, string path)
        {
            _resultTexture = _textureController.ResizeTo2Power(inputTexture, inputTexture.width, inputTexture.height);
            if (_resultTexture)
            {
                _saveController.SaveTexture(_resultTexture, path);
            }
        }

        public void EditeTextureToDivisorOf4(Texture2D inputTexture, string path)
        {
            _resultTexture = _textureController.ResizeTo4Devisor(inputTexture, inputTexture.width, inputTexture.height);
            if (_resultTexture)
            {
                _saveController.SaveTexture(_resultTexture, path);
            }
        }
    }
}
