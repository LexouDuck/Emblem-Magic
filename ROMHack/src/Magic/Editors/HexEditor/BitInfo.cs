using System;
using System.Collections;

namespace Magic.Editors
{
    public class BitInfo
    {
        public Boolean this[Int32 index]
        {
            get
            {
                return (_value & (1 << index)) != 0;
            }
            set
            {
                if (value)
                    _value |= (Byte)(1 << index); //set bit index 1
                else
                    _value &= (Byte)(~(1 << index)); //set bit index 0
            }
        }


        
		public Byte Value
		{
			get { return _value; }
			set { _value = value; }
        }
        Byte _value;

        public Int64 Position { get; set; }



		public BitInfo(Byte value, Int64 position)
		{
			_value = value;
			Position = position;
		}



		public override String ToString()
		{
			var result = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}"
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

		public String GetBitAsString(Int32 index)
		{
			if (this[index])
                return "1";
			else return "0";
		}
	}
}
