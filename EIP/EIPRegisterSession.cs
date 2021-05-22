using System;

namespace EthernetIP.EIP
{
    public class EIPRegisterSession : EIPBase
    {
        private UInt16 ProtocolVersion = 0x01;                      //2b Always 0x01                (2-4.7)
        private UInt16 OptionFlag = 0x00;                           //2b Always 0x00                (2-4.7)

        public EIPRegisterSession()
        {
            this.Command = CommandsEnum.RegisterSession;            //2b Register Session Command   (Vol 2 2-3.2)
            this.Length = 0x0004;                                   //2b Lenght of Payload          (2-3.3)
            this.SessionHandle = 0x0000;                            //4b Session Handle             (2-3.4)
            this.Status = StatusEnum.Success;                       //4b Status always 0x00         (2-3.5)
            this.SenderContext = 0x0000;                            //8b                            (2-3.6)
            this.Options = 0x0000;                                  //4b Options always 0x00        (2-3.7)

            this.CommandSpecificData.Add((byte)ProtocolVersion);
            this.CommandSpecificData.Add((byte)((UInt16)ProtocolVersion >> 8));

            this.CommandSpecificData.Add((byte)OptionFlag);
            this.CommandSpecificData.Add((byte)((UInt16)OptionFlag >> 8));
        }
    }
}
