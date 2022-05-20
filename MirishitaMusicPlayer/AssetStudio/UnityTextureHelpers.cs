using AssetStudio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.Linq;

namespace MirishitaMusicPlayer.AssetStudio
{
    internal static class UnityTextureHelpers
    {
        private static readonly AssetsManager assetsManager = AssetStudioGlobal.AssetsManager;

        public static AssetsManager Assets => assetsManager;

        public static void LoadFiles(params string[] paths)
        {
            assetsManager.LoadFiles(paths);
        }

        public static Bitmap GetBitmap(string name)
        {
            SerializedFile targetAsset = assetsManager.assetsFileList.Where(
                a => (a.Objects.Where(o => o.type == ClassIDType.Texture2D).First() as NamedObject).m_Name.StartsWith(name)).First();
            Texture2D texture = targetAsset.Objects.Where(o => o.type == ClassIDType.Texture2D).First() as Texture2D;

            Image<Bgra32> image = texture.ConvertToImage(true);

            PinnedBitmap bitmap = new(image.ConvertToBytes(), image.Width, image.Height);
            return bitmap.Bitmap;
        }
    }
}
