using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES
{
    class Program
    {

        private static SDES sdes;
        private static string TYPE = ".cif";

        static void Main(string[] args)
        {
            Encrypt("MATERIALES.txt", "juan");
            Decrypt("MATERIALES.txt.cif", "juan");
        }


        private static void Encrypt(string path, string password)
        {
            sdes = new SDES(password);
            var data = new List<string>();


            foreach (var item in FileManager.ReadUnencryptedFile(path))
            {
                data.Add(sdes.Encrypt(item));
            }

            FileManager.WriteHeader(path + TYPE, string.Join(",", sdes.PKey10) + ";" + string.Join(",", sdes.PKey8) + ";" + string.Join(",", sdes.PKey4));

            foreach (var item in data)
            {
                FileManager.WriteFile(path +  TYPE, (char)Convert.ToInt32(item, 2));
            }
        }

        private static void Decrypt(string path, string password)
        {
            var values = FileManager.ReadHeader(path).Split(';');
            if (values.Length < 3) return;
            sdes = new SDES(password, values[0], values[1], values[2]);
            var data = new List<string>();


            foreach (var item in FileManager.ReadEncryptedFile(path))
            {
                data.Add(sdes.Decrypt(item));
            }
            
            foreach (var item in data)
            {
                FileManager.WriteFile(path.Remove(path.Length - TYPE.Length, TYPE.Length), (char)Convert.ToInt32(item, 2));
            }
        }
    }
}
