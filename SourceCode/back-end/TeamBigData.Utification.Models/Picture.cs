using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class Picture
    {
        public String Extension;
        public long ByteLength;
        public byte[] Data;
        public int PinID;

        public Picture()
        {
            Extension = "";
            ByteLength = 0;
            Data = new byte[0];
            PinID = 0;
        }

        public Picture(byte[] data, long byteLength, String extension, int pinID)
        {
            Extension = extension;
            ByteLength = byteLength;
            Data = data;
            PinID = pinID;
        }
    }
}
