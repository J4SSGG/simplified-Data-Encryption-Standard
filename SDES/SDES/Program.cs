using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES
{
    class Program
    {
        static void Main(string[] args)
        {
            SDES sdes = new SDES();

            Console.WriteLine(sdes.Encrypt(Console.ReadLine(), Console.ReadLine()));
            Console.WriteLine(sdes.Desencrypt(Console.ReadLine(), Console.ReadLine(), string.Join(",", sdes.PKey10), string.Join(",", sdes.PKey8), string.Join(",", sdes.PKey4)));
            Console.ReadKey();
        }
    }
}
