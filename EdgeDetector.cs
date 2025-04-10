using System;

namespace EdgeDetection
{
    public enum EdgeDetectionMethod
    {
        Sobel,
        Prewitt
    }

    public class EdgeDetector
    {
        private static readonly int[,] sobelX = {
            { -1, 0, 1 },
            { -2, 0, 2 },
            { -1, 0, 1 }
        };

        private static readonly int[,] sobelY = {
            { -1, -2, -1 },
            {  0,  0,  0 },
            {  1,  2,  1 }
        };

        private static readonly int[,] prewittX = {
            { -1, 0, 1 },
            { -1, 0, 1 },
            { -1, 0, 1 }
        };

        private static readonly int[,] prewittY = {
            { -1, -1, -1 },
            {  0,  0,  0 },
            {  1,  1,  1 }
        };

        public static MyMat DoProcess(MyMat image, EdgeDetectionMethod method)
        {
            switch (method)
            {
                case EdgeDetectionMethod.Sobel:
                    return Sobel(image);
                case EdgeDetectionMethod.Prewitt:
                    return Prewitt(image);
                default:
                    throw new ArgumentException("Unsupported edge detection method.");
            }
        }

        private static MyMat Sobel(MyMat image)
        {
            return convolution(image, sobelX, sobelY);
        }

        private static MyMat Prewitt(MyMat image)
        {
            return convolution(image, prewittX, prewittY);
        }

        private static MyMat convolution(MyMat image, int[,] kernelX, int[,] kernelY)
        {
            int width = image.Width;
            int height = image.Height;
            MyMat result = new MyMat(width, height);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    int gx = 0;
                    int gy = 0;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            gx += image.Data[x + i, y + j] * kernelX[i + 1, j + 1];
                            gy += image.Data[x + i, y + j] * kernelY[i + 1, j + 1];
                        }
                    }

                    int edgeStrength = (int)Math.Min(Math.Sqrt(gx * gx + gy * gy), 255);
                     result.SetValue(x, y, edgeStrength);
                }
            }

            return result;
        }
    }
}
