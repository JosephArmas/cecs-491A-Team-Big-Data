using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamBigData.Utification.Models
{
    public class Picture
    {
        public String extension;
        public long byteLength;
        public byte[] data;
        public int pinID;

        public Picture()
        {
            extension = "";
            byteLength = 0;
            data = new byte[0];
            pinID = 0;
        }

        public Picture(byte[] data, long byteLength, String extension, int pinID)
        {
            this.extension = extension;
            this.byteLength = byteLength;
            this.data = data;
            this.pinID = pinID;
        }
    }
}
