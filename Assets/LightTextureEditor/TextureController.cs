namespace LightTextureEditor
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Логика масштабирования текстур
    /// </summary>
    public class TextureController
    {
        protected Texture2D _resultTexture = default;
        protected Rect _textureCanvas = new Rect(0, 0, 0, 0);

        protected int _powWidth = 0;
        protected int _powHeight = 0;

        public virtual Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            if (texture2D.width == targetX && texture2D.height == targetY)
            {
                return null;
            }
            
            // ВАЖНО! Мы передаем в Renderer чистую текстуру без всяких фильтраций
            texture2D.filterMode = FilterMode.Point;

            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);

            _resultTexture = new Texture2D(targetX, targetY);

            _textureCanvas = new Rect(0, 0, targetX, targetY);
            try
            {
                _resultTexture.ReadPixels(_textureCanvas, 0, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при масштабировании текстуры {nameof(ex)}: {ex.Message}");
            }
            _resultTexture.Apply();
            return _resultTexture;
        }

        public virtual Texture2D ResizeTo2Power(Texture2D texture2D, int width, int height)
        {
            _powWidth = (int)Math.Log(width, 2);
            _powHeight = (int)Math.Log(height, 2);

            return Resize(texture2D, (int)Math.Pow(2, _powWidth), (int)Math.Pow(2, _powHeight));
        }

        public virtual Texture2D ResizeTo4Devisor(Texture2D texture2D, int width, int height)
        {
            return Resize(texture2D, width - width % 4, height - height % 4);
        }
    }
}
