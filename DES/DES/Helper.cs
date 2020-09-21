using System;
using System.Linq;
using System.Text;

namespace DES
{
    public static class Helper
    {
        public static byte[] ToBits(byte decimalnumber, int numberofbits)
        {
            byte[] bitarray = new byte[numberofbits];
            int k = numberofbits - 1;
            char[] bd = Convert.ToString(decimalnumber, 2).ToCharArray();

            for (int i = bd.Length - 1; i >= 0; --i, --k)
            {
                if (bd[i] == '1')
                    bitarray[k] = 1;
                else
                    bitarray[k] = 0;
            }

            while (k >= 0)
            {
                bitarray[k] = 0;
                --k;
            }

            return bitarray;
        }

        public static byte ToDecimal(byte[] bitsarray)
        {
            string stringvalue = "";
            for (byte i = 0; i < bitsarray.Length; i++)
            {
                stringvalue += bitsarray[i].ToString();
            }
            byte DecimalValue = (byte)Convert.ToInt32(stringvalue, 2);

            return DecimalValue;
        }

        public static byte[] ConvertBitsToBytes(byte[] sentarray, int len)
        {
            byte j, k, decimalvalue;
            byte[] tempbitarray = new byte[8];
            byte[] array = new byte[sentarray.Length / 8];
            for (byte i = 0; i < len; i += 8)
            {
                for (k = 0, j = i; j < (i + 8); ++k, ++j)
                {
                    tempbitarray[k] = sentarray[j];
                }
                decimalvalue = ToDecimal(tempbitarray);
                array[i / 8] = decimalvalue;
            }

            return array;
        }

        public static byte[] ConvertToByteArray(byte[] array)
        {
            var mod = array.Length % 8;
            var toadd = mod == 0 ? 0 : 8 - mod;
            var bitsArray = new byte[(array.Length + toadd) * 8];
            for (int i = 0; i < array.Length; i++)
            {
                var tempArray = ToBits(array[i], 8);
                tempArray.CopyTo(bitsArray, i * 8);
            }
            return bitsArray;
        }
        public static byte[] ConvertToByteArray(string str, Encoding encoding)
        {
            var array = encoding.GetBytes(str);
            return ConvertToByteArray(array);
        }

        public static byte[] CombineArrays(byte[] array1, byte[] array2)
        {
            var length = array1.Length + array2.Length;
            var array = new byte[length];
            array1.CopyTo(array, 0);
            array2.CopyTo(array, array1.Length);
            return array;
        }

        public static byte[] XOR(byte[] array1, byte[] array2)
        {
            var array3 = new byte[array1.Length];
            for (int i = 0; i < array1.Length; i++)
            {
                array3[i] = (byte)(array1[i] ^ array2[i]);
            }
            return array3;
        }
    }
}
