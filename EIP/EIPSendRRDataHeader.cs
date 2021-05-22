using System;

namespace EthernetIP.EIP
{
    public class EIPSendRRDataHeader : EIPBase
    {
        private UInt32 EIPInterfaceHandle = 0x00;                           //4b Interface Handel       (2-4.7.2)
        private UInt16 EIPTimeout = 0x00;                                   //2b Always 0x00
        private UInt16 EIPItemCount = 0x02;                                 //2b Always 0x02 for our purposes
        private UInt16 EIPItem1Type = 0x00;                                 //2b Null Item Type
        private UInt16 EIPItem1Length = 0x00;                               //2b No data for Null Item
        private UInt16 EIPItem2Type = 0xB2;                                 //2b Uconnected CIP message to follow
        private UInt16 EIPItem2Length;                                      //2b

        public EIPSendRRDataHeader(UInt32 sessionHandle, UInt64 context, int frameLen)
        {
            this.Command = CommandsEnum.SendRRData;                         //2b EIP SendRRData  (Vol2 2-4.7)     
            this.Length = (UInt16)(16 + frameLen);
            this.SessionHandle = sessionHandle;
            this.Status = 0x00;
            this.SenderContext = context;
            this.Options = 0x00;

            this.EIPItem2Length = (UInt16)frameLen;

            this.CommandSpecificData.Add((byte)EIPInterfaceHandle);
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 8));
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 16));
            this.CommandSpecificData.Add((byte)((UInt32)EIPInterfaceHandle >> 24));

            this.CommandSpecificData.Add((byte)EIPTimeout);
            this.CommandSpecificData.Add((byte)((UInt16)EIPTimeout >> 8));

            this.CommandSpecificData.Add((byte)EIPItemCount);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItemCount >> 8));

            this.CommandSpecificData.Add((byte)EIPItem1Type);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem1Type >> 8));

            this.CommandSpecificData.Add((byte)EIPItem1Length);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem1Length >> 8));

            this.CommandSpecificData.Add((byte)EIPItem2Type);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem2Type >> 8));

            this.CommandSpecificData.Add((byte)EIPItem2Length);
            this.CommandSpecificData.Add((byte)((UInt16)EIPItem2Length >> 8));
        }
    }
}
