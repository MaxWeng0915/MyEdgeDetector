using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace EdgeDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter the image path (8-bit image)：");
                string imagePath = Console.ReadLine();

                // load image to MyMat
                MyMat mat = MyMat.LoadFromFile(imagePath);            
            
                //ask the user to choose an edge detection method.
                Console.WriteLine("Please choose an edge detection method：1. Sobel  2. Prewitt");
                string methodChoice = Console.ReadLine();
                EdgeDetectionMethod method = methodChoice == "1" ? EdgeDetectionMethod.Sobel : EdgeDetectionMethod.Prewitt;

                // apply process
                MyMat edgeMatrix = EdgeDetector.DoProcess(mat, method);


                Console.WriteLine("Please enter the path to save the image：");
                string savePath = Console.ReadLine();

                // Save the processed 8-bit grayscale image
                edgeMatrix.SaveAsImage(savePath);

                Console.WriteLine($"Edge detection complete. The image has been saved as an bmp image. Path：{savePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error：{ex.Message}");
            }
        }
    }
}
