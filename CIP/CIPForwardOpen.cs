using System;

namespace EthernetIP.CIP
{
    public class CIPForwardOpen
    {
        private byte CIPService = 0x54;                                                 //1b CIP OpenForward        Vol 3 (3-5.5.2)(3-5.5)
        private byte CIPPathSize = 0x02;                                                //1b Request Path zize              (2-4.1)
        private byte CIPClassType = 0x20;                                               //1b Segment type                   (C-1.1)(C-1.4)(C-1.4.2)
                                                                                        //[Logical Segment][Class ID][8 bit addressing]
        private byte CIPClass = 0x06;                                                   //1b Connection Manager Object      (3-5)
        private byte CIPInstanceType = 0x24;                                            //1b Instance type                  (C-1.1)
                                                                                        //[Logical Segment][Instance ID][8 bit addressing]
        private byte CIPInstance = 0x01;                                                //1b Instance
        private byte CIPPriority = 0x0A;                                                //1b Timeout info                   (3-5.5.1.3)(3-5.5.1.2)
        private byte CIPTimeoutTicks = 0x0e;                                            //1b Timeout Info                   (3-5.5.1.3)
        private UInt32 CIPOTConnectionID = 0x20000002;                                  //4b O->T connection ID             (3-5.16)
        private UInt32 CIPTOConnectionID = 0x20000001;                                  //4b T->O connection ID             (3-5.16)
        private UInt16 CIPConnectionSerialNumber;                                       //2b Serial number for THIS connection (3-5.5.1.4)
        private UInt16 CIPVendorID;                                                     //2b Vendor ID                      (3-5.5.1.6)
        private UInt32 CIPOriginatorSerialNumber;                                       //4b                        (3-5.5.1.7)
        private byte CIPMultiplier = 0x03;                                              //1b Timeout Multiplier             (3-5.5.1.5)
        private byte[] CIPFiller = new byte[3] { 0x00, 0x00, 0x00 };                    //(BBB) align back to word bound
        private UInt32 CIPOTRPI = 0x00201234;                                           //4b RPI just over 2 seconds        (3-5.5.1.2)
        private UInt16 CIPOTNetworkConnectionParameters = 0x43f4;                       //2b O->T connection Parameters    (3-5.5.1.1)
                                                                                        // Non-Redundant,Point to Point,[reserved],Low Priority,Variable,[500 bytes] 
                                                                                        // Above is word for Open Forward and dint for Large_Forward_Open (3-5.5.1.1)
        private UInt32 CIPTORPI = 0x00204001;                                           //4b RPI just over 2 seconds       (3-5.5.1.2)
        private UInt16 CIPTONetworkConnectionParameters = 0x43f4;                       //2b T-O connection Parameters    (3-5.5.1.1)
                                                                                        // Non-Redundant,Point to Point,[reserved],Low Priority,Variable,[500 bytes] 
                                                                                        // Above is word for Open Forward and dint for Large_Forward_Open (3-5.5.1.1)
        private byte CIPTransportTrigger = 0xA3;                                        //1b                                   (3-5.5.1.12)
        private byte CIPConnectionPathSize = 0x03;                                      //1b                                   (3-5.5.1.9)
        private byte[] CIPConnectionPath;                                               //8x8b Compressed / Encoded Path  (C-1.3)(Fig C-1.2)

        public CIPForwardOpen(UInt16 serialNumber, UInt16 vendorID, UInt32 originatorSerialNumber, byte processorSlot)
        {
            this.CIPConnectionSerialNumber = serialNumber;
            this.CIPVendorID = vendorID;
            this.CIPOriginatorSerialNumber = originatorSerialNumber;
            this.CIPConnectionPath = new byte[8] { 0x01, processorSlot, 0x20, 0x02, 0x24, 0x01, 0x00, 0x00 };
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

            array[8] = (byte)CIPOTConnectionID;
            array[9] = (byte)((UInt32)CIPOTConnectionID >> 8);
            array[10] = (byte)((UInt32)CIPOTConnectionID >> 16);
            array[11] = (byte)((UInt32)CIPOTConnectionID >> 24);

            array[12] = (byte)CIPTOConnectionID;
            array[13] = (byte)((UInt32)CIPTOConnectionID >> 8);
            array[14] = (byte)((UInt32)CIPTOConnectionID >> 16);
            array[15] = (byte)((UInt32)CIPTOConnectionID >> 24);

            array[16] = (byte)CIPConnectionSerialNumber;
            array[17] = (byte)((UInt16)CIPConnectionSerialNumber >> 8);

            array[18] = (byte)CIPVendorID;
            array[19] = (byte)((UInt16)CIPVendorID >> 8);

            array[20] = (byte)CIPOriginatorSerialNumber;
            array[21] = (byte)((UInt32)CIPOriginatorSerialNumber >> 8);
            array[22] = (byte)((UInt32)CIPOriginatorSerialNumber >> 16);
            array[23] = (byte)((UInt32)CIPOriginatorSerialNumber >> 24);

            array[24] = CIPMultiplier;

            array[25] = CIPFiller[0];
            array[26] = CIPFiller[1];
            array[27] = CIPFiller[2];

            array[28] = (byte)CIPOTRPI;
            array[29] = (byte)((UInt32)CIPOTRPI >> 8);
            array[30] = (byte)((UInt32)CIPOTRPI >> 16);
            array[31] = (byte)((UInt32)CIPOTRPI >> 24);

            array[32] = (byte)CIPOTNetworkConnectionParameters;
            array[33] = (byte)((UInt16)CIPOTNetworkConnectionParameters >> 8);

            array[34] = (byte)CIPTORPI;
            array[35] = (byte)((UInt32)CIPTORPI >> 8);
            array[36] = (byte)((UInt32)CIPTORPI >> 16);
            array[37] = (byte)((UInt32)CIPTORPI >> 24);

            array[38] = (byte)CIPTONetworkConnectionParameters;
            array[39] = (byte)((UInt16)CIPTONetworkConnectionParameters >> 8);

            array[40] = CIPTransportTrigger;

            array[41] = CIPConnectionPathSize;

            array[42] = CIPConnectionPath[0];
            array[43] = CIPConnectionPath[1];
            array[44] = CIPConnectionPath[2];
            array[45] = CIPConnectionPath[3];
            array[46] = CIPConnectionPath[4];
            array[47] = CIPConnectionPath[5];

            return array;
        }
    }
}
