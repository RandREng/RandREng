using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace ReportServerBarcode
{
	public class QRCode
	{
		private const BarcodeFormat DEFAULT_BARCODE_FORMAT = BarcodeFormat.QR_CODE;
		private static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Png;
		private const String DEFAULT_OUTPUT_FILE = "out";
		private const int DEFAULT_WIDTH = 96;
		private const int DEFAULT_HEIGHT = 96;

		static public byte[] Encode(string content)
		{
			return Encode(content, DEFAULT_IMAGE_FORMAT, DEFAULT_WIDTH, DEFAULT_HEIGHT);
		}

		static public byte[] Encode(string content, ImageFormat imageFormat, int width, int height)
		{
			byte[] bytes = null;
			using (MemoryStream stream = new MemoryStream())
			{
				Encode(stream, content, imageFormat, width, height, 0);
				bytes = stream.ToArray();
				stream.Close();
			}
			return bytes;
		}

		static public string AutoGenerateTempfile(string content, ImageFormat imageFormat, int width, int height)
		{
			string fileName = "";
			fileName = Path.GetTempFileName();
			using (FileStream stream = new FileStream(fileName, FileMode.Create))
			{
				Encode(stream, content, imageFormat, width, height, 0);
			}
			return fileName;
		}


		static public void Encode(Stream stream, string contents, ImageFormat imageFormat, int width, int height, int margin)
		{
			BarcodeWriter barcodeWriter = new BarcodeWriter
			{
				Format = BarcodeFormat.QR_CODE,
				Options = new ZXing.QrCode.QrCodeEncodingOptions
				{
					ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.L,
					CharacterSet = "ISO-8859-1",
					Height = height,
					Width = width,
					Margin = margin
				}
			};
			Bitmap bitmap = barcodeWriter.Write(contents);
			bitmap.Save(stream, imageFormat);
		}
	}
}
