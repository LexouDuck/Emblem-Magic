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
                return (this._value & (1 << index)) != 0;
            }
            set
            {
                if (value)
                    this._value |= (Byte)(1 << index); //set bit index 1
                else
                    this._value &= (Byte)(~(1 << index)); //set bit index 0
            }
        }


        
		public Byte Value
		{
			get { return this._value; }
			set { this._value = value; }
        }
        Byte _value;

        public Int64 Position { get; set; }



		public BitInfo(Byte value, Int64 position)
		{
            this._value = value;
            this.Position = position;
		}



		public override String ToString()
		{
			var result = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}"
				, this.GetBitAsString(7)
				, this.GetBitAsString(6)
				, this.GetBitAsString(5)
				, this.GetBitAsString(4)
				, this.GetBitAsString(3)
				, this.GetBitAsString(2)
				, this.GetBitAsString(1)
				, this.GetBitAsString(0));
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
