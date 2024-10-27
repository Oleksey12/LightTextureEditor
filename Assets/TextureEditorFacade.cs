namespace LightTextureEditor
{
    using UnityEngine;

    public class TextureEditorFacade
    {
        public enum Modes
        {
            DivisorOf4,
            PowerOf2,
            CustomSize
        };

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
            _saveController.SaveTexture(_resultTexture, path);
        }

        public void EditTexture(Texture2D inputTexture, string path, Modes mode)
        {
            if (mode == Modes.DivisorOf4)
            {
                _resultTexture = _textureController.ResizeTo4Devisor(inputTexture, inputTexture.width, inputTexture.height);
            }
            else if (mode == Modes.PowerOf2)
            {
                _resultTexture = _textureController.ResizeTo2Power(inputTexture, inputTexture.width, inputTexture.height);
            }

            _saveController.SaveTexture(_resultTexture, path);
        }
    }
}
