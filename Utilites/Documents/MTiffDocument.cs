using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace RandREng.Utility.Documents
{
	public class MTiffDocument : IDocument
	{
		TiffBitmapEncoder encoder;
		Stream imageStreamSource;
		TiffBitmapDecoder decoder;

		/// <summary>
		/// 
		/// </summary>
		public string FileName { get; set; }

		public MTiffDocument()
		{
		}

		~MTiffDocument()
		{
			if (imageStreamSource != null)
			{
				imageStreamSource.Dispose();
				imageStreamSource = null;
			}
		}

		public void Open(string fileName)
		{
			imageStreamSource = new MemoryStream();
			Bitmap b = new Bitmap(fileName);
			b.Save(imageStreamSource, ImageFormat.Tiff);
			b.Dispose();
			imageStreamSource.Seek(0, SeekOrigin.Begin);
			decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);
		}

		public int Count { get { return decoder.Frames.Count; } }

		public Bitmap GetImage(int index)
		{
			Bitmap bm = null;
			using (MemoryStream mem = new MemoryStream())
			{
				TiffBitmapEncoder _encoder = new TiffBitmapEncoder();
				BitmapSource bs = decoder.Frames[index];
				_encoder.Compression = TiffCompressOption.Ccitt4;
				_encoder.Frames.Add(decoder.Frames[index]);
				_encoder.Save(mem);
				mem.Flush();
				mem.Seek(0, SeekOrigin.Begin);
				bm = new Bitmap(mem);
			}
			return bm;
		}


		static public void Split(string fileName)
		{
			using (Stream imageStreamSource = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				TiffBitmapDecoder decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnDemand);

				int count = decoder.Frames.Count;
				for (int _i = 0; _i < count; _i++)
				{
					string outName = Path.GetDirectoryName(fileName) + @"\" + Path.GetFileNameWithoutExtension(fileName) + "_PAGE" + _i + Path.GetExtension(fileName);

					using (FileStream splitFiles = new FileStream(outName, FileMode.Create))
					{
                        TiffBitmapEncoder encoder = new TiffBitmapEncoder
                        {
                            Compression = TiffCompressOption.Ccitt4
                        };
                        List<ColorContext> c = new List<ColorContext>();
						c.Add(new ColorContext(PixelFormats.BlackWhite));
						encoder.ColorContexts = new System.Collections.ObjectModel.ReadOnlyCollection<ColorContext>(c);
						encoder.Frames.Add(decoder.Frames[_i]);
						encoder.Save(splitFiles);
					}
				}
			}
			return;
		}

		public void Add(Bitmap bm)
		{
			Add(bm, RotateFlipType.RotateNoneFlipNone);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="image"></param>
		public void Add(Bitmap bm, RotateFlipType rotate)
		{
			Bitmap image = bm;

			if (rotate != RotateFlipType.RotateNoneFlipNone)
			{
				bm.RotateFlip(rotate);
			}

			if (encoder == null)
			{
                encoder = new TiffBitmapEncoder
                {
                    Compression = TiffCompressOption.Ccitt4
                };
            }
			using (MemoryStream mem = new MemoryStream())
			{
				// Lock the bitmap's bits.  
				Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
				BitmapData bmpData = bm.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);

				// Get the address of the first line.
				IntPtr ptr = bmpData.Scan0;

				// Declare an array to hold the bytes of the bitmap.
				int bytes = Math.Abs(bmpData.Stride) * image.Height;
				byte[] rgbValues = new byte[bytes];

				// Copy the RGB values into the array.
				List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
				colors.Add(Colors.Black);
				colors.Add(Colors.White);
				BitmapPalette myPalette = new BitmapPalette(colors);
				BitmapSource bs = BitmapSource.Create(image.Width, image.Height, image.HorizontalResolution, image.VerticalResolution, PixelFormats.Indexed1, myPalette, ptr, bytes, bmpData.Stride);
				encoder.Frames.Add(BitmapFrame.Create(bs));
				// Unlock the bits.
				image.UnlockBits(bmpData);


			}
		}

		public void Save(Bitmap bm, string filename)
		{
			Save(bm, filename, RotateFlipType.RotateNoneFlipNone);
		}

		public void Save(Bitmap bm, string filename, RotateFlipType rotate)
		{
			Bitmap image = bm;

			using (FileStream stream = new FileStream(filename, FileMode.Create))
			{
				if (rotate != RotateFlipType.RotateNoneFlipNone)
				{
					image.RotateFlip(rotate);
				}

                TiffBitmapEncoder _encoder = new TiffBitmapEncoder
                {
                    Compression = TiffCompressOption.Ccitt4
                };


                // Lock the bitmap's bits.  
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
				BitmapData bmpData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);

				// Get the address of the first line.
				IntPtr ptr = bmpData.Scan0;

				// Declare an array to hold the bytes of the bitmap.
				int bytes = Math.Abs(bmpData.Stride) * image.Height;
				byte[] rgbValues = new byte[bytes];

				// Copy the RGB values into the array.
				List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
				colors.Add(Colors.Black);
				colors.Add(Colors.White);
				BitmapPalette myPalette = new BitmapPalette(colors);
				BitmapSource bs = BitmapSource.Create(image.Width, image.Height, image.HorizontalResolution, image.VerticalResolution, PixelFormats.BlackWhite, myPalette, ptr, bytes, bmpData.Stride);
				_encoder.Frames.Add(BitmapFrame.Create(bs));
				// Unlock the bits.
				image.UnlockBits(bmpData);
				_encoder.Save(stream);
				_encoder = null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Close()
		{
			if (encoder != null)
			{
				FileStream stream = new FileStream(FileName, FileMode.Create);
				encoder.Save(stream);
				encoder = null;
				stream.Dispose();
			}

			if (imageStreamSource != null)
			{
				imageStreamSource.Close();
				imageStreamSource.Dispose();
				imageStreamSource = null;
			}

		}
	}
}
