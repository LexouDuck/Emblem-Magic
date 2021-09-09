using Magic.Components;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// An image onto which one or several GBA.Sprite objects can be drawn.
    /// </summary>
    public class SpriteSheet : IDisplayable
    {
        /// <summary>
        /// This indexer allows for fast access to pixel data in indexed color format - for IDisplayable
        /// </summary>
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                Int32 index = GetSpriteIndex(x, y);
                if (index == -1)
                {
                    return 0;
                }
                else
                {
                    x -= Offsets[index].X;
                    y -= Offsets[index].Y;
                    return Sprites[index][x, y];
                }
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            Int32 index = GetSpriteIndex(x, y);
            if (index == -1)
            {
                if (Sprites.Count == 0)
                    return new Color();
                else return Sprites[0].Colors[0];
            }
            else
            {
                x -= Offsets[index].X;
                y -= Offsets[index].Y;

                return Sprites[index].GetColor(x, y);
            }
        }
        /// <summary>
        /// Returns the index of the sprite in this GBA.SpriteSheet that corresponds to the tile coordinates given
        /// </summary>
        public Int32 GetSpriteIndex(Int32 x, Int32 y)
        {
            if (x < 0 || x >= Width) throw new ArgumentException("X given is out of bounds: " + x);
            if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

            for (Int32 i = 0; i < Count; i++)
            {
                if ((x >= Offsets[i].X) && (x < Offsets[i].X + Sprites[i].Width) &&
                    (y >= Offsets[i].Y) && (y < Offsets[i].Y + Sprites[i].Height))
                {
                    if (Sprites[i].GetColor(x - Offsets[i].X, y - Offsets[i].Y) == Sprites[i].Colors[0])
                        continue;
                    else return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// The width of this GBA.SpriteSheet, in pixels
        /// </summary>
        public Int32 Width { get; }
        /// <summary>
        /// The height of this GBA.SpriteSheet, in pixels
        /// </summary>
        public Int32 Height { get; }



        /// <summary>
        /// The offsets at which to display the tilemaps on the SpriteSheet, in pixels
        /// </summary>
        public List<Point> Offsets;
        /// <summary>
        /// The tilemaps that make up this SpriteSheet
        /// </summary>
        public List<Sprite> Sprites;

        /// <summary>
        /// How many different sprites this SpriteSheet holds
        /// </summary>
        public Int32 Count
        {
            get
            {
                return Sprites.Count;
            }
        }



        /// <summary>
        /// Creates an empty spritesheet with the given dimensions
        /// </summary>
        public SpriteSheet(Int32 width, Int32 height)
        {
            Width = width;
            Height = height;
            Offsets = new List<Point>();
            Sprites = new List<Sprite>();
        }


        
        /// <summary>
        /// Adds a sprite to the spritesheet, at the given coordinates
        /// </summary>
        public void AddSprite(Sprite sprite, Int32 offsetX, Int32 offsetY)
        {
            Offsets.Add(new Point(offsetX, offsetY));
            Sprites.Add(sprite);
        }
        /// <summary>
        /// Adds a compound sprite made from an OAM array onto the spritesheet
        /// </summary>
        public void AddSprite(Palette palette, Tileset tileset, OAM_Array oam, Int32 offsetX, Int32 offsetY, Boolean showAffines = true)
        {
            Sprite sprite;
            for (Int32 i = 0; i < oam.Sprites.Count; i++)
            {
                if (oam[i].IsAffineSprite())
                {
                    if (!showAffines) continue;
                    sprite = new Sprite(palette, tileset, oam[i], oam.Affines[oam[i].AffineIndex]);
                }
                else sprite = new Sprite(palette, tileset, oam[i]);

                this.AddSprite(sprite,
                    offsetX + oam[i].ScreenX,
                    offsetY + oam[i].ScreenY);
            }
        }
        /// <summary>
        /// Clears this SpriteSheet of all its sprites
        /// </summary>
        public void Clear()
        {
            Offsets = new List<Point>();
            Sprites = new List<Sprite>();
        }

        public Boolean IsRegionEmpty(Rectangle region)
        {
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
            {
                if (this[x + region.X, y + region.Y] > 0)
                    return false;
            }
            return true;
        }
    }
}
