using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SDES
{
    public static class FileManager
    {
        private static Encoding ENCODE = Encoding.GetEncoding("iso-8859-1");

        public static IEnumerable<string> ReadUnencryptedFile(string path)
        {
            if (File.Exists(path))
            {
                using (var file = new FileStream(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file, ENCODE))
                    {
                        for (int i = 0; i<reader.BaseStream.Length; i++)
                        {
                            yield return Convert.ToString(reader.ReadChar(), 2).PadLeft(8, '0');
                        }
                    }
                }
            }
        }

        public static IEnumerable<string> ReadEncryptedFile(string path)
        {
            if (File.Exists(path))
            {
                using (var file = new FileStream(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file, ENCODE))
                    {
                        reader.ReadString();
                        while (reader.BaseStream.Position <  reader.BaseStream.Length)
                        {
                            yield return Convert.ToString(reader.ReadChar(), 2).PadLeft(8, '0');
                        }
                    }
                }
            }
        }

        public static void WriteFile(string path, char data)
        {
            using (var file = new FileStream(path, FileMode.Append))
            {
                using (var writer = new BinaryWriter(file, ENCODE))
                {
                    writer.Write(data);
                }
            }
        }
        public static void WriteHeader(string path, string data)
        {
            using (var file = new FileStream(path, FileMode.Append))
            {
                using (var writer = new BinaryWriter(file, ENCODE))
                {
                    writer.Write(data + "\n\r");
                }
            }
        }

        public static string ReadHeader(string path)
        {
            if (File.Exists(path))
            {
                using (var file = new FileStream(path, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file, ENCODE))
                    {
                        return reader.ReadString();
                    }
                }
            }
            return "";
        }
    }
}
