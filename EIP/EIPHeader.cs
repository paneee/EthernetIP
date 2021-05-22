using System;

namespace EthernetIP.EIP
{
    public class EIPHeader : EIPBase
    {
        private UInt32 EIPInterfaceHandle = 0x00;                                       //4b Always 0x00
        private UInt16 EIPTimeout = 0x00;                                               //2b Always 0x00
        private UInt16 EIPItemCount = 0x02;                                             //2b For our purposes always 2
        private UInt16 EIPItem1ID = 0xA1;                                               //2b Address (Vol2 Table 2-6.3)(2-6.2.2)
        private UInt16 EIPItem1Length = 0x04;                                           //2b Length of address is 4 bytes
        private UInt32 EIPItem1;// = this.OTNetworkConnectionID;                           //4b O->T Id
        private UInt16 EIPItem2ID = 0xB1;                                               //2b Connecteted Transport (Vol 2 2-6.3.2)
        private UInt16 EIPItem2Length;// = EIPConnectedDataLength;                         //2b Length of CIP Payload
        //private UInt16 EIPSequence;// = this.SequenceCounter;                              //2b

        public EIPHeader(int cipMessageLengtn, UInt32 sessionHandle, ref int contextPointer, UInt32 oTNetworkConnectionID, ref UInt16 sequenceCounter)
        {
            this.Command = CommandsEnum.SendUnitData;                       //2b Send_unit_Data (vol 2 section 2-4.8)
            this.Length = (UInt16)(22 + cipMessageLengtn);                     //2b Length of encapsulated command
            this.SessionHandle = sessionHandle;                             //4bSetup when session crated
            this.Status = 0x00;                                             //4bAlways 0x00
            this.SenderContext = context_dict[contextPointer];
            contextPointer++;
            if (contextPointer == 155)
            {
                contextPointer = 0;
            }
            this.Options = 0x0000;

            byte EIPPayloadLength = (byte)(22 + cipMessageLengtn);             //22 bytes of command specific data + the size of the CIP Payload
            UInt16 EIPConnectedDataLength = (UInt16)(cipMessageLengtn + 2);    //Size of CIP packet plus the sequence counter

            this.EIPItem1 = oTNetworkConnectionID;
            this.EIPItem2Length = EIPConnectedDataLength;

            sequenceCounter++;
            sequenceCounter = (UInt16)(sequenceCounter % 0x10000);

            this.CommandSpecificData.Add((byte)EIPInterfaceHandle);
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 8));
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 16));
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 24));

            this.CommandSpecificData.Add((byte)EIPTimeout);
            this.CommandSpecificData.Add((byte)((UInt16)EIPTimeout >> 8));

            this.CommandSpecificData.Add(((byte)EIPItemCount));
            this.CommandSpecificData.Add(((byte)((UInt16)EIPItemCount >> 8)));

            this.CommandSpecificData.Add((byte)EIPItem1ID);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem1ID >> 8));

            this.CommandSpecificData.Add((byte)EIPItem1Length);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem1Length >> 8));

            this.CommandSpecificData.Add((byte)EIPItem1);
            this.CommandSpecificData.Add((byte)((UInt32)EIPItem1 >> 8));
            this.CommandSpecificData.Add((byte)((UInt32)EIPItem1 >> 16));
            this.CommandSpecificData.Add((byte)((UInt32)EIPItem1 >> 24));

            this.CommandSpecificData.Add((byte)EIPItem2ID);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem2ID >> 8));

            this.CommandSpecificData.Add((byte)EIPItem2Length);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem2Length >> 8));

            this.CommandSpecificData.Add((byte)sequenceCounter);
            this.CommandSpecificData.Add((byte)((UInt16)sequenceCounter >> 8));
        }

        private UInt64[] context_dict = new UInt64[156]
    {
                  0x6572276557,
                  0x6f6e,
                  0x676e61727473,
                  0x737265,
                  0x6f74,
                  0x65766f6c,
                  0x756f59,
                  0x776f6e6b,
                  0x656874,
                  0x73656c7572,
                  0x646e61,
                  0x6f73,
                  0x6f64,
                  0x49,
                  0x41,
                  0x6c6c7566,
                  0x74696d6d6f63,
                  0x7327746e656d,
                  0x74616877,
                  0x6d2749,
                  0x6b6e696874,
                  0x676e69,
                  0x666f,
                  0x756f59,
                  0x746e646c756f77,
                  0x746567,
                  0x73696874,
                  0x6d6f7266,
                  0x796e61,
                  0x726568746f,
                  0x797567,
                  0x49,
                  0x7473756a,
                  0x616e6e6177,
                  0x6c6c6574,
                  0x756f79,
                  0x776f68,
                  0x6d2749,
                  0x676e696c656566,
                  0x6174746f47,
                  0x656b616d,
                  0x756f79,
                  0x7265646e75,
                  0x646e617473,
                  0x726576654e,
                  0x616e6e6f67,
                  0x65766967,
                  0x756f79,
                  0x7075,
                  0x726576654e,
                  0x616e6e6f67,
                  0x74656c,
                  0x756f79,
                  0x6e776f64,
                  0x726576654e,
                  0x616e6e6f67,
                  0x6e7572,
                  0x646e756f7261,
                  0x646e61,
                  0x747265736564,
                  0x756f79,
                  0x726576654e,
                  0x616e6e6f67,
                  0x656b616d,
                  0x756f79,
                  0x797263,
                  0x726576654e,
                  0x616e6e6f67,
                  0x796173,
                  0x657962646f6f67,
                  0x726576654e,
                  0x616e6e6f67,
                  0x6c6c6574,
                  0x61,
                  0x65696c,
                  0x646e61,
                  0x74727568,
                  0x756f79,
                  0x6576276557,
                  0x6e776f6e6b,
                  0x68636165,
                  0x726568746f,
                  0x726f66,
                  0x6f73,
                  0x676e6f6c,
                  0x72756f59,
                  0x73277472616568,
                  0x6e656562,
                  0x676e69686361,
                  0x747562,
                  0x657227756f59,
                  0x6f6f74,
                  0x796873,
                  0x6f74,
                  0x796173,
                  0x7469,
                  0x656469736e49,
                  0x6577,
                  0x68746f62,
                  0x776f6e6b,
                  0x732774616877,
                  0x6e656562,
                  0x676e696f67,
                  0x6e6f,
                  0x6557,
                  0x776f6e6b,
                  0x656874,
                  0x656d6167,
                  0x646e61,
                  0x6572276577,
                  0x616e6e6f67,
                  0x79616c70,
                  0x7469,
                  0x646e41,
                  0x6669,
                  0x756f79,
                  0x6b7361,
                  0x656d,
                  0x776f68,
                  0x6d2749,
                  0x676e696c656566,
                  0x74276e6f44,
                  0x6c6c6574,
                  0x656d,
                  0x657227756f79,
                  0x6f6f74,
                  0x646e696c62,
                  0x6f74,
                  0x656573,
                  0x726576654e,
                  0x616e6e6f67,
                  0x65766967,
                  0x756f79,
                  0x7075,
                  0x726576654e,
                  0x616e6e6f67,
                  0x74656c,
                  0x756f79,
                  0x6e776f64,
                  0x726576654e,
                  0x6e7572,
                  0x646e756f7261,
                  0x646e61,
                  0x747265736564,
                  0x756f79,
                  0x726576654e,
                  0x616e6e6f67,
                  0x656b616d,
                  0x756f79,
                  0x797263,
                  0x726576654e,
                  0x616e6e6f67,
                  0x796173,
                  0x657962646f6f67,
                  0x726576654e,
                  0xa680e2616e6e6f67
     };
    }
}
