using System;

namespace EthernetIP.CIP
{
    public class CIPForwardClose
    {
        private byte CIPService = 0x4E;                                     //1b CIP OpenForward Vol 3 (3-5.5.2)(3-5.5)
        private byte CIPPathSize = 0x02;                                    //1b Request Path zize (2-4.1)
        private byte CIPClassType = 0x20;                                   //1b Segment type (C-1.1)(C-1.4)(C-1.4.2) [Logical Segment][Class ID][8 bit addressing]
        private byte CIPClass = 0x06;                                       //1b Connection Manager Object (3-5)
        private byte CIPInstanceType = 0x24;                                //1b Instance type (C-1.1) 
        private byte CIPInstance = 0x01;                                    //1b Instance
        private byte CIPPriority = 0x0A;                                    //1b Timeout info (3-5.5.1.3)(3-5.5.1.2)
        private byte CIPTimeoutTicks = 0x0e;                                //1b Timeout Info (3-5.5.1.3)
        private UInt16 CIPConnectionSerialNumber;                           //2b Serial number for THIS connection (3-5.5.1.4)
        private UInt16 CIPVendorID;                                         //2b
        private UInt32 CIPOriginatorSerialNumber;                           //(4b)
        private byte CIPConnectionPathSize = 0x03;                          //1b (3-5.5.1.9)
        private byte CIPReserved = 0x00;                                    //1b
        private byte[] CIPConnectionPath;                                   //(6b) Compressed / Encoded Path (C-1.3)(Fig C-1.2)


        public CIPForwardClose(UInt16 serialNumber, UInt16 vendorID, UInt32 originatorSerialNumber, byte processorSlot)
        {
            this.CIPConnectionSerialNumber = serialNumber;
            this.CIPOriginatorSerialNumber = originatorSerialNumber;
            this.CIPVendorID = vendorID;
            this.CIPConnectionPath = new byte[6] { 0x01, processorSlot, 0x20, 0x02, 0x24, 0x01 };
        }


        public byte[] Get()
        {
            byte[] array = new byte[48];

            array[0] = CIPService;
            array[1] = CIPPathSize;
            array[2] = CIPClassType;
            array[3] = CIPClass;
            array[4] = CIPInstanceType;
            array[5] = CIPInstance;
            array[6] = CIPPriority;
            array[7] = CIPTimeoutTicks;

            array[8] = (byte)CIPConnectionSerialNumber;
            array[9] = (byte)((UInt16)CIPConnectionSerialNumber >> 8);

            array[10] = (byte)CIPVendorID;
            array[11] = (byte)((UInt16)CIPVendorID >> 8);

            array[12] = (byte)CIPOriginatorSerialNumber;
            array[13] = (byte)((UInt32)CIPOriginatorSerialNumber >> 8);
            array[14] = (byte)((UInt32)CIPOriginatorSerialNumber >> 16);
            array[15] = (byte)((UInt32)CIPOriginatorSerialNumber >> 24);

            array[16] = CIPConnectionPathSize;

            array[17] = CIPReserved;

            array[18] = CIPConnectionPath[0];
            array[19] = CIPConnectionPath[1];
            array[20] = CIPConnectionPath[2];
            array[21] = CIPConnectionPath[3];
            array[22] = CIPConnectionPath[4];
            array[23] = CIPConnectionPath[5];

            return array;
        }
    }
}
