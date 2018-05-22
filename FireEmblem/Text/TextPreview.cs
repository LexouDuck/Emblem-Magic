using EmblemMagic.Components;
using GBA;
using System;
using System.Collections.Generic;

namespace EmblemMagic.FireEmblem
{
    public class TextPreview : IDisplayable
    {
        public Color this[int x, int y]
        {
            get
            {
                if (Text[x, y] == Glyph.Colors[0])
                {
                    if (Bubble == null
                     || Bubble.Tiling[x / 8, y / 8].TileIndex == 0xF
                     || Bubble[x, y] == Bubble.Palettes[0][0])
                        return Glyph.Colors[0];
                    else return Bubble[x, y];
                }
                else return Text[x, y];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                return 240;
            }
        }
        public int Height
        {
            get
            {
                return 64;
            }
        }

        Bitmap Text { get; }

        TSA_Image Bubble { get; }



        public TextPreview(Font font, bool bubble, string[] text, int line)
        {
            Text = new Bitmap(Width, Height);
            for (int i = 0; i < Glyph.Colors.Length; i++)
            {
                Text.Colors.Add(Glyph.Colors[i]);
            }
            Dictionary<char, byte> fontmap = (Core.CurrentROM.Version == GameVersion.JAP) ?
                Font.GetFontMap(bubble) : Font.GetFontMap();
            List<Glyph>[] glyphs = new List<Glyph>[text.Length];
            int[] lengths = new int[text.Length];
            int length;
            int offset_x;
            int offset_y;
            for (int i = 0; i < text.Length; i++)
            {
                glyphs[i] = new List<Glyph>();
                for (int j = 0; j < text[i].Length; j++)
                {
                    glyphs[i].Add(font.GetGlyph(text[i][j], fontmap));

                    if (glyphs[i][glyphs[i].Count - 1] != null)
                        lengths[i] += glyphs[i][glyphs[i].Count - 1].TextWidth;
                }
            }
            int lines = Math.Min(bubble ? 2 : 4, text.Length);
            for (int i = line; i < line + lines; i++)
            {
                offset_x =  bubble ? 8 : Width / 2 - lengths[i] / 2;
                offset_y = (bubble ? 8 : 0) + (i - line) * 16;

                length = 0;
                for (int j = 0; j < glyphs[i].Count; j++)
                {
                    if (glyphs[i][j] != null)
                    {
                        for (int y = 0; y < 16; y++)
                        for (int x = 0; x < 16; x++)
                        {
                            if (offset_x + length + x < Width && offset_y + y < Height && glyphs[i][j][x, y] != Glyph.Colors[0])
                                Text[offset_x + length + x, offset_y + y] = glyphs[i][j][x, y];
                        }
                        length += glyphs[i][j].TextWidth;
                    }
                }
            }

            length = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (lengths[i] > length) length = lengths[i];
            }   // Store the pixel length of the biggest line

            if (bubble)
            {
                Bubble = MakeTextBubble(length,
                    Core.GetPointer("Text Bubble Tileset"),
                    Core.GetPointer("Text Bubble Palette"));
            }
        }



        static TSA_Image MakeTextBubble(int longestLine, Pointer tilesetAddress, Pointer paletteAddress)
        {
            Palette palette = Core.ReadPalette(paletteAddress, Palette.LENGTH);

            Tileset tileset = new Tileset(Core.ReadData(tilesetAddress, 0));

            TSA_Array tsa = new TSA_Array(30, 8);
            for (int y = 0; y < 8; y++)
            for (int x = 0; x < 30; x++)
            {
                tsa.SetTile(x, y, 0xF);
            }

            int length = 2;
            if (longestLine / 8 > length)
            {
                length = longestLine / 8;
                if (longestLine % 8 != 0)
                    length++;
            }

            tsa.SetTile(0, 0, 0x0);
            for (int i = 1; i <= length; i++)
            {
                tsa.SetTile(i, 0, 0x1);
            }
            tsa.SetTile(0, 1, 0x2);
            tsa.SetTile(0, 2, 0x2);
            tsa.SetTile(0, 3, 0x2);
            tsa.SetTile(0, 4, 0x2);
            tsa.SetTile(0, 5, 0x0); tsa.SetFlipV(0, 5, true);
            for (int i = 2; i <= length; i++)
            {
                tsa.SetTile(i, 5, 0x1);
                tsa.SetFlipV(i, 5, true);
            }
            tsa.SetTile(1, 5, 0x4);
            tsa.SetTile(2, 5, 0x4); tsa.SetFlipH(2, 5, true); tsa.SetFlipV(2, 5, false);
            tsa.SetTile(1, 6, 0x6); tsa.SetFlipH(1, 6, true);
            tsa.SetTile(2, 6, 0x5); tsa.SetFlipH(2, 6, true);
            tsa.SetTile(1 + length, 0, 0x0); tsa.SetFlipH(1 + length, 0, true);
            tsa.SetTile(1 + length, 1, 0x2); tsa.SetFlipH(1 + length, 1, true);
            tsa.SetTile(1 + length, 2, 0x2); tsa.SetFlipH(1 + length, 2, true);
            tsa.SetTile(1 + length, 3, 0x2); tsa.SetFlipH(1 + length, 3, true);
            tsa.SetTile(1 + length, 4, 0x2); tsa.SetFlipH(1 + length, 4, true);
            tsa.SetTile(1 + length, 5, 0x0); tsa.SetFlipH(1 + length, 5, true); tsa.SetFlipV(1 + length, 5, true);
            for (int y = 1; y <= 4; y++)
            for (int x = 1; x <= length; x++)
            {
                tsa.SetTile(x, y, 0x3);
            }

            return new TSA_Image(palette, tileset, tsa);
        }
    }
}
