namespace LightTextureEditor
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    public class SaveController
    {
        private byte[] _imageData = default;

        public void SaveTexture(Texture2D texture2D, string path)
        {
            _imageData = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, _imageData);
            AssetDatabase.Refresh();
        }
    }
}
