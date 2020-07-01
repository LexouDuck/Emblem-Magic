using System;
using System.Collections;

namespace EmblemMagic.Editors
{
    public class BitInfo
    {
        public bool this[int index]
        {
            get
            {
                return (_value & (1 << index)) != 0;
            }
            set
            {
                if (value)
                    _value |= (byte)(1 << index); //set bit index 1
                else
                    _value &= (byte)(~(1 << index)); //set bit index 0
            }
        }


        
		public byte Value
		{
			get { return _value; }
			set { _value = value; }
        } byte _value;

        public long Position { get; set; }



		public BitInfo(byte value, long position)
		{
			_value = value;
			Position = position;
		}



		public override string ToString()
		{
			var result = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}"
				, GetBitAsString(7)
				, GetBitAsString(6)
				, GetBitAsString(5)
				, GetBitAsString(4)
				, GetBitAsString(3)
				, GetBitAsString(2)
				, GetBitAsString(1)
				, GetBitAsString(0));
			return result;
		}

		public string GetBitAsString(int index)
		{
			if (this[index])
                return "1";
			else return "0";
		}
	}
}
