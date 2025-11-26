namespace LightTextureEditor
{
    using UnityEngine;
    using System;

    /// <summary>
    /// Логика масштабирования текстур
    /// </summary>
    public class TextureController
    {
        protected Texture2D resultTexture = default;
        protected Rect textureCanvas = new Rect(0, 0, 0, 0);

        protected int powWidth = 0;
        protected int powHeight = 0;

        /// <summary>
        /// Масштабирует текстуру до заданных значений длины и ширины
        /// </summary>
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

            resultTexture = new Texture2D(targetX, targetY);

            textureCanvas = new Rect(0, 0, targetX, targetY);
            try
            {
                resultTexture.ReadPixels(textureCanvas, 0, 0);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при масштабировании текстуры {nameof(ex)}: {ex.Message}");
            }
            resultTexture.Apply();
            return resultTexture;
        }

        /// <summary>
        /// Масштабировать длину и ширину текстуры до степени двойки
        /// </summary>
        public virtual Texture2D ResizeTo2Power(Texture2D texture2D, int width, int height)
        {
            powWidth = (int)Math.Log(width, 2);
            powHeight = (int)Math.Log(height, 2);

            return Resize(texture2D, (int)Math.Pow(2, powWidth), (int)Math.Pow(2, powHeight));
        }

        /// <summary>
        /// Масштабировать длину и ширину до значений, кратных четырём
        /// </summary>
        public virtual Texture2D ResizeTo4Devisor(Texture2D texture2D, int width, int height)
            => Resize(texture2D, width - width % 4, height - height % 4);
    }
}
