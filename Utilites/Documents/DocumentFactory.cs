using System.IO;

namespace RandREng.Utility.Documents
{
    public static class DocumentFactory
    {

        public static IDocument GetDocument(string file)
        {
            IDocument doc = null;
            string ext = Path.GetExtension(file).ToUpper();
            if (ext == ".TIF" || ext == ".TIFF")
            {
                doc = new MTiffDocument();
            }
            if (ext == ".PDF")
            {
                doc = new PDFDocument();
            }
            if (doc != null)
            {
                doc.Open(file);
            }
            return doc;
        }

    }
}
