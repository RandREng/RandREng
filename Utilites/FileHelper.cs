using System.IO;

namespace RandREng.Utility
{
    public class FileHelper
    {
        public static bool Move(string sourceFile, string destFile)
        {
            bool bOk = false;

            if (File.Exists(sourceFile))
            {
                if (!Directory.Exists(Path.GetDirectoryName(destFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                }

                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                File.Move(sourceFile, destFile);
                bOk = true;
            }
            return bOk;
        }

        public static bool Compare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while (file1byte == file2byte && file1byte != -1);

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return file1byte - file2byte == 0;
        }

        public static bool MoveFile(string SourceFilename, string DestPath, out string DestFilename)
        {
            FileInfo file = new FileInfo(SourceFilename);
            bool bNew = MoveFile(ref file, DestPath);
            DestFilename = file.FullName;
            return bNew;
        }

        public static bool MoveFile(ref FileInfo sourceFile, string DestPath)
        {
            return MoveFile(ref sourceFile, DestPath, true);
        }

        public static bool MoveFile(ref FileInfo sourceFile, string DestPath, bool Unique)
        {
            bool bNew = true;
            if (!Directory.Exists(DestPath))
            {
                Directory.CreateDirectory(DestPath);
            }

            string LastFilename = "";
            string DestFilename = UniqueFileName(sourceFile.Name, DestPath, out LastFilename);

            if (Unique && !string.IsNullOrEmpty(LastFilename))
            {
                if (Compare(LastFilename, sourceFile.FullName))
                {
                    bNew = false;
                    DestFilename = LastFilename;
                    sourceFile.Delete();
                    sourceFile = new FileInfo(DestFilename);
                }
            }
            if (bNew)
            {
                sourceFile.MoveTo(DestFilename);
            }
            return bNew;
        }

        public static string UniqueFileName(string fileName, out string LastFilename)
        {
            string DestPath = Path.GetDirectoryName(fileName);
            return UniqueFileName(fileName, DestPath, out LastFilename);
        }

        public static string UniqueFileName(string fileName, string DestPath, out string LastFilename)
        {
            int Count = 1;
            string BaseFileName = Path.GetFileNameWithoutExtension(fileName);
            string Ext = Path.GetExtension(fileName);
            string DestFilename = Path.Combine(DestPath, Path.GetFileName(fileName));
            LastFilename = "";
            while (File.Exists(DestFilename))
            {
                LastFilename = DestFilename;
                DestFilename = Path.Combine(DestPath, BaseFileName + "_" + Count + Ext);
                Count++;
            }
            return DestFilename;
        }
    }

}
