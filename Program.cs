using System;
using System.Net;
using EthernetIP.PLC;

namespace EthernetIP
{
    class Program
    {
        static void Main(string[] args)
        {
            PLCDevice plc = new PLCDevice(IPAddress.Parse("10.11.20.181"), 1);

            plc.Connect();

            Console.WriteLine("Test BOOL \n Read value:  " + plc.ReadTag("TEST_BOOL", TAGType.BOOL));
            Console.ReadKey();
            Console.WriteLine(" Write value: {0}", true);
            plc.WriteTag("TEST_BOOL", TAGType.BOOL, 1);
            Console.WriteLine(" Read value:  " + plc.ReadTag("TEST_BOOL", TAGType.BOOL)+ "\n");
            Console.ReadKey();

            Console.WriteLine("Test SINT \n Read value:  " + plc.ReadTag("TEST_SINT", TAGType.SINT));
            Console.ReadKey();
            Console.WriteLine(" Write value: {0}", 23);
            plc.WriteTag("TEST_SINT", TAGType.SINT, 23);
            Console.WriteLine(" Read value:  " + plc.ReadTag("TEST_SINT", TAGType.SINT) + "\n");
            Console.ReadKey();

            Console.WriteLine("Test INT \n Read value:  " + plc.ReadTag("TEST_INT", TAGType.INT));
            Console.ReadKey();
            Console.WriteLine(" Write value: {0}", 100);
            plc.WriteTag("TEST_INT", TAGType.INT, 100);
            Console.WriteLine(" Read value:  " + plc.ReadTag("TEST_INT", TAGType.INT) + "\n");
            Console.ReadKey();

            Console.WriteLine("Test DINT \n Read value:  " + plc.ReadTag("TEST_DINT", TAGType.DINT));
            Console.ReadKey();
            Console.WriteLine(" Write value: {0}", 1050);
            plc.WriteTag("TEST_DINT", TAGType.DINT, 1050);
            Console.WriteLine(" Read value:  " + plc.ReadTag("TEST_DINT", TAGType.DINT) + "\n");
            Console.ReadKey();

            plc.Disconect();
        }
    }
}
