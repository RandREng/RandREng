using System;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;

namespace ReportServerBarcode
{
    public class QRCode
	{
        private static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Png;
        private const int DEFAULT_WIDTH = 96;
		private const int DEFAULT_HEIGHT = 96;

		public static byte[] Encode(string content)
		{
			return Encode(content, DEFAULT_IMAGE_FORMAT, DEFAULT_WIDTH, DEFAULT_HEIGHT);
		}

		public static byte[] Encode(string content, ImageFormat imageFormat, int width, int height)
		{
			byte[] bytes = null;
			using (MemoryStream stream = new())
			{
				Encode(stream, content, imageFormat, width, height, 0);
				bytes = stream.ToArray();
				stream.Close();
			}
			return bytes;
		}

		public static string AutoGenerateTempfile(string content, ImageFormat imageFormat, int width, int height)
		{
			string fileName = "";
			fileName = Path.GetTempFileName();
			using (FileStream stream = new(fileName, FileMode.Create))
			{
				Encode(stream, content, imageFormat, width, height, 0);
			}
			return fileName;
		}


		public static void Encode(Stream stream, string contents, ImageFormat imageFormat, int width, int height, int margin)
		{
			BarcodeWriterPixelData barcodeWriter = new()
            {
				Format = BarcodeFormat.QR_CODE,
				Options = new QrCodeEncodingOptions
				{
					ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.L,
					CharacterSet = "ISO-8859-1",
					Height = height,
					Width = width,
					Margin = margin
				}
			};
            ZXing.Rendering.PixelData pixelData = barcodeWriter.Write(contents);
			// creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
			// that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
			using (System.Drawing.Bitmap bitmap = new(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
			{
                BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
				System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
				try
				{
					// we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
					System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
					pixelData.Pixels.Length);
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
				}
				// save to stream as PNG
				bitmap.Save(stream, imageFormat);
			}
		}
	}
}
