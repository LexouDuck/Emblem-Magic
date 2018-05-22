using EmblemMagic.Components;
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
        /// This indexer allows for fast access to pixel data in GBA.Color format - for IDisplayable
        /// </summary>
        public Color this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);
                
                int index = GetSpriteIndex(x, y);
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

                    return Sprites[index][x, y];
                }
            }
        }

        /// <summary>
        /// The width of this GBA.SpriteSheet, in pixels
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// The height of this GBA.SpriteSheet, in pixels
        /// </summary>
        public int Height { get; }



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
        public int Count
        {
            get
            {
                return Sprites.Count;
            }
        }



        /// <summary>
        /// Creates an empty spritesheet with the given dimensions
        /// </summary>
        public SpriteSheet(int width, int height)
        {
            Width = width;
            Height = height;
            Offsets = new List<Point>();
            Sprites = new List<Sprite>();
        }


        
        /// <summary>
        /// Adds a sprite to the spritesheet, at the given coordinates
        /// </summary>
        public void AddSprite(Sprite sprite, int offsetX, int offsetY)
        {
            Offsets.Add(new Point(offsetX, offsetY));
            Sprites.Add(sprite);
        }
        /// <summary>
        /// Adds a compound sprite made from an OAM array onto the spritesheet
        /// </summary>
        public void AddSprite(Palette palette, Tileset tileset, OAM_Array oam, int offsetX, int offsetY, bool showAffines = true)
        {
            Sprite sprite;
            for (int i = 0; i < oam.Sprites.Count; i++)
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

        /// <summary>
        /// Returns the index of the sprite in this GBA.SpriteSheet that corresponds to the tile coordinates given
        /// </summary>
        private int GetSpriteIndex(int x, int y)
        {
            for (int i = 0; i < Count; i++)
            {
                if ((x >= Offsets[i].X) && (x < Offsets[i].X + Sprites[i].Width) &&
                    (y >= Offsets[i].Y) && (y < Offsets[i].Y + Sprites[i].Height))
                {
                    if (Sprites[i][x - Offsets[i].X, y - Offsets[i].Y] == Sprites[i].Colors[0])
                        continue;
                    else return i;
                }
            }
            return -1;
        }
    }
}
