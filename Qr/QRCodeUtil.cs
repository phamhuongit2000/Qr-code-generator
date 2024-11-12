using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System;
using ZXing.QrCode;
using ZXing;
using ZXing.QrCode.Internal;

namespace Qr
{
    public static class QRCodeUtil
    {
        private static QrCodeEncodingOptions CreateEncodeOption(int width, int height, int margin, int version)
        {
            return new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                QrVersion = version,
                Margin = margin,
                ErrorCorrection = ErrorCorrectionLevel.H
            };
        }

        public static byte[] CreateQRCodeWithLogo(string content, string logoPath, int width = 300, int height = 300, int margin = 0, int version = 3)
        {
            BarcodeWriter<Bitmap> writer = new BarcodeWriter<Bitmap>
            {
                Renderer = new ZXing.Rendering.BitmapRenderer(),
                Options = CreateEncodeOption(width, height, margin, version),
                Format = BarcodeFormat.QR_CODE
            };

            using (Bitmap qrCodeImage = writer.Write(content))
            {
                // Read logo and resize
                Bitmap logo = new Bitmap(logoPath);
                Image resizedLogo = ResizeImageBaseOnQrCode(logo, qrCodeImage.Size);

                // Draw the logo onto the QR code
                using (Graphics graphics = Graphics.FromImage(qrCodeImage))
                {
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(resizedLogo, (qrCodeImage.Width - resizedLogo.Width) / 2, (qrCodeImage.Height - resizedLogo.Height) / 2);
                }

                // Convert the image to byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    return ms.ToArray();  // Return the byte array
                }
            }
        }

        // Method to create a QR Code without a logo
        public static byte[] CreateQRCodeWithoutLogo(string content, int width = 300, int height = 300, int margin = 0, int version = 3)
        {
            BarcodeWriter<Bitmap> writer = new BarcodeWriter<Bitmap>
            {
                Renderer = new ZXing.Rendering.BitmapRenderer(),
                Options = CreateEncodeOption(width, height, margin, version),
                Format = BarcodeFormat.QR_CODE
            };

            using (Bitmap qrCodeImage = writer.Write(content))
            {
                // Convert the QR code image to byte array
                using (MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    return ms.ToArray();  // Return the byte array
                }
            }
        }

        private static Image ResizeImageBaseOnQrCode(Image imgToResize, Size qrCodeSize)
        {
            float logoScale = 0.3f; // 30% of the QR code size
            int logoWidth = (int)(qrCodeSize.Width * logoScale);
            int logoHeight = (int)(qrCodeSize.Height * logoScale);

            // Maintain aspect ratio
            float scale = Math.Min((float)logoWidth / imgToResize.Width, (float)logoHeight / imgToResize.Height);
            logoWidth = (int)(imgToResize.Width * scale);
            logoHeight = (int)(imgToResize.Height * scale);

            Bitmap resizedLogo = new Bitmap(logoWidth, logoHeight);
            using (Graphics g = Graphics.FromImage(resizedLogo))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, logoWidth, logoHeight);
            }
            return resizedLogo;
        }
    }
}
