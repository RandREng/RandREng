using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RandREng.Utility.Documents
{

	internal class ImageRenderListener : IRenderListener
	{
		#region Fields

		public Bitmap Image = null;
		Dictionary<System.Drawing.Image, string> images = new Dictionary<System.Drawing.Image, string>();
		#endregion Fields

		#region Properties

		public Dictionary<System.Drawing.Image, string> Images
		{
			get { return images; }
		}
		#endregion Properties

		#region Methods

		#region Public Methods

		public void BeginTextBlock() { }

		public void EndTextBlock() { }

		public void RenderImage(ImageRenderInfo renderInfo)
		{
			PdfImageObject image = renderInfo.GetImage();
			PdfName filter = (PdfName)image.Get(PdfName.FILTER);

			int width = Convert.ToInt32(image.Get(PdfName.WIDTH).ToString());
			int bitsPerComponent = Convert.ToInt32(image.Get(PdfName.BITSPERCOMPONENT).ToString());
			string subtype = image.Get(PdfName.SUBTYPE).ToString();
			int height = Convert.ToInt32(image.Get(PdfName.HEIGHT).ToString());
			int length = Convert.ToInt32(image.Get(PdfName.LENGTH).ToString());
			string colorSpace = image.Get(PdfName.COLORSPACE).ToString();

			/* It appears to be safe to assume that when filter == null, PdfImageObject 
			 * does not know how to decode the image to a System.Drawing.Image. 
			 * 
			 * Uncomment the code above to verify, but when I’ve seen this happen, 
			 * width, height and bits per component all equal zero as well. */
			if (filter != null)
			{
				Bitmap drawingImage = image.GetDrawingImage() as Bitmap;

				string extension = ".";

				if (filter == PdfName.DCTDECODE)
				{
					extension += PdfImageObject.ImageBytesType.JPG.FileExtension;
				}
				else if (filter == PdfName.JPXDECODE)
				{
					extension += PdfImageObject.ImageBytesType.JP2.FileExtension;
				}
				else if (filter == PdfName.FLATEDECODE)
				{
					extension += PdfImageObject.ImageBytesType.PNG.FileExtension;
				}
				else if (filter == PdfName.LZWDECODE)
				{
					extension += PdfImageObject.ImageBytesType.CCITT.FileExtension;
				}

				/* Rather than struggle with the image stream and try to figure out how to handle 
				 * BitMapData scan lines in various formats (like virtually every sample I’ve found 
				 * online), use the PdfImageObject.GetDrawingImage() method, which does the work for us. */
				Images.Add(drawingImage, extension);
				Image = drawingImage;
			}
		}
		public void RenderText(TextRenderInfo renderInfo) { }

		//public void test()
		//{

		//	PdfSharp.Pdf.Filters.FlateDecode flate = new PdfSharp.Pdf.Filters.FlateDecode();
		//	byte[] decodedBytes = flate.Decode(image.Stream.Value);

		//	System.Drawing.Imaging.PixelFormat pixelFormat;

		//	switch (bitsPerComponent)
		//	{
		//		case 1:
		//			pixelFormat = PixelFormat.Format1bppIndexed;
		//			break;
		//		case 8:
		//			pixelFormat = PixelFormat.Format8bppIndexed;
		//			break;
		//		case 24:
		//			pixelFormat = PixelFormat.Format24bppRgb;
		//			break;
		//		default:
		//			throw new Exception("Unknown pixel format " + bitsPerComponent);
		//	}

		//	Bitmap bmp = new Bitmap(width, height, pixelFormat);
		//	var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
		//	int length = (int)Math.Ceiling(width * bitsPerComponent / 8.0);
		//	for (int i = 0; i < height; i++)
		//	{
		//		int offset = i * length;
		//		int scanOffset = i * bmpData.Stride;
		//		Marshal.Copy(decodedBytes, offset, new IntPtr(bmpData.Scan0.ToInt32() + scanOffset), length);
		//	}
		//	bmp.UnlockBits(bmpData);
		//	using (FileStream fs = new FileStream(@"D:\BeyondBGCM\BeyondBGCM\PDFToWord\" + String.Format("Image{0}.png", count++), FileMode.Create, FileAccess.Write))
		//	{
		//		bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
		//	}
		//}

		#endregion Public Methods

		#endregion Methods
	}

	public class MyImageRenderListener : IRenderListener
	{
		public void RenderText(TextRenderInfo renderInfo) { }
		public void BeginTextBlock() { }
		public void EndTextBlock() { }

		public Bitmap Image = null;
		public void RenderImage(ImageRenderInfo renderInfo)
		{
			try
			{
				PdfImageObject image = renderInfo.GetImage();
				if (image == null) return;

				using (MemoryStream ms = new MemoryStream(image.GetImageAsBytes()))
				{
					Bitmap i = (Bitmap)System.Drawing.Image.FromStream(ms);
					Image = (Bitmap)i.Clone();
					i.Dispose();
					//                    int dpi = i.Height / 11;
					int yDPI = Image.Height / 11;
					int xDPI = Image.Width * 2 / 17;

					xDPI = Math.Abs(xDPI - 300) < 10 ? 300 : xDPI;
					yDPI = Math.Abs(yDPI - 300) < 10 ? 300 : yDPI;
					xDPI = Math.Abs(xDPI - 600) < 10 ? 600 : xDPI;
					yDPI = Math.Abs(yDPI - 600) < 10 ? 600 : yDPI;

					if (xDPI == yDPI)
					{
						Image.SetResolution(xDPI, yDPI);
					}
					else
					{

					}
				}
			}
			catch (IOException)
			{
				/*
                 * pass-through; image type not supported by iText[Sharp]; e.g. jbig2
                */
			}
		}
	}

	public class PDFDocument : IDocument
	{
		public string FileName { get; set; }
		~PDFDocument()
		{
			Close();
		}

		public static void AddImage(Stream inputPdfStream, Stream outputPdfStream, Stream inputImageStream)
		{
			PdfReader reader = new PdfReader(inputPdfStream);
			iTextSharp.text.Rectangle size = reader.GetPageSize(1);

			iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
			image.SetAbsolutePosition(size.Width - 98, size.Height - 98);

			PdfStamper stamper = new PdfStamper(reader, outputPdfStream);

			int page = 1;

			//            for (int page = 1; page <= reader.NumberOfPages; page++)
			{

				PdfContentByte pdfContentByte = stamper.GetOverContent(page);

				pdfContentByte.AddImage(image);
			}
			stamper.Close();
		}

		public void Open(string fileName)
		{
			reader = new PdfReader(fileName);
			parser = new PdfReaderContentParser(reader);
			listener = new MyImageRenderListener();

		}

		public int Count { get { return reader.NumberOfPages; } }

		public Bitmap GetImage(int index)
		{
			return GetImage2(index);
			//parser.ProcessContent(index + 1, listener);
			//return listener.Image;
		}

		public Bitmap GetImage2(int index)
		{
			PdfDictionary page = reader.GetPageN(index + 1);
			return GetImagesFromPdfDict(page);
		}

		private PdfReaderContentParser parser = null;
		private MyImageRenderListener listener = null;

		private PdfReader reader = null;

		private Bitmap GetImagesFromPdfDict(PdfDictionary dict)
		{
			PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(dict.Get(PdfName.RESOURCES));
			PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
			Bitmap bm = null;
			if (xobj != null)
			{
				foreach (PdfName name in xobj.Keys)
				{
					PdfObject obj = xobj.Get(name);
					if (obj.IsIndirect())
					{
						PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
						PdfName subtype = (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
						if (PdfName.IMAGE.Equals(subtype))
						{
							int xrefIdx = ((PRIndirectReference)obj).Number;
							PdfObject pdfObj = reader.GetPdfObject(xrefIdx);
							PRStream str = (PRStream)pdfObj;

							PdfArray decode = tg.GetAsArray(PdfName.DECODE);
							int width = tg.GetAsNumber(PdfName.WIDTH).IntValue;
							int height = tg.GetAsNumber(PdfName.HEIGHT).IntValue;
							int bpc = tg.GetAsNumber(PdfName.BITSPERCOMPONENT).IntValue;
							var filter = tg.Get(PdfName.FILTER);

							if (filter.Equals(PdfName.FLATEDECODE))
							{
								var imageBytes = PdfReader.GetStreamBytesRaw(str);

								var decodedBytes = PdfReader.FlateDecode(imageBytes); //decode the raw image
								var streamBytes = PdfReader.DecodePredictor(decodedBytes, str.GetAsDict(PdfName.DECODEPARMS)); //decode predict to filter the bytes
								var pixelFormat = PixelFormat.Format1bppIndexed;
								switch (bpc) //determine the BPC
								{
									case 1:
										pixelFormat = PixelFormat.Format1bppIndexed;
										break;
									case 8:
										pixelFormat = PixelFormat.Format8bppIndexed;
										break;
									case 24:
										pixelFormat = PixelFormat.Format24bppRgb;
										break;
								}

								bm = new Bitmap(width, height, pixelFormat);
								{
									var bmpData = bm.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat);
									var length = (int)Math.Ceiling(width * bpc / 8.0);
									for (int i = 0; i < height; i++)
									{
										int offset = i * length;
										int scanOffset = i * bmpData.Stride;
										Marshal.Copy(streamBytes, offset, new IntPtr(bmpData.Scan0.ToInt32() + scanOffset), length);
									}
									bm.UnlockBits(bmpData);
								}
							}
							else
							{
								PdfImageObject pdfImage = new PdfImageObject(str);

								bm = (Bitmap)pdfImage.GetDrawingImage();
							}
							int yDPI = bm.Height / 11;
							int xDPI = bm.Width * 2 / 17;

							xDPI = Math.Abs(xDPI - 300) < 10 ? 300 : xDPI;
							yDPI = Math.Abs(yDPI - 300) < 10 ? 300 : yDPI;
							xDPI = Math.Abs(xDPI - 600) < 10 ? 600 : xDPI;
							yDPI = Math.Abs(yDPI - 600) < 10 ? 600 : yDPI;

							if (xDPI == yDPI)
							{
								bm.SetResolution(xDPI, yDPI);
							}
							else
							{

							}
							break;
						}
						else if (PdfName.FORM.Equals(subtype) || PdfName.GROUP.Equals(subtype))
						{
							GetImagesFromPdfDict(tg);
						}
					}
				}
			}
			return bm;
		}

		public void Split(string fileName)
		{
			throw new NotImplementedException();
		}

		public void Save(Bitmap bm, string filename)
		{
			Save(bm, filename, RotateFlipType.RotateNoneFlipNone);
		}

		const float PAGE_LEFT_MARGIN = 0;
		const float PAGE_RIGHT_MARGIN = 0;
		const float PAGE_TOP_MARGIN = 0;
		const float PAGE_BOTTOM_MARGIN = 0;

		public void Save(Bitmap bm, string filename, RotateFlipType rotate)
		{
			Bitmap image = bm;

			if (rotate != RotateFlipType.RotateNoneFlipNone)
			{
				image.RotateFlip(rotate);
			}

			using (FileStream stream = new FileStream(filename, FileMode.Create))
			{
				using (Document pdfDocument = new Document(PageSize.LETTER, PAGE_LEFT_MARGIN, PAGE_RIGHT_MARGIN, PAGE_TOP_MARGIN, PAGE_BOTTOM_MARGIN))
				{
					PdfWriter writer = PdfWriter.GetInstance(pdfDocument, stream);
					pdfDocument.Open();

					MemoryStream ms = new MemoryStream();
					image.Save(ms, ImageFormat.Tiff);
					iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ms);
					img.ScaleToFit(PageSize.LETTER.Width - (PAGE_LEFT_MARGIN + PAGE_RIGHT_MARGIN), PageSize.LETTER.Height - (PAGE_TOP_MARGIN + PAGE_BOTTOM_MARGIN));
					pdfDocument.Add(img);

					pdfDocument.Close();
					writer.Close();
				}
			}
		}

		public void Add(Bitmap bm)
		{
			Add(bm, RotateFlipType.RotateNoneFlipNone);
		}

		FileStream stream;
		Document pdfDocument;
		PdfWriter writer;

		public void Add(Bitmap bm, RotateFlipType rotate)
		{
			if (stream == null)
			{
				stream = new FileStream(FileName, FileMode.Create);
				pdfDocument = new Document(PageSize.LETTER, PAGE_LEFT_MARGIN, PAGE_RIGHT_MARGIN, PAGE_TOP_MARGIN, PAGE_BOTTOM_MARGIN);
				writer = PdfWriter.GetInstance(pdfDocument, stream);
				pdfDocument.Open();
			}

			Bitmap image = bm;

			if (rotate != RotateFlipType.RotateNoneFlipNone)
			{
				image.RotateFlip(rotate);
			}

			{
				iTextSharp.text.Image img = getImage(image);
				img.ScaleToFit(PageSize.LETTER.Width - (PAGE_LEFT_MARGIN + PAGE_RIGHT_MARGIN), PageSize.LETTER.Height - (PAGE_TOP_MARGIN + PAGE_BOTTOM_MARGIN));
				pdfDocument.Add(img);
			}
		}

		iTextSharp.text.Image getImage(Bitmap image)
		{
			if (image.PixelFormat == PixelFormat.Format1bppIndexed)
			{
				int w = image.Width;
				int h = image.Height;

				System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, image.Width, image.Height);
				BitmapData bmpData =
					image.LockBits(rect, ImageLockMode.ReadWrite,
					image.PixelFormat);

				// Get the address of the first line.
				IntPtr ptr = bmpData.Scan0;
				int bytes = Math.Abs(bmpData.Stride) * image.Height;
				byte[] rgbValues = new byte[bytes];

				int byteWidth = w / 8 + ((w & 7) != 0 ? 1 : 0);
				byte[] pixelsByte = new byte[byteWidth * h];

				// Copy the RGB values into the array.
				for (int a = 0; a < h; a++)
				{
					Marshal.Copy(ptr, pixelsByte, a * byteWidth, byteWidth);
					ptr = ptr + bmpData.Stride;
				}
				image.UnlockBits(bmpData);

				return iTextSharp.text.Image.GetInstance(w, h, 1, 1, pixelsByte, null);
			}

			else
			{
				return iTextSharp.text.Image.GetInstance(image, null, true);
			}
		}
		public void Close()
		{
			if (pdfDocument != null)
			{
				pdfDocument.Close();
				pdfDocument.Dispose();
				writer.Close();
				writer.Dispose();
				stream.Close();
				stream.Dispose();
				pdfDocument = null;
				writer = null;
				pdfDocument = null;
			}

			if (reader != null)
			{
				reader.Close();
			}

		}



		public void Tag(string oldFile, Stream fs, string text)
		{
			float x;
			float y;

			// open the reader
			PdfReader reader = new PdfReader(oldFile);
			iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
			float height = 60;
			float width = 150;
			x = size.Width - 40 - width;
			y = size.Height - height;
			Document document = new Document(size);

			// open the writer
			PdfWriter writer = PdfWriter.GetInstance(document, fs);
			document.Open();

			// the pdf content
			PdfContentByte cb = writer.DirectContent;

			// create the new page and add it to the pdf
			PdfImportedPage page = writer.GetImportedPage(reader, 1);
			cb.AddTemplate(page, 0, 0);

			cb.Rectangle(x, y, width, height);
			cb.SetColorFill(BaseColor.WHITE);
			cb.Fill();
			// close the streams and voilá the file should be changed :)


			// write the text in the pdf content
			cb.BeginText();
			// select the font properties
			BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
			cb.SetColorFill(BaseColor.BLACK);
			cb.SetFontAndSize(bf, 20);
			// put the alignment and coordinates here
			cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text, x + 5, y + 10, 0);
			cb.EndText();

			writer.Flush();
			document.Close();
			reader.Close();
			//			fs.Seek(0, SeekOrigin.Begin);
		}
	}
}
