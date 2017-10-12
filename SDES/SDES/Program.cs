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
        private static string logo = (".----------------.  .----------------.  .----------------.  .----------------.  .----------------.\n" +
                                    "| .--------------. || .--------------. || .--------------. || .--------------. || .--------------. |\n" +
                                    "| |     _____    | || |     ______   | || |  _______     | || |  ____  ____  | || |   ______     | |\n" +
                                    "| |    |_   _|   | || |   .' ___  |  | || | |_   __ \\    | || | |_  _||_  _| | || |  |_   __ \\   | |\n" +
                                    "| |      | |     | || |  / .'   \\_|  | || |   | |__) |   | || |   \\ \\  / /   | || |    | |__) |  | |\n" +
                                    "| |   _  | |     | || |  | |         | || |   |  __ /    | || |    \\ \\/ /    | || |    |  ___/   | |\n" +
                                    "| |  | |_' |     | || |  \\ `.___.'\\  | || |  _| |  \\ \\_  | || |    _|  |_    | || |   _| |_      | |\n" +
                                    "| |  `.___.'     | || |   `._____.'  | || | |____| |___| | || |   |______|   | || |  |_____|     | |\n" +
                                    "| |              | || |              | || |              | || |              | || |              | |\n" +
                                    "| '--------------' || '--------------' || '--------------' || '--------------' || '--------------' |\n" +
                                    "'----------------'  '----------------'  '----------------'  '----------------'  '----------------' ");
        static void Main(string[] args)
        {
            Console.WriteLine(logo);
            Console.WriteLine("             Escriba -h en la línea de comandos para visualizar el menú de ayuda\n");
            while (true)
            {
                Console.Write("jCryp > ");
                Console.WriteLine(Instrucciones(Console.ReadLine()));
            }
        }


        private static string Instrucciones(string cadena)
        {
            var instrucciones = cadena.Split(' ').Where(x => x != "").ToArray();
            switch (instrucciones[0].ToLower())
            {
                case "-c":
                    if (instrucciones.Length < 5) return "La cadena no contiene el formato correcto: no se ingresaron algunos parámetros. Escriba -h para ver el menú de ayuda.";
                    switch (instrucciones[1].ToLower())
                    {
                        case "-p":
                            if (instrucciones[2].Equals("-f")) return "La contraseña no puede estar vacía.";
                            if (!instrucciones[3].ToLower().Equals("-f")) return "Se esperaba -f después de: " + instrucciones[1] + " " + instrucciones[2];
                            if (!FileManager.FileExists(instrucciones[4])) return "La ruta del archivo no existe o no es accesible en: " + instrucciones[4];
                            Console.WriteLine("Comprimiendo archivo...");
                            return Encrypt(instrucciones[4], instrucciones[2]);
                        case "-f":
                            if (instrucciones[2].Equals("-p")) return "La ruta de archivo no puede estar vacía";
                            if (!FileManager.FileExists(instrucciones[2])) return "La ruta del archivo no existe o no es accesible en: " + instrucciones[2];
                            if (!instrucciones[3].ToLower().Equals("-p")) return "Se esperaba -p después de: " + instrucciones[1] + " " + instrucciones[2];
                            if (instrucciones.Length < 4) return "La contraseña no puede estar vacía";
                            Console.WriteLine("Comprimiendo archivo...");
                            return Encrypt(instrucciones[2], instrucciones[4]);
                        default:
                            return "La cadena no contiene el formato correcto: se esperaba -p o -f. Escriba -h para ver el menú de ayuda.";
                    }
                case "-d":
                    if (instrucciones.Length < 5) return "La cadena no contiene el formato correcto: no se ingresaron algunos parámetros. Escriba -h para ver el menú de ayuda.";
                    switch (instrucciones[1].ToLower())
                    {
                        case "-p":
                            if (instrucciones[2].Equals("-f")) return "La contraseña no puede estar vacía.";
                            if (!instrucciones[3].ToLower().Equals("-f")) return "Se esperaba -f después de: " + instrucciones[1] + " " + instrucciones[2];
                            if (!FileManager.FileExists(instrucciones[4])) return "La ruta del archivo no existe o no es accesible en: " + instrucciones[4];
                            Console.WriteLine("Comprimiendo archivo...");
                            return Encrypt(instrucciones[4], instrucciones[2]);
                        case "-f":
                            if (instrucciones[2].Equals("-p")) return "La ruta de archivo no puede estar vacía";
                            if (!FileManager.FileExists(instrucciones[2])) return "La ruta del archivo no existe o no es accesible en: " + instrucciones[2];
                            if (!instrucciones[3].ToLower().Equals("-p")) return "Se esperaba -p después de: " + instrucciones[1] + " " + instrucciones[2];
                            if (instrucciones.Length < 4) return "La contraseña no puede estar vacía";
                            Console.WriteLine("Descomprimiendo archivo... Si la contraseña no es correcta, el archivo no será legible.");
                            return Decrypt(instrucciones[2], instrucciones[4]);
                        default:
                            return "La cadena no contiene el formato correcto: se esperaba -p o -f. Escriba -h para ver el menú de ayuda.";
                    }
                case "-h":
                    return "jCryp cuenta con las siguientes opciones:\n\n" +
                            "   -c      Indica que se desea cifrar un archivo. Al usarla, debe ser la primera instrucción indicada.\n" +
                            "   -d      Indica que se desea descifrar un archivo. Al usarla, debe ser la primera instrucción indicada.\n" +
                            "   -f      Indica que la siguiente cadena es la ruta del archivo.\n" +
                            "   -p      Indica que la siguiente cadena es la contraseña con la que se desea cifrar el archivo.\n" +
                            "   clear   Limpia la consola de todos los registros.\n" +
                            "\n\nEjemplos (omita las comillas):\n" +
                            "   '-c -f C:\\Agenda.txt -p AdFs78d'               -> Cifrará un archivo llamado 'Agenda.txt' ubicado en C:\\ usando la clave 'AdFs78d'.\n" +
                            "   '-c -p AdFs78d -f C:\\Agenda.txt '              -> Equivalente a la expresion anterior.\n" +
                            "   '-d -f C:\\Usuarios.txt.cif -p Uq5q45d'         -> Descifrará un archivo antes comprimida llamado 'Usuarios.txt.cif' ubicado en C.\\ usando la clave 'Uq5q45d'.\n" +
                            "   '-d -p Uq5q45d -f C:\\Usuarios.txt.cif'         -> Equivalente a la expresion anterior.\n" +
                            "\n\n   * Recuerde, la primera instrucción debe ser -c o -d." +
                            "\n\n   * Si se produce un error, el programa le indicará como corregirlo.";
                case "clear":
                    Console.Clear();
                    Console.WriteLine(logo);
                    Console.WriteLine("             Escriba -h en la línea de comandos para visualizar el menú de ayuda\n");
                    return "";
                default:
                    return "La cadena no contiene el formato correcto: la primera instrucción debe ser -c o -d. Escriba -h para ver el menú de ayuda.";
            }
        }

        private static string Encrypt(string path, string password)
        {
            try
            {
                sdes = new SDES(password); // Genera un objeto sdes con una clave única y permutaciones nuevas
                var data = new List<string>(); //Lista que guardará cada fragmento cifrado

                // Comienza el cifrado
                foreach (var item in FileManager.ReadUnencryptedFile(path))
                {
                    data.Add(sdes.Encrypt(item));
                }
                
                //Escribe los datos al archivo .cif
                FileManager.WriteHeader(path + TYPE, string.Join(",", sdes.PKey10) + ";" + string.Join(",", sdes.PKey8) + ";" + string.Join(",", sdes.PKey4));
                FileManager.WriteFile(path + TYPE, data);
                //Retorna el estado.
                return "Archivo cifrado y guardado como: " + path + TYPE + "\nSe cifró con la contraseña: " + password;
            }
            catch (Exception ex)
            {
                return "Archivo no cifrado debido al siguiente error: " + ex.Message;
            }
        }

        private static string Decrypt(string path, string password)
        {
            try
            {
                var values = FileManager.ReadHeader(path).Split(';');  // Cargamos los datos que requerimos
                if (values.Length < 3) return "El archivo parece estar dañado. ¿Ha modificado el archivo?"; // Verificamos los datos

                sdes = new SDES(password, values[0], values[1], values[2]); //Genera un objeto sdes con las permutaciones del archivo y genera la clave única (esta clave solo es la misma que la generada al cifrar si la contraseña es la misma)
                var data = new List<string>(); // Lista que guardará cada fragmento descifrado.

                // Comienza el descifrado
                foreach (var item in FileManager.ReadEncryptedFile(path))
                {
                    data.Add(sdes.Decrypt(item));
                }

                //Escribimos el archivo original
                FileManager.WriteFile(path.Remove(path.Length - TYPE.Length, TYPE.Length), data);
                //Retornamos el estado
                return "Archivo descifrado y guardado como: "  + path.Remove(path.Length - TYPE.Length, TYPE.Length) + "\nSe descifró con la contraseña: " + password;
            }
            catch (Exception ex)
            {
                return "Archivo no descifrado debido al siguiente error: " + ex.Message;
            }
        }
    }
}
