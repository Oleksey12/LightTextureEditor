namespace LightTextureEditor
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Сохранение отредактированной текстуры
    /// </summary>
    public class SaveController
    {
        protected byte[] imageData = default;

        /// <summary>
        /// Сохраняет новую, масштабированную текстуру
        /// </summary>
        public virtual void SaveTexture(Texture2D texture2D, string path)
        {
            imageData = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, imageData);
            AssetDatabase.Refresh();
        }
    }
}
