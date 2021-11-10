using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace WpfView
{
    public static class ImageCache
    {
        private static readonly Dictionary<string, Bitmap> Cache = new();

        public static void ClearCache()
        {
            Cache.Clear();
        }

        public static Bitmap GetImageFromCache(string key)
        {
            if (!Cache.ContainsKey(key))
            {
                Cache.Add(key, new Bitmap(key));
            }

            return (Bitmap)Cache[key].Clone();
        }

        public static Bitmap CreateBitmap(int width, int height)
        {
            const string key = "empty";
            if (!Cache.ContainsKey(key))
            {
                Cache.Add(key, new Bitmap(width, height));
                Graphics graphics = Graphics.FromImage(Cache[key]);
                graphics.Clear(ColorTranslator.FromHtml("#AECC48"));
            }

            return (Bitmap)Cache[key].Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}