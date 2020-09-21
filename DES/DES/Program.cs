using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Text;

namespace DES
{
    class Program
    {
        static byte[] Process(byte[] array, string key, Operation operation)
        {
            var keyArray = Helper.ConvertToByteArray(key, Encoding.ASCII);
            var sendArray = new byte[64];
            var finalArray = new byte[array.Length];
            for (byte i = 0; i < array.Length; i += 64)
            {
                Array.Copy(array, i, sendArray, 0, 64);
                var processedArray = DesAlgorithm(sendArray, keyArray, operation);
                processedArray.CopyTo(finalArray, i);
            }
            var bytesArray = Helper.ConvertBitsToBytes(finalArray, finalArray.Length);
            return bytesArray;
        }

        static byte[] DesAlgorithm(byte[] array, byte[] key, Operation operation)
        {
            var initalPermutationArray = array.Permutate(PermutationTables.InitialPermutation);
            var keyArray = key.Permutate(PermutationTables.PermutationKey);
            var (left, right) = initalPermutationArray.SplitArrayInHalf();
            for (byte i = 0; i < 16; i++)
            {
                (left, right, keyArray) = Round(left, right, keyArray, i, operation);
            }
            var combinedArray = Helper.CombineArrays(right, left);
            var finalPermutatedArray = combinedArray.Permutate(PermutationTables.FinalPermutationTable);
            return finalPermutatedArray;
        }

        static (byte[], byte[], byte[]) Round(byte[] left, byte[] right, byte[] key, byte round, Operation operation)
        {
            byte[] newKey;
            if (operation == Operation.ENCRYPT)
            {
                var (cKey, dKey) = key.SplitArrayInHalf();
                var numberOfShifts = PermutationTables.CircularLeftShiftTable[round];
                for (byte i = 0; i < numberOfShifts; i++)
                {
                    cKey.CircularLeftShift();
                    dKey.CircularLeftShift();
                }
                newKey = Helper.CombineArrays(cKey, dKey);
            }
            else
            {
                newKey = key;
            }
            
            var compressionArray = newKey.Permutate(PermutationTables.CompressionPermutationTable);
            var expenssionArray = right.Permutate(PermutationTables.ExpansionPermutationTable);
            var xoredRightArray = Helper.XOR(expenssionArray, compressionArray);
            var sBoxPermutation = xoredRightArray.Permutate(PermutationTables.SBoxTables);
            var pBoxPermutation = sBoxPermutation.Permutate(PermutationTables.PBox);
            var xoredLeft = Helper.XOR(pBoxPermutation, left);
            if (operation == Operation.DECRYPT)
            {
                var (cKey, dKey) = key.SplitArrayInHalf();
                var numberOfShifts = PermutationTables.CircularRightShiftTable[round];
                for (byte i = 0; i < numberOfShifts; i++)
                {
                    cKey.CircularRightShift();
                    dKey.CircularRightShift();
                }
                newKey = Helper.CombineArrays(cKey, dKey);
            }
            return (right, xoredLeft, newKey);
        }

        static void Main(string[] args)
        {
            var text = "ANGELA123456";
            var key = "MYSECRET";

            var bytes = Helper.ConvertToByteArray(text, Encoding.ASCII);

            var bytesArray = Process(bytes, key, Operation.ENCRYPT);
            Console.WriteLine("Encrypted bites in hex: {0}", string.Join(" ", bytesArray.Select(x => x.ToString("X2"))));
            Console.WriteLine("Encrypted text: {0}", Encoding.ASCII.GetString(bytesArray));

            var decryptedBytesArray = Helper.ConvertToByteArray(bytesArray);
            var decryptedArray = Process(decryptedBytesArray, key, Operation.DECRYPT);
            var decryptedText = Encoding.ASCII.GetString(decryptedArray);
            Console.WriteLine("Decrypted text: {0}",decryptedText);
        }
    }
}
