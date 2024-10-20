namespace LightTextureEditor
{
    using System;
    using UnityEngine;

    public class TextureEditorFacade
    {
        public enum Modes
        {
            DivisorOf4,
            PowerOf2
        };

        private static readonly Lazy<SaveController> _saveController = new Lazy<SaveController>(() => new SaveController());
        private static readonly Lazy<TextureController> _textureController = new Lazy<TextureController>(() => new TextureController());

        private Texture2D _resultTexture = default;

        public void EditTexture(Texture2D inputTexture, string path, int width, int height)
        {
            _resultTexture = _textureController.Value.Resize(inputTexture, width, height);
            _saveController.Value.SaveTexture(_resultTexture, path);
        }

        public void EditTexture(Texture2D inputTexture, string path, Modes mode)
        {
            if (mode == Modes.DivisorOf4)
            {
                _resultTexture = _textureController.Value.ResizeTo4Devisor(inputTexture, inputTexture.width, inputTexture.height);
            }
            else if (mode == Modes.PowerOf2)
            {
                _resultTexture = _textureController.Value.ResizeTo2Power(inputTexture, inputTexture.width, inputTexture.height);
            }

            _saveController.Value.SaveTexture(_resultTexture, path);
        }
    }
}
