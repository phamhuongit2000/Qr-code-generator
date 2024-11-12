using System;
using System.IO;
using Qr;

namespace Qr
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ask the user for content input
            Console.WriteLine("Enter the content for the QR code (e.g., URL, text):");
            string content = Console.ReadLine();

            // Ask the user for the logo file path
            Console.WriteLine("Enter the path to the logo image (e.g., D:\\google_logo.png):");
            string logoPath = Console.ReadLine();

            // Ask the user for the output file paths
            Console.WriteLine("Enter the output file path for the QR code with logo (e.g., D:\\qr-with-logo.png):");
            string outputPath = Console.ReadLine();

            // Generate QR codes with and without logo
            byte[] qrCode = QRCodeUtil.CreateQRCodeWithLogo(content, logoPath);

            // Save the generated QR codes to the specified paths
            File.WriteAllBytes(outputPath, qrCode);

            // Notify the user of the results
            Console.WriteLine($"QR code with logo saved at: {outputPath}");
        }
    }
}
