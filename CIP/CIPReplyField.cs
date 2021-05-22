using EIP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EthernetIP.CIP
{
    public class CIPReplyField : CIPBase
    {
        public byte ReplyService { get; private set; }
        public byte Reserved { get; private set; }
        public byte GeneralStatus { get; private set; }
        public byte ExtendStatusSize { get; private set; }
        public List<byte> ReplyData { get; private set; } = new List<byte>();

        private object value { get; set; }
        private List<object> valueArray { get; set; } = new List<object>();

        public CIPReplyField()
        {

        }

        public void Decode(byte[] data)
        {
            this.ReplyService = data[46];
            this.Reserved = data[47];
            this.GeneralStatus = data[48];
            this.ExtendStatusSize = data[49];
            for (int i = 50; i < data.Count(); i++)
            {
                this.ReplyData.Add(data[i]);
            }
        }

        public object GetValue(bool isArray)
        {
            if (!isArray)
            {
                switch (this.ReplyData[0])
                {
                    case (byte)DataType.BOOL:
                        if (ReplyData[2] == 0xFF) { value = true; }
                        else { value = false; }
                        break;

                    case (byte)DataType.SINT:
                        value = (byte)this.ReplyData[2];
                        break;

                    case (byte)DataType.INT:
                        value = (byte)this.ReplyData[2] + 256 * (byte)this.ReplyData[3];
                        break;

                    case (byte)DataType.DINT:
                        value = (UInt32)((byte)this.ReplyData[2] + 256 * (byte)this.ReplyData[3] + 256 * 256 * (byte)this.ReplyData[4] + 256 * 256 * 256 * (byte)this.ReplyData[5]);
                        break;

                    case (byte)DataType.REAL:
                        value = BitConverter.ToSingle(new[] { (byte)this.ReplyData[2], (byte)this.ReplyData[3], (byte)this.ReplyData[4], (byte)this.ReplyData[5] }, 0);
                        break;

                    case (byte)DataType.STRUCT:
                        for (int i = 8; i < this.ReplyData.Count(); i++)
                        {
                            if (ReplyData[i] != 0x0)
                            {
                                value += Char.ConvertFromUtf32(ReplyData[i]);
                            }
                        }
                        break;

                    default:
                        break;
                }
                return (object)value;
            }
            else
            {
                switch (this.ReplyData[0])
                {
                    case (byte)DataType.DWORD:                                               //Special bool array
                        for (int i = 0; i < this.ReplyData.Count - 2; i++)
                        {
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 0));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 1));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 2));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 3));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 4));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 5));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 6));
                            valueArray.Add(Helper.GetBit(ReplyData[i + 2], 7));
                        }
                        break;

                    case (byte)DataType.SINT:
                        for (int i = 0; i < this.ReplyData.Count - 2; i++)
                        {
                            valueArray.Add((byte)this.ReplyData[i + 2]);
                        }
                        break;

                    case (byte)DataType.INT:
                        for (int i = 0; i < this.ReplyData.Count - 2; i = i + 2)
                        {
                            valueArray.Add((byte)this.ReplyData[i + 2] + 256 * (byte)this.ReplyData[i + 3]);
                        }
                        break;

                    case (byte)DataType.DINT:
                        for (int i = 0; i < this.ReplyData.Count - 2; i = i + 4)
                        {
                            valueArray.Add((UInt32)((byte)this.ReplyData[i + 2] + 256 * (byte)this.ReplyData[i + 3] + 256 * 256 * (byte)this.ReplyData[i + 4] + 256 * 256 * 256 * (byte)this.ReplyData[i + 5]));
                        }
                        break;

                    case (byte)DataType.REAL:
                        for (int i = 0; i < this.ReplyData.Count - 2; i = i + 4)
                        {
                            valueArray.Add(BitConverter.ToSingle(new[] { (byte)this.ReplyData[i + 2], (byte)this.ReplyData[i + 3], (byte)this.ReplyData[i + 4], (byte)this.ReplyData[i + 5] }, 0));
                        }
                        break;

                    case (byte)DataType.STRUCT:
                        for (int k = 0; k < 3; k++)
                        {
                            value = "";
                            for (int i = 8 + k * 88; i < 8 + k * 88 + 82; i++)
                            {
                                if (i < ReplyData.Count - 2)
                                {
                                    if (ReplyData[i] != 0x0)
                                    {
                                        value += Char.ConvertFromUtf32(ReplyData[i]);
                                    }
                                }
                            }
                            valueArray.Add(value);
                        }
                        break;

                    default:
                        break;
                }
            }
            return (object)valueArray;
        }
    }
}


