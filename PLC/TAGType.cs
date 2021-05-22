namespace EthernetIP.PLC
{
    public enum TAGType
    {
        BOOL = 193,         //DataLenght = 1, DataName = "BOOL", DataNameChar = '?'
        SINT = 194,         //DataLenght = 1, DataName = "SINT", DataNameChar = 'b'
        INT = 195,          //DataLenght = 2, DataName = "INT", DataNameChar = 'h' 
        DINT = 196,         //DataLenght = 4, DataName = "DINT", DataNameChar = 'i' 
        REAL = 202,         //DataLenght = 4, DataName = "REAL", DataNameChar = 'f' 
        STRING = 160,       //String it is struct;
        BOOLARRAY = 211,    //DataLenght = 4, DataName = "DWORD", DataNameChar = 'I' 

        //STRUCT = 160, //DataLenght = 0, DataName = "STRUCT", DataNameChar = 'B'
        //DWORD = 211,        //DataLenght = 4, DataName = "DWORD", DataNameChar = 'I' 
    }
}
