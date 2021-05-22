namespace EthernetIP.EIP
{
    class EIPUnRegisterSession : EIPBase
    {
        public EIPUnRegisterSession()
        {
            this.Command = CommandsEnum.UnRegisterSession;          //2b UnRegister Session Command   (Vol 2 2-3.2)
            this.Length = 0x0000;                                   //2b Lenght of Payload          (2-3.3)
            this.SessionHandle = 0x0000;                            //4b Session Handle             (2-3.4)
            this.Status = StatusEnum.Success;                       //4b Status always 0x00         (2-3.5)
            this.SenderContext = 0x0000;                            //8b                            (2-3.6)
            this.Options = 0x0000;                                  //4b Options always 0x00        (2-3.7)
        }
    }
}
