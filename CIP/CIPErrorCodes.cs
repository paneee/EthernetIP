using System.Collections.Generic;

namespace EthernetIP.CIP
{
    public class CIPError
    {
        public List<CIPErrorCode> Codes { get; private set; } = new List<CIPErrorCode>();

        public CIPError()
        {
            this.Codes.Add(new CIPErrorCode(0x00, "Success"));
            this.Codes.Add(new CIPErrorCode(0x00, "Success"));
            this.Codes.Add(new CIPErrorCode(0x01, "Connection failure"));
            this.Codes.Add(new CIPErrorCode(0x02, "Resource unavailable"));
            this.Codes.Add(new CIPErrorCode(0x03, "Invalid parameter value"));
            this.Codes.Add(new CIPErrorCode(0x04, "Path segment error"));
            this.Codes.Add(new CIPErrorCode(0x05, "Path destination unknown"));
            this.Codes.Add(new CIPErrorCode(0x06, "Partial transfer"));
            this.Codes.Add(new CIPErrorCode(0x07, "Connection lost"));
            this.Codes.Add(new CIPErrorCode(0x08, "Service not supported"));
            this.Codes.Add(new CIPErrorCode(0x09, "Invalid Attribute"));
            this.Codes.Add(new CIPErrorCode(0x0A, "Attribute list error"));
            this.Codes.Add(new CIPErrorCode(0x0B, "Already in requested mode/state"));
            this.Codes.Add(new CIPErrorCode(0x0C, "Object state conflict"));
            this.Codes.Add(new CIPErrorCode(0x0D, "Object already exists"));
            this.Codes.Add(new CIPErrorCode(0x0E, "Attribute not settable"));
            this.Codes.Add(new CIPErrorCode(0x0F, "Privilege violation"));
            this.Codes.Add(new CIPErrorCode(0x10, "Device state conflict"));
            this.Codes.Add(new CIPErrorCode(0x11, "Reply data too large"));
            this.Codes.Add(new CIPErrorCode(0x12, "Fragmentation of a premitive value"));
            this.Codes.Add(new CIPErrorCode(0x13, "Not enough data"));
            this.Codes.Add(new CIPErrorCode(0x14, "Attribute not supported"));
            this.Codes.Add(new CIPErrorCode(0x15, "Too much data"));
            this.Codes.Add(new CIPErrorCode(0x16, "Object does not exist"));
            this.Codes.Add(new CIPErrorCode(0x17, "Service fragmentation sequence not in progress"));
            this.Codes.Add(new CIPErrorCode(0x18, "No stored attribute data"));
            this.Codes.Add(new CIPErrorCode(0x19, "Store operation failure"));
            this.Codes.Add(new CIPErrorCode(0x1A, "Routing failure, request packet too large"));
            this.Codes.Add(new CIPErrorCode(0x1B, "Routing failure, response packet too large"));
            this.Codes.Add(new CIPErrorCode(0x1C, "Missing attribute list entry data"));
            this.Codes.Add(new CIPErrorCode(0x1D, "Invalid attribute value list"));
            this.Codes.Add(new CIPErrorCode(0x1E, "Embedded service error"));
            this.Codes.Add(new CIPErrorCode(0x1F, "Vendor specific"));
            this.Codes.Add(new CIPErrorCode(0x20, "Invalid Parameter"));
            this.Codes.Add(new CIPErrorCode(0x21, "Write once value or medium already written"));
            this.Codes.Add(new CIPErrorCode(0x22, "Invalid reply received"));
            this.Codes.Add(new CIPErrorCode(0x23, "Buffer overflow"));
            this.Codes.Add(new CIPErrorCode(0x24, "Invalid message format"));
            this.Codes.Add(new CIPErrorCode(0x25, "Key failure in path"));
            this.Codes.Add(new CIPErrorCode(0x26, "Path size invalid"));
            this.Codes.Add(new CIPErrorCode(0x27, "Unexpected attribute in list"));
            this.Codes.Add(new CIPErrorCode(0x28, "Invalid member ID"));
            this.Codes.Add(new CIPErrorCode(0x29, "Member not settable"));
            this.Codes.Add(new CIPErrorCode(0x2A, "Group 2 only server general failure"));
            this.Codes.Add(new CIPErrorCode(0x2B, "Unknown Modbus error"));
            this.Codes.Add(new CIPErrorCode(0x2C, "Attribute not gettable"));
        }
    }


    public class CIPErrorCode
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public CIPErrorCode(int id, string description)
        {
            this.Id = id;
            this.Description = description;
        }
    }
}

