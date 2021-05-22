using EthernetIP.PLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EthernetIP.CIP
{
    public class CIPRequestField : CIPBase
    {
        private byte RequestService { get; set; }
        private byte RequestPathSize { get; set; }
        private List<byte> RequestPath { get; set; } = new List<byte>();
        private List<byte> RequestData { get; set; } = new List<byte>();

        public CIPRequestField()
        {
        }

        public byte[] Get(string tag, ServiceRequest serv, TAGType tagType, int arraySize, int? valueForWrite)
        {
            this.RequestService = (byte)serv;

            string[] structTag = tag.Split('.');
            if (structTag.Count() > 1)
            {
                rwStruct(structTag);
            }
            else if ((tag.EndsWith("]") && tagType != TAGType.BOOL))
            {
                rwArray(tag);
            }
            else if (tag.EndsWith("]") && (tagType == TAGType.BOOL))
            {
                rwBoolArray(tag);
            }
            else
            {
                rwSimpleTag(tag);
            }

            this.RequestPathSize = (byte)(RequestPath.Count() / 2);

            if (serv == ServiceRequest.Read)
            {
                this.RequestData.Add((byte)arraySize);
                this.RequestData.Add((byte)((UInt16)arraySize >> 8));
            }

            if (serv == ServiceRequest.Write)
            {
                this.RequestData.Add((byte)tagType);
                this.RequestData.Add(0x00);

                this.RequestData.Add((byte)arraySize);
                this.RequestData.Add((byte)((UInt16)arraySize >> 8));

                switch (tagType)
                {
                    case TAGType.BOOL:
                        if (valueForWrite == 0)
                        {
                            this.RequestData.Add((byte)0x00);
                        }
                        else
                        {
                            this.RequestData.Add((byte)0xFF);
                        }
                        break;

                    case TAGType.SINT:
                        this.RequestData.Add((byte)valueForWrite);
                        break;

                    case TAGType.INT:
                        this.RequestData.Add((byte)valueForWrite);
                        this.RequestData.Add((byte)((UInt16)valueForWrite >> 8));
                        break;

                    case TAGType.DINT:
                        this.RequestData.Add((byte)valueForWrite);
                        this.RequestData.Add((byte)((UInt32)valueForWrite >> 8));
                        this.RequestData.Add((byte)((UInt32)valueForWrite >> 16));
                        this.RequestData.Add((byte)((UInt32)valueForWrite >> 24));
                        break;

                    case TAGType.REAL:

                        break;

                    case TAGType.STRING:
                        break;



                    default:
                        break;
                }

            }


            List<byte> ret = new List<byte>();
            ret.Add(this.RequestService);
            ret.Add(this.RequestPathSize);
            ret.AddRange(this.RequestPath);
            ret.AddRange(this.RequestData);

            return ret.ToArray();
        }

        private void rwStruct(string[] structTag)
        {
            foreach (string str in structTag)
            {
                if (str.EndsWith("]"))
                {
                    rwArray(str);
                }
                else
                {
                    rwSimpleTag(str);
                }
            }
        }

        private void rwArray(string arrayTag)
        {
            string[] tab = arrayTag.Split(new char[] { ',', '[', ']' });
            int index = tab.Count();

            string tabTagName = tab[0];
            List<int> tabValues = new List<int>();

            for (int i = 1; i < index - 1; i++)
            {
                tabValues.Add(int.Parse(tab[i]));
            }


            this.RequestPath.Add((byte)(FrameConstant.RequestPathStart));
            this.RequestPath.Add((byte)(tabTagName.Count()));
            this.RequestPath.AddRange(Encoding.Default.GetBytes(tabTagName));
            if (this.RequestPath.Count() % 2 == 1)
            {
                this.RequestPath.Add((byte)0x00);
            }

            for (int i = 0; i < tabValues.Count(); i++)
            {
                if (tabValues[i] < 256)
                {
                    RequestPath.Add((byte)(0x28));
                    RequestPath.Add((byte)(tabValues[i]));
                }
                else if ((tabValues[i] < 65536) && (tabValues[i] > 255))
                {
                    RequestPath.Add((byte)(0x29));
                    RequestPath.Add((byte)(0x00));
                    RequestPath.AddRange(BitConverter.GetBytes((UInt16)tabValues[i]));
                }
                else
                {
                    RequestPath.Add((byte)(0x2A));
                    RequestPath.Add((byte)(0x00));
                    RequestPath.AddRange(BitConverter.GetBytes((UInt32)tabValues[i]));
                }
            }
        }

        private void rwSimpleTag(string tag)
        {
            this.RequestPath.Add((byte)(FrameConstant.RequestPathStart));
            this.RequestPath.Add((byte)(tag.Count()));
            this.RequestPath.AddRange(Encoding.Default.GetBytes(tag));
            if (this.RequestPath.Count() % 2 == 1)
            {
                this.RequestPath.Add((byte)0x00);
            }
        }

        private void rwBoolArray(string arrayTag)
        {
            string[] tab = arrayTag.Split(new char[] { ',', '[', ']' });
            int index = tab.Count();

            string tabTagName = tab[0];
            List<int> tabValues = new List<int>();

            for (int i = 1; i < index - 1; i++)
            {
                tabValues.Add(int.Parse(tab[i]) / 32);
            }


            this.RequestPath.Add((byte)(FrameConstant.RequestPathStart));
            this.RequestPath.Add((byte)(tabTagName.Count()));
            this.RequestPath.AddRange(Encoding.Default.GetBytes(tabTagName));
            if (this.RequestPath.Count() % 2 == 1)
            {
                this.RequestPath.Add((byte)0x00);
            }

            for (int i = 0; i < tabValues.Count(); i++)
            {
                if (tabValues[i] < 256)
                {
                    RequestPath.Add((byte)(0x28));
                    RequestPath.Add((byte)(tabValues[i]));
                }
                else if ((tabValues[i] < 65536) && (tabValues[i] > 255))
                {
                    RequestPath.Add((byte)(0x29));
                    RequestPath.Add((byte)(0x00));
                    RequestPath.AddRange(BitConverter.GetBytes((UInt16)tabValues[i]));
                }
                else
                {
                    RequestPath.Add((byte)(0x2A));
                    RequestPath.Add((byte)(0x00));
                    RequestPath.AddRange(BitConverter.GetBytes((UInt32)tabValues[i]));
                }
            }
        }
    }
}
