using System;
using System.Drawing;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class WorldMap_FE6_Small : GBA.Bitmap
    {
        public WorldMap_FE6_Small(String path) : base(path)
        {
            if (this.Width != 240 || this.Height != 160)
                throw new Exception("Image given has invalid dimensions. It should be 240x160 pixels.");

            for (Int32 i = this.Colors.Count; i < 256; i++)
            {
                this.Colors.Add(new GBA.Color(0x0000));
            }
        }

        public WorldMap_FE6_Small(
            Pointer palette,
            Pointer tiles)
            : base(240, 160,
                Core.ReadData(Core.ReadPointer(palette), 0),
                Core.ReadData(Core.ReadPointer(tiles), 0))
        {
            for (Int32 i = 0; i < 256; i++)
            {
                this.Colors[i] = this.Colors[i].SetAlpha(false);
            }
        }
    }



    public class WorldMap_FE6_Large : IDisplayable
    {
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                Int32 i = (((x < Screen.WIDTH) ? 0 : 1) + ((y < Screen.HEIGHT) ? 0 : 2));
                return this.Graphics[i][x % Screen.WIDTH, y % Screen.HEIGHT];
            }
        }
        public GBA.Color GetColor(Int32 x, Int32 y)
        {
            Int32 i = (((x < Screen.WIDTH) ? 0 : 1) + ((y < Screen.HEIGHT) ? 0 : 2));
            return this.Graphics[i].GetColor(x % Screen.WIDTH, y % Screen.HEIGHT);
        }

        public Int32 Width
        {
            get
            {
                return WIDTH * 8;
            }
        }
        public Int32 Height
        {
            get
            {
                return HEIGHT * 8;
            }
        }

        public const Int32 WIDTH = 60;
        public const Int32 HEIGHT = 40;

        public GBA.Bitmap[] Graphics { get; set; }



        public WorldMap_FE6_Large(GBA.Bitmap image)
        {
            if (image.Width != this.Width || image.Height != this.Height)
                throw new Exception("Image given has invalid dimensions. It should be 480x320 pixels.");

            for (Int32 i = image.Colors.Count; i < 256; i++)
            {
                image.Colors.Add(new GBA.Color(0x0000));
            }
            this.Graphics = new GBA.Bitmap[4];
            for (Int32 i = 0; i < 4; i++)
            {
                this.Graphics[i] = new GBA.Bitmap(image, new Rectangle(
                        ((i & 0x1) == 0) ? 0 : 240,
                        ((i & 0x2) == 0) ? 0 : 160,
                        240, 160));
            }
        }

        public WorldMap_FE6_Large(
            Pointer palette,
            Pointer tiles)
        {
            Pointer[] palettes = GetPointerArray(palette);
            Pointer[] tilesets = GetPointerArray(tiles);
            this.Graphics = new GBA.Bitmap[4];
            for (Int32 i = 0; i < 4; i++)
            {
                this.Graphics[i] = new GBA.Bitmap(240, 160,
                    Core.ReadData(palettes[i], 0),
                    Core.ReadData(tilesets[i], 0));
                for (Int32 p = 0; p < 256; p++)
                {
                    this.Graphics[i].Colors[p] = this.Graphics[i].Colors[p].SetAlpha(false);
                }
            }
        }

        public static Pointer[] GetPointerArray(Pointer address)
        {
            Pointer[] result = new Pointer[4];
            for (Int32 i = 0; i < 4; i++)
            {
                result[i] = Core.ReadPointer(address + i * 8);
            }
            return result;
        }
    }
}
