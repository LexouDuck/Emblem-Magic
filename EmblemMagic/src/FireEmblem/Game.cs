using System;
using System.Text;
using Compression;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public enum GameType
    {
        Invalid = 0,
        FE6,
        FE7,
        FE8,
    }


    public abstract class Game : GBA.Game<GameType>
    {
        /// <summary>
        /// Creates a FireEmblem Game instance from the given parameters
        /// </summary>
        public static Game FromTypeAndRegion(GameType type, GameRegion region)
        {
            switch (type)
            {
                case GameType.FE6: return FE6.FromRegion(region);
                case GameType.FE7: return FE7.FromRegion(region);
                case GameType.FE8: return FE8.FromRegion(region);
            }
            return null;
        }

        /// <summary>
        /// This creates a GameType instance by checking the current ROM
        /// </summary>
        public static Game FromROM(DataManager ROM)
        {
            Byte[] id_data = Core.ReadData(new Pointer(ID_ADDRESS), (int)ID_LENGTH);
            String id = new String(Encoding.ASCII.GetChars(id_data));

            GameRegion region = GameRegion.Invalid;
            GameType   game   = GameType.Invalid;
            foreach (GameRegion r in Enum.GetValues(typeof(GameRegion)))
            {
                if (r == GameRegion.Invalid) continue;
                else if (r == GameRegion.JAP)
                if (id.Equals(FE6.FromRegion(r).ID)) { game = GameType.FE6; region = r; break; }
                if (id.Equals(FE7.FromRegion(r).ID)) { game = GameType.FE7; region = r; break; }
                if (id.Equals(FE8.FromRegion(r).ID)) { game = GameType.FE8; region = r; break; }
            }
            return Game.FromTypeAndRegion(game, region);
        }


        /*
        Address_ClassArray();               // Address at which the array of unit classes is located for this ROM
        Address_ChapterArray();             // Address at which the chapter array is located for this ROM
        Address_MapDataArray();             // Address at which the big map/event data pointer array is located for this ROM

        Address_TextArray();                // Address at which the text array is located for this ROM
        Address_HuffmanTree();              // Address at which the huffman tree begins for this ROM
        Address_HuffmanTreeRoot();          // Address at which the huffman tree ends for this ROM

        Address_Font_Menu();                // Address at which the menu font for this ROM is
        Address_Font_Bubble();              // Address at which the dialogue text bubble font for this ROM is
        Address_TextBubbleTileset();        // Address at which the text bubble graphics is for this game
        Address_TextBubblePalette();        // Address at which the text bubble palette is for this game

        Address_MusicArray();               // Address at which the array of songs is for this game

        Address_PortraitArray();            // Address at which the portrait array is located for this ROM

        Address_MapTerrainNames();          // Address at which the map tileset terrain name text index array is located in this ROM

        Address_MapSpriteIdleArray();       // Address of the first (standing) map sprite array
        Address_MapSpriteMoveArray();       // Address of the second (moving) map sprite array
        Address_MapSpritePalettes();        // Address at which the 4 palettes for map sprites is located in this ROM

        Address_DialogBackgroundArray();    // Address at which the array of backgrounds for this FE game is located
        Address_BattleBackgroundArray();    // Address at which the array of battle backgrounds for this game is located
        Address_CutsceneScreenArray();      // Address at which the array of cutscene screens for this game is located

        Address_ItemIconTileset();          // Address at which the item icon graphics are for this game
        Address_ItemIconPalette();          // Address at which the item icon palette is for this game

        Address_CharacterPaletteArray();    // Address at which the list of character in-battle palettes is
        Address_BattleAnimationArray();     // Address at which the battle animation array starts in this FE game

        Address_SpellAnimationArray();      // Address at which the spell animations start in this game

        Address_BattlePlatformArray();      // Address at which the array of platforms for the battle screen is
        Address_BattleScreenFrame();        // Addresses of the battle screen frame

        Address_WorldMap();                 // Addresses of both the small world map and large world map (and mini if FE8)

        Address_TitleScreen();              // Addresses of the graphics that make up the title screen
        */
    }
}