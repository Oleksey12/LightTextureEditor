namespace LightTextureEditor
{
    using UnityEngine;

    /// <summary>
    /// Фасад для вызова обработки разных способов масштабирования текстур
    /// </summary>
    public class TextureEditorFacade
    {
        protected readonly SaveController saveController = default;
        protected readonly TextureController textureController = default;

        protected Texture2D resultTexture = default;

        /// <summary>
        /// Конструктор фасада
        /// </summary>
        public TextureEditorFacade()
        {
            saveController = new SaveController();
            textureController = new TextureController();
        }

        /// <summary>
        /// Масштабирует длину и ширину до новых значений
        /// </summary>
        public virtual void EditTexture(Texture2D inputTexture, string path, int width, int height)
        {
            resultTexture = textureController.Resize(inputTexture, width, height);
            if (resultTexture)
            {
                saveController.SaveTexture(resultTexture, path);
            }
        }

        /// <summary>
        /// Масштабирует длину и ширину текстуры до значений, являющимися степенью двойки
        /// </summary>
        public virtual void EditTextureToPowerOf2(Texture2D inputTexture, string path)
        {
            resultTexture = textureController.ResizeTo2Power(inputTexture, inputTexture.width, inputTexture.height);
            if (resultTexture)
            {
                saveController.SaveTexture(resultTexture, path);
            }
        }

        /// <summary>
        /// Масштабирует длину и ширину текстуры до значений, кратным четырём
        /// </summary>
        public virtual void EditeTextureToDivisorOf4(Texture2D inputTexture, string path)
        {
            resultTexture = textureController.ResizeTo4Devisor(inputTexture, inputTexture.width, inputTexture.height);
            if (resultTexture)
            {
                saveController.SaveTexture(resultTexture, path);
            }
        }
    }
}
