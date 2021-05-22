namespace EthernetIP.CIP
{
    public class CIPBase
    {
        public enum FrameConstant : byte
        {
            RequestPathStart = 0x91,
        }

        public enum ServiceRequest : byte
        {
            Read = 0x4C,
            Write = 0x4D,
            ReadTagFragmented = 0x52,
            WriteTagFragmented = 0x53,
        }

        public enum ServiceReply : byte
        {
            Read = 0xCC,
            Write = 0xCD,
        }

        public enum DataType
        {
            BOOL = 0x00C1,      //1 byte
            SINT = 0x00C2,      //1 byte
            INT = 0x00C3,       //2 bytes
            DINT = 0x00C4,      //4 bytes
            REAL = 0x00CA,      //4 bytes
            DWORD = 0x00D3,     //4 bytes
            LINT = 0x00C5,      //8 bytes

            STRUCT = 0x00A0,      //8 bytes
        }
    }
}
