using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES
{
    public class Class1
    {
        public int[] PKey10 { get; private set; } = { 8, 5, 3, 4, 7, 6, 2, 9, 0, 1, };
        public int[] PKey8 { get; private set; } = { 0, 7, 4, 6, 2, 5, 3, 1, };
        public int[] PKey4 { get; private set; } = { 0,2,3,1 };
        private string[,] SWB1 = { { "00", "01", "10", "11" }, { "01", "10", "11", "00" }, { "10", "11", "00", "01" }, { "11", "00", "01", "10" } };
        private string[,] SWB2 = { { "11", "00", "01", "10" }, { "01", "10", "11", "00" }, { "00", "01", "10", "11" }, { "10", "11", "00", "01" } };
        private string Key10 = "";
        private string Key8_1 = "";
        private string Key8_2 = "";

        public async Task<object> Encrypt(object inputs) // data es un byte representado en binario -> 11010110
        {
            var parameter = (IDictionary<string, object>)inputs;
            var data = parameter["data"].ToString();
            var password = parameter["password"].ToString();

            BuildValues(password);
            var compress = "";
            foreach (var item in data.ToCharArray())
            {
                compress += ((char)Convert.ToInt32(Confuse(Convert.ToString(item, 2).PadLeft(8, '0'), Key8_1, Key8_2, true), 2)).ToString();
            }
            return compress;
        }

        public async Task<object> Decrypt(object inputs)
        {
            var parameter = (IDictionary<string, object>)inputs;
            var data = parameter["data"].ToString();
            var password = parameter["password"].ToString();

            var uncompress = "";
            foreach (var item in data)
            {
                uncompress += ((char)Convert.ToInt32(Confuse(Convert.ToString(item, 2).PadLeft(8, '0'), Key8_1, Key8_2, false), 2)).ToString();
            }
            return uncompress;
        }

        private string Confuse(string data, string key8_1, string key8_2, bool encrypt)
        {
            //Permutamos la data que es de 8 bits con PKey8 (la permutacion inicial)
            var temp = data;
            data = "";
            foreach (var index in PKey8)
            {
                data += temp[index];
            }

            //La permutación cuatro ya se ha generado

            //Se divide la data de 8 bits
            var key4 = Divide(data);

            //El mismo metodo funciona igual para ambas acciones (encriptar/desencriptar) solo se valida
            var RightData = "";
            var LeftData = "";
            if (encrypt)
            {
                //La primera fase consiste en enviar ambas partes de key4 a MakeConfusion
                //MakeConfusion retorna la segunda parte de la data ya encriptada, que se usara en la segunda fase
                RightData = ManageConfusion(key4[0], key4[1], key8_1, SWB1);
                //Con la data derecha ya encriptada, entonces la segunda fase intercambia la data de la siguiente forma
                LeftData = ManageConfusion(key4[1], RightData, key8_2, SWB2);
            }
            else
            {
                //La primera fase consiste en enviar ambas partes de key4 a MakeConfusion
                //MakeConfusion retorna la segunda parte de la data ya encriptada, que se usara en la segunda fase
                RightData = ManageConfusion(key4[0], key4[1], key8_2, SWB2);
                //Con la data derecha ya encriptada, entonces la segunda fase intercambia la data de la siguiente forma
                LeftData = ManageConfusion(key4[1], RightData, key8_1, SWB1);
            }

            //Se genera la permutacion inversa
            data = "";
            for (int i = 0; i < 8; i++)
            {
                data += (LeftData + RightData)[Array.IndexOf(PKey8, i)];
            }
            return data;
        }

        private string ManageConfusion(string LeftData, string RightData, string key8, string[,] SWB)
        {
            //La permutacion cuatro ya se ha generado
            //Permutamos la data derecha de 4 bits
            var PRightData4 = "";
            foreach (var index in PKey4)
            {
                PRightData4 += RightData[index];
            }

            //Expandimos con la permutacion dada
            var tempData = PRightData4 + PRightData4;
            RightData = "";

            //Hacemos un XOR sobre tempData y key8, que es una de las dos key8 generadas
            for (int i = 0; i < 8; i++)
            {
                RightData += (Convert.ToUInt16(tempData[i]) ^ Convert.ToUInt16(key8[i]));
            }

            //Se recupera de la SwitchBox
            tempData = "";
            var row = Convert.ToInt32(RightData[0].ToString() + RightData[3].ToString(), 2);
            var col = Convert.ToInt32(RightData[1].ToString() + RightData[2].ToString(), 2);
            tempData += SWB[row, col];
            row = Convert.ToInt32(RightData[4].ToString() + RightData[7].ToString(), 2);
            col = Convert.ToInt32(RightData[5].ToString() + RightData[6].ToString(), 2);
            tempData += SWB[row, col];

            //Permutamos la data recuperada de la SB
            PRightData4 = "";
            foreach (var index in PKey4)
            {
                PRightData4 += tempData[index];
            }

            //Hacemos un XOR sobre la data permutada y la data izquierda
            RightData = "";
            for (int i = 0; i < 4; i++)
            {
                RightData += (Convert.ToUInt16(PRightData4[i]) ^ Convert.ToUInt16(LeftData[i]));
            }

            // retornamos la data derecha, que es de 4 bits finalmente
            return RightData;
        }

        private void BuildValues(string password)
        {
            //Obtenemos la clave de 10 digitos y generamos un codigo unico
            var temp10 = string.Join("", Convert.ToString(password.GetHashCode(), 2).Take(10));

            //Permutamos la llave de 10 digitos
            Key10 = "";
            foreach (var index in PKey10)
            {
                Key10 += temp10[index];
            }

            //Se divide la clave de 10 digitios
            var key5 = Divide(Key10);

            //Se hace un LeftShift de 1
            key5[0] = LeftShift(1, key5[0]);
            key5[1] = LeftShift(1, key5[1]);

            //Obtenemos la primera clave de 8 digitos
            Key8_1 = "";
            temp10 = string.Join("", key5);
            foreach (var index in PKey8)
            {
                Key8_1 += temp10[index];
            }

            //Se hace otro LeftShift pero de 2 sobre el anterior LeftShift
            key5[0] = LeftShift(2, key5[0]);
            key5[1] = LeftShift(2, key5[1]);

            //Obtenemos la segunda clave de 8 digitos
            Key8_2 = "";
            temp10 = string.Join("", key5);
            foreach (var index in PKey8)
            {
                Key8_2 += temp10[index];
            }
        }

        private string LeftShift(int moves, string data)
        {
            return data.Substring(moves) + string.Join("", data.Take(moves));
        }

        private void GeneratePermut(object nulll)
        {
            Random random = new Random();
            PKey10 = PKey10.OrderBy(x => random.Next()).ToArray();
            PKey8 = PKey8.OrderBy(x => random.Next()).ToArray();
            PKey4 = PKey4.OrderBy(x => random.Next()).ToArray();
        }

        private string[] Divide(string key)
        {
            string[] data = new string[2];
            data[0] = string.Join("", key.Take(key.Length / 2));
            data[1] = key.Substring(key.Length / 2);
            return data;
        }

    }
}