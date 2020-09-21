using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DES
{
    public static class Extensions
    {
        public static (byte[], byte[]) SplitArrayInHalf(this byte[] initialArray)
        {
            var length = initialArray.Length;
            var leftArray = initialArray.Take(length / 2).ToArray();
            var rightArray = initialArray.Skip(length / 2).ToArray();

            return (leftArray, rightArray);
        }


        public static byte[] Permutate(this byte[] initialArray, byte[] permutationArray)
        {
            var permutatedArray = new byte[permutationArray.Length];
            for (byte i = 0; i < permutationArray.Length; i++)
            {
                var index = permutationArray[i];
                permutatedArray[i] = initialArray[index - 1];
            }
            return permutatedArray;
        }

        public static byte[] Permutate(this byte[] initialArray, byte[,] permutationArray)
        {
            byte boxIndex = 0;
            var row = new byte[2];
            var col = new byte[4];
            var array = new byte[32];
            var arrayIndex = 0;
            for (byte i = 0; i < 48; i += 6)
            {
                row[0] = initialArray[i];
                row[1] = initialArray[i + 5];
                var rowindex = Helper.ToDecimal(row);

                col[0] = initialArray[i + 1];
                col[1] = initialArray[i + 2];
                col[2] = initialArray[i + 3];
                col[3] = initialArray[i + 4];

                var colindex = Helper.ToDecimal(col);

                var innerIndex = (16 * rowindex) + colindex;
                var value = permutationArray[boxIndex, innerIndex];
                boxIndex++;
                var byteArray = Helper.ToBits(value, 4);
                array[arrayIndex] = byteArray[0];
                array[arrayIndex + 1] = byteArray[1];
                array[arrayIndex + 2] = byteArray[2];
                array[arrayIndex + 3] = byteArray[3];
                arrayIndex += 4;
            }
            return array;
        }
        public static void CircularLeftShift(this byte[] array)
        {
            byte firstbyte = array[0];
            for (byte i = 0; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
            array[array.Length - 1] = firstbyte;
        }

        public static void CircularRightShift(this byte[] array)
        {
            byte lastBit = array[array.Length - 1];
            for (int i = array.Length - 1; i >= 1; --i)
            {
                array[i] = array[i - 1];
            }
            array[0] = lastBit;
        }
    }
}
