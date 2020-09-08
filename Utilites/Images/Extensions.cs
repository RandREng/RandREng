using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RandREng.Utility.Images
{
	public static class Extensions
	{
		/// <summary>
		/// Trims string after checking for null so no exception is thrown
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Bitmap ScaleImage(this Image image, int dpi)
		{
            double ratioX = (double)dpi / image.HorizontalResolution;
            double ratioY = (double)dpi / image.VerticalResolution;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratioX);
            int newHeight = (int)(image.Height * ratioY);

            Bitmap newImage = new Bitmap(newWidth, newHeight);
			newImage.SetResolution(dpi, dpi);
			Graphics g = Graphics.FromImage(newImage);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
			g.DrawImage(image, 0, 0, newWidth, newHeight);

			MemoryStream ms = new MemoryStream();
			newImage.Save(ms, ImageFormat.Tiff);
			ms.Flush();
			ms.Seek(0, SeekOrigin.Begin);
			Bitmap bm = new Bitmap(ms);
			return bm;


			//BitmapImage bi = new BitmapImage();

			//// Begin initialization.
			//bi.BeginInit();

			//bi.CacheOption = BitmapCacheOption.OnDemand;
			//bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
			//bi.StreamSource = ms;
			//bi.EndInit();

			//FormatConvertedBitmap formatConvertedBitmap = new FormatConvertedBitmap(bi, PixelFormats.Indexed1, BitmapPalettes.BlackAndWhite, .5);

			//MemoryStream ms2 = new MemoryStream();
			//TiffBitmapEncoder encoder = new TiffBitmapEncoder
			//{
			//	Compression = TiffCompressOption.None
			//};
			//encoder.Frames.Add(BitmapFrame.Create(formatConvertedBitmap));
			//encoder.Save(ms2);

			//ms2.Flush();
			//ms2.Seek(0, SeekOrigin.Begin);

			//Bitmap bm2 = new Bitmap(ms2);
			//return bm2;
		}

		public static string QRDecode(this Bitmap bm)
		{
			return bm.QRDecode(null);
		}

		public static string QRDecode(this Bitmap bm, ILogger logger)
		{
			string qrcode = null;

			//using (MemoryStream stream = new MemoryStream())
			//{

			//	Result r = null;

			//	Image<Rgba32> image = Image<Rgba32>.Load<Rgba32>(stream);

			//	IBarcodeReader reader = new BarcodeReader();
			//	reader.Options.TryHarder = true;
			//	reader.Options.PossibleFormats = new List<BarcodeFormat>();
			//	reader.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
			//	int dpi = (int)bm.HorizontalResolution;
			//	int originalDpi = dpi;
			//	try
			//	{
			//		//				for (int dpi = 20; dpi <= 100; dpi++)
			//		{
			//			//					using (Bitmap bm2 = bm.ScaleImage(dpi))
			//			{
			//				r = reader.Decode(image);
			//			}
			//			if (r != null && r.BarcodeFormat == BarcodeFormat.QR_CODE)
			//			{
			//				if (logger != null)
			//				{
			//					logger.Log(LogLevel.Information, string.Format("QRDecode - resolution {0} - {1}", originalDpi, dpi));
			//				}
			//				System.Console.WriteLine("QRDecode - resolution {0} - {1}", originalDpi, dpi);
			//				qrcode = r.Text;
			//				//						break;
			//			}
			//		}
			//	}
			//	catch (Exception)
			//	{
			//	}
			//}
			return qrcode;
		}
	}
}
