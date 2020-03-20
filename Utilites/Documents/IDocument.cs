using System.Drawing;

namespace RandREng.Utility.Documents
{
    public interface IDocument
    {
        //        List<Bitmap> Bitmaps { get; }
        //        bool Parse(string fileName);
        //        void BeginParse(string fileName);
        //        void EndParse();

        void Open(string fileName);
        int Count { get; }
        Bitmap GetImage(int index);

        //       void Split(string fileName);
        void Save(Bitmap bm, string filename, RotateFlipType rotate);
        void Save(Bitmap bm, string filename);

        void Add(Bitmap bm);
        void Add(Bitmap bm, RotateFlipType rotate);
        void Close();

        string FileName { get; set; }
    }
}
