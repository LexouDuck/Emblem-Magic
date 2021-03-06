﻿using EmblemMagic.Components;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EmblemMagic.FireEmblem
{
    public enum BackgroundType
    {
        None = 0,
        Dialog = 1, // Dialog scene background
        Battle = 2, // Battle background
        Screen = 3  // Cutscene screen
    }

    public class Background : TSA_Image
    {
        public Background(
            byte[] palettes,
            byte[] tileset,
            TSA_Array tsa)
            : base(palettes, tileset, tsa) { }

        public Background(
            int width, int height,
            GBA.Bitmap image,
            int paletteAmount,
            bool checkRedundantTiles)
            : base(width, height, image, paletteAmount, checkRedundantTiles) { }

        public Background(
            int width, int height,
            GBA.Bitmap image,
            GBA.Palette palette,
            int paletteAmount,
            bool checkRedundantTiles)
            : base(width, height, image, palette, paletteAmount, checkRedundantTiles) { }



        public static int GetPaletteAmount(BackgroundType bgtype)
        {
            switch (bgtype)
            {
                case BackgroundType.Dialog: return (Core.CurrentROM is FE6) ? 4 : 8;
                case BackgroundType.Battle: return 10;
                case BackgroundType.Screen: return (Core.CurrentROM is FE6) ? 1 : 6;
                default: throw new Exception("background type is null");
            }
        }
        public static Size GetBGSize(BackgroundType bgtype)
        {
            switch (bgtype)
            {
                case BackgroundType.Dialog: return new Size(30, 20);
                case BackgroundType.Battle: return new Size(30, 20);
                case BackgroundType.Screen:
                    return (Core.CurrentROM is FE6) ?
                        new Size(15, 10) :
                        new Size(30, 20);
                default: throw new Exception("background type is null");
            }
        }
    }
}
