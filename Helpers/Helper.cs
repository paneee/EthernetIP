using System;
using System.Linq;

namespace EIP.Helpers
{
    public static class Helper
    {
        public static void WriteToConsole(this byte[] byteArray, string str)
        {
            int i = 0;
            Console.Write(str);
            Console.Write(" (" + byteArray.Count() + ") ");
            foreach (byte b in byteArray)
            {
                string stingHex1 = "\n " + "(" + i++ + ")" + "0x" + BitConverter.ToString(new byte[] { b });
                Console.Write(stingHex1);
            }
            Console.WriteLine();
        }
        public static void Con()
        {
            UInt32 EIPOptions = 0x12345678;
            UInt32 EIPOptions2;
            byte[] array = new byte[4];

            array[0] = (byte)EIPOptions;
            array[1] = (byte)((UInt32)EIPOptions >> 8);
            array[2] = (byte)((UInt32)EIPOptions >> 16);
            array[3] = (byte)((UInt32)EIPOptions >> 24);

            EIPOptions2 = BitConverter.ToUInt32(new byte[] { array[0], array[1], array[2], array[3] }, 0);
        }

        public static bool GetBit(byte myByte, int bitNumber)
        {
            bool bit = (myByte & (1 << bitNumber)) != 0;
            return bit;
        }
    }
}