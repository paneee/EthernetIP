using EthernetIP.CIP;
using EthernetIP.EIP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using static EthernetIP.CIP.CIPBase;

namespace EthernetIP.PLC
{
    public class PLCDevice
    {
        public IPAddress IPAddress { get; private set; }                        /* 01 */
        public byte ProcessorSlot { get; private set; }                         /* 02 */
        public int Port { get; private set; }                                   /* 03 */
        public UInt16 VendorID { get; private set; }                            /* 04 */
        public UInt32 Context { get; private set; }                             /* 05 */
        public int ContextPointer;// { get; private set; }                      /* 06 */ 
        public Socket Socket { get; private set; }                              /* 07 */
        public bool SocketConnected { get; private set; }                       /* 09 */
        public UInt32 OTNetworkConnectionID { get; private set; }               /* 10 */
        public UInt32 SessionHandle { get; private set; }                       /* 11 */
        public bool SessionRegistered { get; private set; }                     /* 12 */
        public UInt16 SerialNumber { get; private set; }                        /* 13 */
        public UInt32 OriginatorSerialNumber { get; private set; }              /* 14 */
        public UInt16 SequenceCounter;// { get; private set; }                  /* 15 */ 
        public int Offset { get; private set; }                                 /* 16 */
        //public List<KnowTag> KnownTags { get; set; } = new List<KnowTag>();   /* 17 */
        public UInt16 StructIdentifier { get; private set; }                    /* 18 */
        public List<CIPTypes> CIPTypes { get; private set; }                    /* 19 */
        public PLCDevice(IPAddress ip, byte procesorSlot)
        {
            this.IPAddress = ip;                                                /* 01 */
            this.ProcessorSlot = procesorSlot;                                  /* 02 */
            this.Port = 44818;                                                  /* 03 */
            this.VendorID = 0x1337;                                             /* 04 */
            this.Context = 0x00;                                                /* 05 */
            this.ContextPointer = 0;                                            /* 06 */
            this.Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);      /* 07 */
            this.Socket.ReceiveTimeout = 500;
            this.Socket.SendTimeout = 500;
            this.SocketConnected = false;                                       /* 09 */
            this.OTNetworkConnectionID = 0;                                     /* 10 */
            this.SessionHandle = 0x0000;                                        /* 11 */
            this.SessionRegistered = false;                                     /* 12 */
            this.SerialNumber = (UInt16)(new Random().Next(65000));             /* 13 */
            this.OriginatorSerialNumber = 42;                                   /* 14 */
            this.SequenceCounter = 1;                                           /* 15 */
            this.Offset = 0;                                                    /* 16 */
                                                                                /* 17 */
            this.StructIdentifier = 0x0FCE;                                     /* 18 */
        }

        public bool Connect()
        {
            EIPRegisterSession RegisterSessionPacket = new EIPRegisterSession();

            if (this.SocketConnected)
            {
                return true;
            }

            try
            {
                this.Socket.Connect(this.IPAddress, this.Port);
            }
            catch
            {
                return false;
            }

            var retData = SendData(RegisterSessionPacket.Get());
            if (retData != null)
            {
                this.SessionHandle = BitConverter.ToUInt32((new byte[] { retData[4], retData[5], retData[6], retData[7] }), 0);
            }
            else
            {
                this.SocketConnected = false;
                Console.WriteLine("Failed register session");
                return false;
            }

            CIPForwardOpen CIPForwardOpenPacket = new CIPForwardOpen(this.SerialNumber, this.VendorID, this.OriginatorSerialNumber, this.ProcessorSlot);
            EIPSendRRDataHeader EIPDataHeaderPacket = new EIPSendRRDataHeader(this.SessionHandle, this.Context, CIPForwardOpenPacket.Get().Count());
            retData = SendData(EIPDataHeaderPacket.Get().Concat(CIPForwardOpenPacket.Get()).ToArray());
            if (retData != null)
            {
                if (retData.Count() > 46)
                {
                    this.OTNetworkConnectionID = BitConverter.ToUInt32(new byte[] { retData[44], retData[45], retData[46], retData[47] }, 0);
                    this.SocketConnected = true;
                }
            }
            else
            {
                this.SocketConnected = false;
                Console.WriteLine("Forward Open Failed");
                return false;
            }
            return true;
        }

        public bool Disconect()
        {
            CIPForwardClose CIPForwardClosePacket = new CIPForwardClose(this.SerialNumber, this.VendorID, this.OriginatorSerialNumber, this.ProcessorSlot);
            EIPSendRRDataHeader EIPDataHeaderPacket = new EIPSendRRDataHeader(this.SessionHandle, this.Context, CIPForwardClosePacket.Get().Count());
            var retData = SendData(EIPDataHeaderPacket.Get().Concat(CIPForwardClosePacket.Get()).ToArray());

            EIPUnRegisterSession UnRegisterSessionPacket = new EIPUnRegisterSession();
            retData = SendData(UnRegisterSessionPacket.Get());

            this.Socket.Close();
            return true;
        }

        public object ReadTag(string tag, TAGType tagType)
        {
            return Read(tag, tagType, false, 0);
        }

        public object ReadArray(string tag, TAGType tagType, int arrayLenght)
        {
            return Read(tag, tagType, true, arrayLenght);
        }

        public bool WriteTag(string tag, TAGType tagType, int value)
        {
            return Write(tag, tagType, false, 1, value);
        }

        private object Read(string tag, TAGType tagType, bool isArray, int arrayLenght)
        {
            List<byte> CIPMessageReq = new List<byte>();
            CIPRequestField CIPRequest = new CIPRequestField();
            if (isArray == false)
            {
                CIPMessageReq = CIPRequest.Get(tag, ServiceRequest.Read, tagType, 1, null).ToList();
            }
            else
            {
                CIPMessageReq = CIPRequest.Get(tag, ServiceRequest.Read, tagType, arrayLenght, null).ToList();
            }

            EIPHeader EIPHeaderPacket = new EIPHeader(CIPMessageReq.Count(), this.SessionHandle, ref this.ContextPointer, this.OTNetworkConnectionID, ref this.SequenceCounter);

            byte[] dataReturn = SendData(EIPHeaderPacket.Get().Concat(CIPMessageReq).ToArray());
            CIPReplyField CIPMessageRep = new CIPReplyField();
            CIPMessageRep.Decode(dataReturn);

            if (CIPMessageRep.GeneralStatus == 0)
            {
                if (CIPMessageRep.ReplyData[0] == (byte)tagType)
                {
                    return CIPMessageRep.GetValue(isArray);
                }
                else if ((CIPMessageRep.ReplyData[0] == (byte)TAGType.BOOLARRAY) && isArray)
                {
                    int firselement = int.Parse(tag.Remove(0, tag.LastIndexOf("[")).Replace("]", "").Replace("[", ""));
                    List<bool> retArray = new List<bool>();
                    IList retArrayAll = (IList)CIPMessageRep.GetValue(isArray);

                    if (CIPMessageRep.ReplyData.Count() >= firselement + arrayLenght)
                    {
                        for (int i = firselement; i < firselement + arrayLenght; i++)
                        {
                            retArray.Add((bool)retArrayAll[i]);
                        }
                    }
                    return retArray;
                }
                else
                {
                    throw new Exception("Data type is different");
                }
            }
            else
            {
                CIPError error = new CIPError();
                throw new Exception(error.Codes.Where(p => p.Id == CIPMessageRep.ReplyData[0]).Select(o => o.Description).First());
            }
        }

        private bool Write(string tag, TAGType tagType, bool isArray, int arrayLenght, int value)
        {
            bool result = false;
            List<byte> CIPMessageReq = new List<byte>();
            CIPRequestField CIPRequest = new CIPRequestField();
            if (isArray == false)
            {
                CIPMessageReq = CIPRequest.Get(tag, ServiceRequest.Write, tagType, 1, value).ToList();
            }
            else
            {
                //CIPMessageReq = CIPRequest.Get(tag, ServiceRequest.Read, tagType, arrayLenght, null).ToList();
            }

            EIPHeader EIPHeaderPacket = new EIPHeader(CIPMessageReq.Count(), this.SessionHandle, ref this.ContextPointer, this.OTNetworkConnectionID, ref this.SequenceCounter);

            byte[] dataReturn = SendData(EIPHeaderPacket.Get().Concat(CIPMessageReq).ToArray());
            CIPReplyField CIPMessageRep = new CIPReplyField();
            CIPMessageRep.Decode(dataReturn);

            if (CIPMessageRep.GeneralStatus == 0)
            {
                result = true;
                return result;
            }
            else
            {
                CIPError error = new CIPError();
                throw new Exception(error.Codes.Where(p => p.Id == CIPMessageRep.ReplyData[0]).Select(o => o.Description).First());
            }
        }



        private byte[] SendData(byte[] arrayByte)
        {
            try
            {
                //arrayByte.WriteToConsole("Wysłano: ");
                this.Socket.Send(arrayByte);
                byte[] retData = new byte[1024];
                int length = this.Socket.Receive(retData);
                Array.Resize(ref retData, length);
                //retData.WriteToConsole("Otrzymano: ");

                if (retData != null)
                {
                    return retData.ToArray();
                }
                else
                {
                    this.SocketConnected = false;
                    return null;
                }
            }
            catch
            {
                this.SocketConnected = false;
                return null;
            }
        }
    }
}





