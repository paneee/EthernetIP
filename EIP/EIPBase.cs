using System;
using System.Collections.Generic;

namespace EthernetIP.EIP
{
    public abstract class EIPBase
    {
        public CommandsEnum Command { get; set; }
        public UInt16 Length { get; set; }
        public UInt32 SessionHandle { get; set; }
        public StatusEnum Status { get; set; }
        public UInt64 SenderContext { get; set; }
        public UInt32 Options { get; set; }
        public List<byte> CommandSpecificData = new List<byte>();

        public byte[] Get()
        {
            List<byte> list = new List<byte>();

            list.Add((byte)Command);
            list.Add((byte)((UInt16)Command >> 8));

            list.Add((byte)Length);
            list.Add((byte)((UInt16)Length >> 8));

            list.Add((byte)SessionHandle);
            list.Add((byte)((UInt32)SessionHandle >> 8));
            list.Add((byte)((UInt32)SessionHandle >> 16));
            list.Add((byte)((UInt32)SessionHandle >> 24));

            list.Add((byte)Status);
            list.Add((byte)((UInt32)Status >> 8));
            list.Add((byte)((UInt32)Status >> 16));
            list.Add((byte)((UInt32)Status >> 24));

            list.Add((byte)SenderContext);
            list.Add((byte)((UInt64)SenderContext >> 8));
            list.Add((byte)((UInt64)SenderContext >> 16));
            list.Add((byte)((UInt64)SenderContext >> 24));
            list.Add((byte)((UInt64)SenderContext >> 32));
            list.Add((byte)((UInt64)SenderContext >> 40));
            list.Add((byte)((UInt64)SenderContext >> 48));
            list.Add((byte)((UInt64)SenderContext >> 56));

            list.Add((byte)Options);
            list.Add((byte)((UInt32)Options >> 8));
            list.Add((byte)((UInt32)Options >> 16));
            list.Add((byte)((UInt32)Options >> 24));

            foreach (byte b in CommandSpecificData)
            {
                list.Add(b);
            }

            return list.ToArray();
        }
    }

    public enum CommandsEnum : UInt16
    {
        NOP = 0x0000,
        ListServices = 0x0004,
        ListIdentity = 0x0063,
        ListInterfaces = 0x0064,
        RegisterSession = 0x0065,
        UnRegisterSession = 0x0066,
        SendRRData = 0x006F,
        SendUnitData = 0x0070,
        IndicateStatus = 0x0072,
        Cancel = 0x0073
    }

    public enum StatusEnum : UInt32
    {
        Success = 0x0000,
        InvalidCommand = 0x0001,
        InsufficientMemory = 0x0002,
        IncorrectData = 0x0003,
        InvalidSessionHandle = 0x0064,
        InvalidLength = 0x0065,
        UnsupportedEncapsulationProtocol = 0x0069
    }



}
