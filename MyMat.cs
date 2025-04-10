using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace EdgeDetection
{
    //MyMat only support 8 bit image format    
    
    /*
    Loading images into the MyMat format can simplify the process, 
    so that developers don't need to convert formats each time. 
    This allows algorithm development to focus more easily on data research and development.
    */

    public class MyMat
    {
        public int[,] Data { get; }
        public int Width { get; }
        public int Height { get; }

        public MyMat(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new int[width, height];
        }

        public MyMat(int[,] data, int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }

        public int GetValue(int x, int y)
        {
            return Data[x, y];
        }

        public void SetValue(int x, int y, int value)
        {
            Data[x, y] = value;
        }

         // Static method: Read Bitmap from file path to MyMat
        public static MyMat LoadFromFile(string path)
        {
            Bitmap bmp = new Bitmap(path);
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException("only allow 8 bit image");
            return FromBitmap(bmp);
        }

        // Save MyMat as a Bitmap to the specified path.
        public void SaveAsImage(string path)
        {
            Bitmap bmp = this.ToBitmap();
            bmp.Save(path, ImageFormat.Bmp);
        }


        // convert Bitmap to MyMat.
        private static MyMat FromBitmap(Bitmap bmp)
        {
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException("Bitmap must be 8 bit.");

            int width = bmp.Width;
            int height = bmp.Height;
            MyMat mat = new MyMat(width, height);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            int stride = bmpData.Stride;
            byte[] buffer = new byte[stride * height];
            Marshal.Copy(bmpData.Scan0, buffer, 0, buffer.Length);
            bmp.UnlockBits(bmpData);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int idx = y * stride + x;
                    mat.Data[x, y] = buffer[idx];
                }
            }

            return mat;
        }

        // Convert MyMat to Bitmap
        private Bitmap ToBitmap()
        {
            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed);

            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp.Palette = palette;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            int stride = bmpData.Stride;
            byte[] buffer = new byte[stride * Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int idx = y * stride + x;
                    buffer[idx] = (byte)Math.Clamp(Data[x, y], 0, 255);
                }
            }

            Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

       
    }
}
