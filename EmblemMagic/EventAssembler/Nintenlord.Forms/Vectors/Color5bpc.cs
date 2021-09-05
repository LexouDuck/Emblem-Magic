using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Nintenlord.Forms.Vectors
{
    [StructLayout(LayoutKind.Explicit, Pack = 2, Size = 2)]
    public struct Color5bpc : IPrimitiveVector<ushort>, IPrimitiveVector<short>, IRGBColor
    {
        [FieldOffset(0)]
        ushort value;

        const int Shift = sizeof(byte) - 5;
        const int Mask = 0x1F;
        const int Size = 5;

        #region IPrimitiveVector<ushort> Members

        ushort IPrimitiveVector<ushort>.Primitive
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion

        #region IPrimitiveVector<short> Members

        short IPrimitiveVector<short>.Primitive
        {
            get
            {
                return (short)value;
            }
            set
            {
                this.value = (ushort)value;
            }
        }

        #endregion
        
        #region IColorVector Members


        public int R
        {
            get
            {
                return value & 0x1F;
            }
            set
            {
                int newValue = this.value;
                newValue &= ~Mask;
                newValue |= value >> Shift;
                this.value = (ushort)newValue;
            }
        }

        public int G
        {
            get
            {
                return (value >> 5) & 0x1F;
            }
            set
            {
                int newValue = this.value;
                newValue &= ~(Mask << 5);
                newValue |= (value >> Shift) << 5;
                this.value = (ushort)newValue;
            }
        }

        public int B
        {
            get
            {
                return (value >> 10) & 0x1F;
            }
            set
            {
                int newValue = this.value;
                newValue &= ~(Mask << 10);
                newValue |= (value >> Shift) << 10;
                this.value = (ushort)newValue;
            }
        }

        #endregion
    }
}
