using System;
using Magic.Components;
using Magic;
using System.Collections.Generic;

namespace GBA
{
    public class TSA_Image : IDisplayable
    {
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                Int32 tileX = x / Tile.SIZE;
                Int32 tileY = y / Tile.SIZE;
                Tile tile = Graphics[Tiling[tileX, tileY].TileIndex];
                Int32 palette = Tiling[tileX, tileY].Palette;
                Boolean flipH = Tiling[tileX, tileY].FlipH;
                Boolean flipV = Tiling[tileX, tileY].FlipV;
                tileX = flipH ? 7 - x % Tile.SIZE : x % Tile.SIZE;
                tileY = flipV ? 7 - y % Tile.SIZE : y % Tile.SIZE;
                return palette * Palette.MAX + tile[tileX, tileY];
            }
        }
        public Byte GetPaletteIndex(Int32 x, Int32 y)
        {
            if (x < 0 || x >= Width) throw new ArgumentException("X given is out of bounds: " + x);
            if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

            Int32 tileX = x / Tile.SIZE;
            Int32 tileY = y / Tile.SIZE;
            Tile tile = Graphics[Tiling[tileX, tileY].TileIndex];
            return Tiling[tileX, tileY].Palette;
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            if (x < 0 || x >= Width) throw new ArgumentException("X given is out of bounds: " + x);
            if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

            Int32 tileX = x / Tile.SIZE;
            Int32 tileY = y / Tile.SIZE;
            Tile tile = Graphics[Tiling[tileX, tileY].TileIndex];
            Int32 palette = Tiling[tileX, tileY].Palette;
            Boolean flipH = Tiling[tileX, tileY].FlipH;
            Boolean flipV = Tiling[tileX, tileY].FlipV;
            tileX = flipH ? 7 - x % Tile.SIZE : x % Tile.SIZE;
            tileY = flipV ? 7 - y % Tile.SIZE : y % Tile.SIZE;
            return Palettes[palette][tile[tileX, tileY]];
        }
        /// <summary>
        /// The width of this TSA_Image, in pixels
        /// </summary>
        public Int32 Width { get; }
        /// <summary>
        /// The height of this TSA_Image, in pixels
        /// </summary>
        public Int32 Height { get; }

        /// <summary>
        /// The array of 16-color Palettes that this TSA_Image uses
        /// </summary>
        public Palette[] Palettes { get; protected set; }
        /// <summary>
        /// The tiles that make up this TSA_Image
        /// </summary>
        public Tileset Graphics { get; protected set; }
        /// <summary>
        /// The TSA tile/palette layout information
        /// </summary>
        public TSA_Array Tiling { get; protected set; }



        public TSA_Image(Int32 width, Int32 height)
        {
            Width = width * 8;
            Height = height * 8;
        }
        public TSA_Image(
            Byte[] palettes,
            Byte[] graphics,
            TSA_Array tsa)
        {
            if (palettes == null)
                throw new Exception("Given palette data is null");
            if (graphics == null)
                throw new Exception("Given tileset data is null");
            if (tsa == null)
                throw new Exception("Given TSA Array is null");
            if (palettes.Length % GBA.Palette.LENGTH != 0)
                throw new Exception("Given palette data has invalid length");

            Width = tsa.Width * 8;
            Height = tsa.Height * 8;

            Tiling = tsa;

            Graphics = new Tileset(graphics);

            Palettes = new Palette[palettes.Length / GBA.Palette.LENGTH];
            Byte[] buffer = new Byte[GBA.Palette.LENGTH];
            for (Int32 p = 0; p < Palettes.Length; p++)
            {
                Array.Copy(palettes, p * GBA.Palette.LENGTH, buffer, 0, GBA.Palette.LENGTH);
                Palettes[p] = new Palette(buffer);
            }
        }
        public TSA_Image(
            Palette palettes,
            Tileset graphics,
            TSA_Array tsa)
        {
            if (palettes == null)
                throw new Exception("Given palette data is null");
            if (graphics == null)
                throw new Exception("Given tileset data is null");
            if (palettes.Count % GBA.Palette.MAX != 0)
                throw new Exception("Given palette data has invalid length");

            Width = tsa.Width * 8;
            Height = tsa.Height * 8;

            Tiling = tsa;
            Graphics = graphics;
            Palettes = Palette.Split(palettes, palettes.Count / Palette.MAX);
        }
        /// <summary>
        /// Automatically generates a TSA-compliant image with the specified palette
        /// </summary>
        /// <param name="width">Width of the TSA (in tiles)</param>
        /// <param name="height">Height of the TSA (in tiles)</param>
        /// <param name="image">The bitmap to convert</param>
        /// <param name="paletteAmount">the maximum amount of 16-color palettes</param>
        /// <param name="checkRedundantTiles">whether or not to add all tiles found to the tileset</param>
        public TSA_Image(
            Int32 width, Int32 height,
            GBA.Bitmap image,
            GBA.Palette palette,
            Int32 paletteAmount,
            Boolean checkRedundantTiles)
        {
            Width = width * 8;
            Height = height * 8;

            if (image.Width != Width || image.Height != Height)
                throw new Exception("Image given has invalid dimensions.\n" +
                    "It must be " + Width + "x" + Height + " pixels.");

            while (palette.Count < paletteAmount * Palette.MAX)
            {
                palette.Add(new GBA.Color());
            }

            if (image.Colors.Count <= 16)
            {   // no need to run TSA-ifier code
                Palettes = Palette.Split(palette, paletteAmount);

                Graphics = new Tileset(new Image(image, palette));

                Tiling = TSA_Array.GetBasicTSA(width, height);
            }
            else using (FormLoading loading = new FormLoading())
            {   // run the image TSA-ifier and show a loading bar window
                loading.Show();

                    Int32 tileAmount = width * height;
                    Byte[] bytes = image.ToBytes();
                    Int32[] colorTotals = new Int32[image.Colors.Count];
                    Int32[,] colorAmounts = new Int32[image.Colors.Count, tileAmount];

                loading.SetLoading("Checking colors...", 2);

                    Int32 index;
                    Int32 tile_index = 0;
                for (Int32 tileY = 0; tileY < height; tileY++)
                for (Int32 tileX = 0; tileX < width; tileX++)
                {
                    index = tileX * 8 + tileY * 8 * Width;
                    for (Int32 y = 0; y < 8; y++)
                    {
                        for (Int32 x = 0; x < 8; x++)
                        {
                            colorTotals[bytes[index]]++;
                            colorAmounts[bytes[index], tile_index]++;
                        }
                        index += Width - 8;
                    }
                    tile_index++;
                }   // first we take a look at which colors are most present (in all, and per tile)

                loading.SetPercent(7);

                List<Int32> colors = new List<Int32>();
                for (Int32 i = 0; i < colorTotals.Length; i++)
                {
                    index = 0;
                    while (index < colors.Count && colorTotals[i] > colors[index])
                        index++;

                    if (index < colors.Count)
                        colors.Insert(index, i);
                    else colors.Add(i);

                    if (colors.Count > paletteAmount * 15)
                        colors.RemoveAt(0);
                }   // then we create a list of indices to colors with only the most used colors

                loading.SetPercent(9);

                Palettes = Palette.Split(palette, paletteAmount);

                loading.SetLoading("Asserting tile palettes...", 10);

                    Byte[] tilePalettes = new Byte[tileAmount];
                    Int32[] certainty = new Int32[tileAmount];
                GBA.Color color;
                    Int32 amount;
                for (Int32 i = colors.Count - 1; i >= 0; i--)
                {
                    color = image.Colors[colors[i]];
                    tile_index = 0;

                    while (tile_index < tilePalettes.Length)
                    {
                        amount = colorAmounts[colors[i], tile_index];

                        if (Palettes[tilePalettes[tile_index]].Contains(color))
                        {
                            certainty[tile_index] += amount;
                        }
                        else
                        {
                            if (certainty[tile_index] < amount)
                            {
                                tilePalettes[tile_index]++;
                                tilePalettes[tile_index] %= (Byte)paletteAmount;
                                certainty[tile_index] = amount;
                                i++; break;
                            }
                            else certainty[tile_index] -= amount;
                        }
                        tile_index++;
                    }
                    loading.SetPercent(10 + (50 - 50 * ((Single)i / (Single)(colors.Count))));
                }   // and we do a loop going from  most used color to least used, setting tilePalettes

                loading.SetMessage("Creating TSA information...");

                Graphics = new Tileset(width * height);
                Tiling = new TSA_Array(width, height);
                    Int32 pixel;
                tile_index = 0;
                    Byte HI_nibble;
                    Byte LO_nibble;
                Tile tile;
                Tuple<Int32, Boolean, Boolean> current;
                for (Int32 tileY = 0; tileY < height; tileY++)
                for (Int32 tileX = 0; tileX < width; tileX++)
                {
                    index = (Palettes[tilePalettes[tile_index]].IsFull) ? tilePalettes[tile_index] : 0;
                    bytes = new Byte[GBA.Tile.LENGTH];

                    for (Int32 y = 0; y < 8; y++)
                    for (Int32 x = 0; x < 4; x++)
                    {
                        color = image.GetColor(tileX * 8 + x * 2, tileY * 8 + y);
                        pixel = GBA.Color.GetNearest(Palettes[index], color);
                        LO_nibble = (pixel == -1) ? (Byte)0x00 : (Byte)(pixel);

                        color = image.GetColor(tileX * 8 + x * 2 + 1, tileY * 8 + y);
                        pixel = GBA.Color.GetNearest(Palettes[index], color);
                        HI_nibble = (pixel == -1) ? (Byte)0x00 : (Byte)(pixel);

                        bytes[x + y * 4] = (Byte)((HI_nibble << 4) | LO_nibble);
                    }

                    tile = new Tile(bytes);

                    if (checkRedundantTiles)
                    {
                        if (tile.IsEmpty())
                        {
                            current = Tuple.Create(0, false, false);
                        }
                        else
                        {
                            current = Graphics.FindMatch(tile);

                            if (current == null)
                            {
                                current = Tuple.Create(Graphics.Count, false, false);
                                Graphics.Add(tile);
                            }
                        }
                    }
                    else
                    {
                        current = Tuple.Create(Graphics.Count, false, false);
                        Graphics.Add(tile);
                    }
                    // try {
                    Tiling[tileX, tileY] = new TSA(
                        (UInt16)current.Item1,
                        tilePalettes[tile_index],
                        current.Item2,
                        current.Item3);
                    // } catch { }

                    loading.SetPercent(60 + 40 * ((Single)tile_index / (Single)tileAmount));
                    tile_index++;
                }
            }
        }
        /// <summary>
        /// Automatically generates a TSA-compliant image from a 256(or fewer)-color image
        /// </summary>
        /// <param name="width">Width of the TSA (in tiles)</param>
        /// <param name="height">Height of the TSA (in tiles)</param>
        /// <param name="image">The bitmap to convert</param>
        /// <param name="paletteAmount">the maximum amount of 16-color palettes</param>
        /// <param name="checkRedundantTiles">whether or not to add all tiles found to the tileset</param>
        public TSA_Image(
            Int32 width, Int32 height,
            GBA.Bitmap image,
            Int32 paletteAmount,
            Boolean checkRedundantTiles)
        {
            Width = width * 8;
            Height = height * 8;

            if (image.Width != Width || image.Height != Height)
                throw new Exception("Image given has invalid dimensions.\n" +
                    "It must be " + Width + "x" + Height + " pixels.");

            if (image.Colors.Count <= 16)
            {   // No need to run TSA-ifier code
                Palettes = Palette.Split(image.Colors, paletteAmount);

                Graphics = new Tileset(new Image(image));

                Tiling = TSA_Array.GetBasicTSA(width, height);
            }
            else using (FormLoading loading = new FormLoading())
            {   // Run the image TSA-ifier
                loading.Show();

                    Int32 tileAmount = width * height;
                    Byte[] bytes = image.ToBytes();
                    Int32[] colorTotals = new Int32[image.Colors.Count];
                    Int32[,] colorAmounts = new Int32[image.Colors.Count, tileAmount];

                loading.SetLoading("Checking colors...", 2);

                    Int32 index;
                    Int32 tile = 0;
                for (Int32 tileY = 0; tileY < height; tileY++)
                for (Int32 tileX = 0; tileX < width; tileX++)
                {
                    index = tileX * 8 + tileY * 8 * Width;
                    for (Int32 y = 0; y < 8; y++)
                    {
                        for (Int32 x = 0; x < 8; x++)
                        {
                            colorTotals[bytes[index]]++;
                            colorAmounts[bytes[index], tile]++;
                        }
                        index += Width - 8;
                    }
                    tile++;
                }   // first we take a look at which colors are most present (in all, and per tile)

                loading.SetPercent(7);

                List<Int32> colors = new List<Int32>();
                for (Int32 i = 0; i < colorTotals.Length; i++)
                {
                    index = 0;
                    while (index < colors.Count && colorTotals[i] > colors[index])
                        index++;

                    if (index < colors.Count)
                        colors.Insert(index, i);
                    else colors.Add(i);

                    if (colors.Count > paletteAmount * 15)
                        colors.RemoveAt(0);
                }   // then we create a list of indices to colors with only the most used colors

                loading.SetPercent(9);

                Palettes = new Palette[paletteAmount];
                for (Int32 i = 0; i < paletteAmount; i++)
                {
                    Palettes[i] = new GBA.Palette(16);
                    Palettes[i].Add(new GBA.Color(0x0000));
                }   // we create our palettes, forcing the 1st color to be black on each palette

                loading.SetLoading("Asserting tile palettes...", 10);

                    Byte[] tilePalettes = new Byte[tileAmount];
                    Int32[] certainty = new Int32[tileAmount];
                GBA.Color color;
                    Int32 amount;
                for (Int32 i = colors.Count - 1; i >= 0; i--)
                {
                    color = image.Colors[colors[i]];
                    tile = 0;

                    while (tile < tilePalettes.Length)
                    {
                        amount = colorAmounts[colors[i], tile];

                        if (Palettes[tilePalettes[tile]].Contains(color))
                        {
                            certainty[tile] += amount;
                        }
                        else
                        {
                            if (Palettes[tilePalettes[tile]].IsFull)
                            {
                                if (certainty[tile] < amount)
                                {
                                    tilePalettes[tile]++;
                                    tilePalettes[tile] %= (Byte)Palettes.Length;
                                    certainty[tile] = 0;
                                    i++; break;
                                }
                                else certainty[tile] -= amount;
                            }
                            else if (amount != 0)
                            {
                                Palettes[tilePalettes[tile]].Add(color);
                                certainty[tile] += amount;
                            }
                        }
                        tile++;
                    }
                    loading.SetPercent(10 + (50 - 50 * ((Single)i / (Single)(colors.Count))));
                }   // and we do a loop going from  most used color to least used, setting tilePalettes and filling said palettes

                amount = 0;
                for (Int32 p = 0; p < Palettes.Length; p++)
                {
                    Palettes[p].Sort(delegate (GBA.Color first, GBA.Color second)
                    {
                        return (first.GetValueR() + first.GetValueG() + first.GetValueB())
                                - (second.GetValueR() + second.GetValueG() + second.GetValueB());
                    });
                    for (Int32 i = Palettes[p].Count; i < 16; i++)
                    {
                        Palettes[p].Add(new Color(0x0000));
                    }
                }

                loading.SetMessage("Creating TSA information...");

                Graphics = new Tileset(width * height);
                Tiling = new TSA_Array(width, height);
                    Int32 pixel;
                tile = 0;
                    Byte HI_nibble;
                    Byte LO_nibble;
                Tile currentTile;
                Tuple<Int32, Boolean, Boolean> current;
                for (Int32 tileY = 0; tileY < height; tileY++)
                for (Int32 tileX = 0; tileX < width; tileX++)
                {
                    index = (Palettes[tilePalettes[tile]].IsFull) ? tilePalettes[tile] : 0;
                    bytes = new Byte[GBA.Tile.LENGTH];

                    for (Int32 y = 0; y < 8; y++)
                    for (Int32 x = 0; x < 4; x++)
                    {
                        color = image.GetColor(tileX * 8 + x * 2, tileY * 8 + y);
                        pixel = GBA.Color.GetNearest(Palettes[index], color);
                        LO_nibble = (pixel == -1) ? (Byte)0x00 : (Byte)(pixel);

                        color = image.GetColor(tileX * 8 + x * 2 + 1, tileY * 8 + y);
                        pixel = GBA.Color.GetNearest(Palettes[index], color);
                        HI_nibble = (pixel == -1) ? (Byte)0x00 : (Byte)(pixel);

                        bytes[x + y * 4] = (Byte)((HI_nibble << 4) | LO_nibble);
                    }

                    currentTile = new Tile(bytes);

                    if (checkRedundantTiles)
                    {
                        if (currentTile.IsEmpty())
                        {
                            current = Tuple.Create(0, false, false);
                        }
                        else
                        {
                            current = Graphics.FindMatch(currentTile);

                            if (current == null)
                            {
                                current = Tuple.Create(Graphics.Count, false, false);
                                Graphics.Add(currentTile);
                            }
                        }
                    }
                    else
                    {
                        current = Tuple.Create(Graphics.Count, false, false);
                        Graphics.Add(currentTile);
                    }
                    // try {
                    Tiling[tileX, tileY] = new TSA(
                        (UInt16)current.Item1,
                        tilePalettes[tile],
                        current.Item2,
                        current.Item3);
                    // } catch { }

                    loading.SetPercent(60 + 40 * ((Single)tile / (Single)tileAmount));
                    tile++;
                }
            }
        }
    }
}
