// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.GBA.GBAPointerMaker
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

namespace Nintenlord.Event_Assembler.Core.GBA
{
	public class GBAPointerMaker : IPointerMaker
	{
		public int MakePointer (int offset)
		{
			if (offset == 0)
				return 0;
			
			return offset | 0x08000000;
		}

		public int MakeOffset (int pointer)
		{
			return pointer & 0x1FFFFFF;
		}

		public bool IsAValidPointer (int pointer)
		{
			if (pointer != 0)
				return pointer >> 25 == 4;
			return true;
		}
	}
}
