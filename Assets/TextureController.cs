namespace LightTextureEditor
{
    using System;
    using UnityEngine;

    public class TextureController
    {
        private Texture2D _resultTexture = default;
        private Rect _textureCanvas = new Rect(0, 0, 0, 0);

        private int _powWidth = 0;
        private int _powHeight = 0;

        public virtual Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(texture2D.width, texture2D.height, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);

            _resultTexture = new Texture2D(targetX, targetY);

            _textureCanvas = new Rect(0, 0, targetX, targetY);
            _resultTexture.ReadPixels(_textureCanvas, 0, 0);
            _resultTexture.Apply();
            return _resultTexture;
        }

        public virtual Texture2D ResizeTo2Power(Texture2D texture2D, int width, int height)
        {
            _powWidth = (int)Math.Log(2, width);
            _powHeight = (int)Math.Log(2, width);

            return Resize(texture2D, (int)Math.Pow(2, _powWidth), (int)Math.Pow(2, _powHeight));
        }

        public virtual Texture2D ResizeTo4Devisor(Texture2D texture2D, int width, int height)
        {
            return Resize(texture2D, width - width % 4, height - height % 4);
        }
    }
}
