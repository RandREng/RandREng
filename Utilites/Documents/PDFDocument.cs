using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace RandREng.Utility.Documents
{
	public class PDFDocument : IDocument
	{
        private const float PAGE_LEFT_MARGIN = 0;
        private const float PAGE_RIGHT_MARGIN = 0;
        private const float PAGE_TOP_MARGIN = 0;
        private const float PAGE_BOTTOM_MARGIN = 0;

		private enum Mode
		{
			Read,
			Write,
			NotOpen
		}

		private Mode _mode = Mode.NotOpen;

		public void Add(Bitmap bm)
		{
			Add(bm, RotateFlipType.RotateNoneFlipNone);
		}

		public void Save(Bitmap bm, string filename)
		{
			Save(bm, filename, RotateFlipType.RotateNoneFlipNone);
		}

		public string FileName { get; set; }

		~PDFDocument()
		{
			Close();
		}


		public class MyImageRenderListener : IEventListener
		{

			protected MemoryStream stream;
			protected String extension;

			public MyImageRenderListener(MemoryStream stream)
			{
				this.stream = stream;
			}

			public void EventOccurred(IEventData data, EventType type)
			{
				switch (type)
				{
					case EventType.RENDER_IMAGE:
						try
						{
							BinaryWriter os;
							ImageRenderInfo renderInfo = (ImageRenderInfo)data;
							PdfImageXObject image = renderInfo.GetImage();
							if (image == null)
							{
								return;
							}

							byte[] imageByte = image.GetImageBytes(true);
							extension = image.IdentifyImageFileExtension();
							// You can use raw image bytes directly, or write image to disk
							//							filename = System.IO.Path.Combine(path, image.GetPdfObject().GetIndirectReference().GetObjNumber().ToString() + "." + extension);
							os = new BinaryWriter(this.stream, Encoding.UTF8, true);
							os.Write(imageByte);
							os.Flush();
							os.Close();
						}
						catch (Exception )
						{

						}
						break;

					case EventType.RENDER_TEXT:
						try
						{
							TextRenderInfo renderInfo = (TextRenderInfo)data;
						}
						catch (Exception )
						{

						}
						break;

					default:
						break;
				}
			}


			public ICollection<EventType> GetSupportedEvents()
			{
				return null;
			}
		}

		public static void AddImage(Stream inputPdfStream, Stream outputPdfStream, byte[] inputBytes)
		{
			PdfDocument pdfDoc = new(new PdfReader(inputPdfStream), new PdfWriter(outputPdfStream));
			ImageData img = ImageDataFactory.Create(inputBytes);
			PdfPage page = pdfDoc.GetFirstPage();
			iText.Kernel.Geom.Rectangle rect = page.GetPageSize();
			float x = rect.GetWidth() - 98;
			float y = rect.GetHeight() - 98;
            new PdfCanvas(page.NewContentStreamAfter(), pdfDoc.GetFirstPage().GetResources(), pdfDoc)
                .AddImageAt(img, x, y, false);

			pdfDoc.Close();
		}
		
		private PdfDocument _pdfDoc;

        public void Open(string fileName)
		{
			_pdfDoc = new PdfDocument(new PdfReader(fileName));
			//reader = new PdfReader(fileName);
			//parser = new PdfReaderContentParser(reader);
			//listener = new MyImageRenderListener();

		}

		public int Count { get { return _pdfDoc.GetNumberOfPages(); } }

		public Bitmap GetImage(int index)
		{
			MemoryStream stream = new();
			IEventListener listener = new MyImageRenderListener(stream);
			PdfCanvasProcessor parser = new(listener);
			//            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
			//            {
			parser.ProcessPageContent(_pdfDoc.GetPage(index));
			//            }

			if (stream.Length > 0)
			{
				stream.Seek(0, SeekOrigin.Begin);
				return (Bitmap)Bitmap.FromStream(stream);
			}
			else
			{
				return null;
			}
		}

		public void Close()
		{
			if (this._document != null)
			{
				this._document.Close();
				this._document = null;
			}
			if (this._pdfDoc != null)
			{
				this._pdfDoc.Close();
			}
		}

        private Document _document;

        public void Add(Bitmap bm, RotateFlipType rotate)
		{
			if (_mode == Mode.NotOpen)
			{
				//stream = new FileStream(FileName, FileMode.Create);
				//pdfDocument = new Document(PageSize.LETTER, PAGE_LEFT_MARGIN, PAGE_RIGHT_MARGIN, PAGE_TOP_MARGIN, PAGE_BOTTOM_MARGIN);
				//writer = PdfWriter.GetInstance(pdfDocument, stream);
				//pdfDocument.Open();

				PdfWriter writer = new(FileName);
				_pdfDoc = new PdfDocument(writer);
				_document = new Document(_pdfDoc, PageSize.LETTER);
				_document.SetMargins(PAGE_TOP_MARGIN, PAGE_RIGHT_MARGIN, PAGE_BOTTOM_MARGIN, PAGE_LEFT_MARGIN);
				_mode = Mode.Write;
			}

			Bitmap image = bm;

			if (rotate != RotateFlipType.RotateNoneFlipNone)
			{
				image.RotateFlip(rotate);
			}

			MemoryStream ms = new();
			image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

			iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory
			   .CreateJpeg(ms.GetBuffer()))
			   .SetTextAlignment(TextAlignment.CENTER)
				.ScaleToFit(PageSize.LETTER.GetWidth() - (PAGE_LEFT_MARGIN + PAGE_RIGHT_MARGIN), PageSize.LETTER.GetHeight() - (PAGE_TOP_MARGIN + PAGE_BOTTOM_MARGIN));
			_document.Add(img);
		}


		public void Save(Bitmap bm, string filename, RotateFlipType rotate)
		{
			//Bitmap image = bm;

			//if (rotate != RotateFlipType.RotateNoneFlipNone)
			//{
			//	image.RotateFlip(rotate);
			//}

			//using (FileStream stream = new FileStream(filename, FileMode.Create))
			//{
			//	using (Document pdfDocument = new Document(PageSize.LETTER, PAGE_LEFT_MARGIN, PAGE_RIGHT_MARGIN, PAGE_TOP_MARGIN, PAGE_BOTTOM_MARGIN))
			//	{
			//		PdfWriter writer = PdfWriter.GetInstance(pdfDocument, stream);
			//		pdfDocument.Open();

			//		MemoryStream ms = new MemoryStream();
			//		image.Save(ms, ImageFormat.Tiff);
			//		iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ms);
			//		img.ScaleToFit(PageSize.LETTER.Width - (PAGE_LEFT_MARGIN + PAGE_RIGHT_MARGIN), PageSize.LETTER.Height - (PAGE_TOP_MARGIN + PAGE_BOTTOM_MARGIN));
			//		pdfDocument.Add(img);

			//		pdfDocument.Close();
			//		writer.Close();
			//	}
			//}
		}

	}

}
