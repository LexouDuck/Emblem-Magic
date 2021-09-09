using System.Collections.Generic;
using System.Drawing;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class Font : IDisplayable
    {
        public System.Int32 this[System.Int32 x, System.Int32 y]
        {
            get
            {
                System.Int32 font_i = (x / 16) + (y / 16) * 16;
                if (Glyphs[font_i] == null)
                    return 0;
                else return Glyphs[font_i][x % 16, y % 16];
            }
        }
        public GBA.Color GetColor(System.Int32 x, System.Int32 y)
        {
            System.Int32 font_i = (x / 16) + (y / 16) * 16;
            if (Glyphs[font_i] == null) return Glyph.Colors[0];
            else return Glyphs[font_i].GetColor(x % 16, y % 16);
        }

        public System.Int32 Width
        {
            get
            {
                return 256;
            }
        }
        public System.Int32 Height
        {
            get
            {
                return (Core.App.Game.Region == GameRegion.JAP ? 512 : 256);
            }
        }

        /// <summary>
        /// The address at which this font array is located
        /// </summary>
        public Pointer Address { get; }
        /// <summary>
        /// The glyphs that make up this font
        /// </summary>
        public Glyph[] Glyphs { get; }



        public Font(Pointer address)
        {
            Address = address;
            Glyphs = new Glyph[(Core.App.Game.Region == GameRegion.JAP ? 512 : 256)];
            Pointer pointer;
            for (System.Int32 i = 0; i < Glyphs.Length; i++)
            {
                if (Core.App.Game.Region == GameRegion.JAP)
                {
                    // normally you would compare the first byte of the Shift-JIS char to the 5th byte of the glyph; 
                    // if not equal, use the pointer which directs us to the next glyph (linked list)
                    try
                    {
                        pointer = Core.ReadPointer(address + i * 4);
                    }
                    catch { continue; }
                }
                else pointer = Core.ReadPointer(address + i * 4);
                Glyphs[i] = (pointer == new Pointer()) ? null : new Glyph(pointer);
            }
        }
        public Font(GBA.Bitmap image)
        {
            GBA.Color[] colors = new GBA.Color[4]
            {
                image.Colors[0],
                image.Colors[1],
                image.Colors[2],
                image.Colors[3]
            };
            Glyphs = new Glyph[256];
            System.Int32 x = 1;
            System.Int32 y = 0;
            for (System.Int32 i = 1; i < Glyphs.Length; i++)
            {
                Glyphs[i] = new Glyph(new GBA.Bitmap(image,
                    new Rectangle(x * 16, y * 16, 16, 16)), colors);

                if (Glyphs[i].IsEmpty())
                    Glyphs[i] = null;
                /*
                for (int j = 0; j < i; j++)
                {
                    if (Glyphs[i].Equals(Glyphs[j]))
                        Glyphs[i] = Glyphs[j];
                }
                */
                x++;
                if (x % 16 == 0)
                {
                    x = 0;
                    y++;
                }
            }
        }



        public Glyph GetGlyph(System.Char letter, Dictionary<System.Char, System.Byte> fontmap)
        {
            System.Byte index = 0x00;
            if (fontmap.TryGetValue(letter, out index))
                return Glyphs[index];
            else return null;
        }

        public static Dictionary<System.Char, System.Byte> GetFontMap()
        {
            Dictionary<System.Char, System.Byte> result = new Dictionary<System.Char, System.Byte>();

            result.Add(' ', 0x20);
            result.Add('!', 0x21);
            result.Add('"', 0x22);
            result.Add('#', 0x23);
            result.Add('$', 0x24);
            result.Add('%', 0x25);
            result.Add('&', 0x26);
            result.Add('\'',0x27);
            result.Add('(', 0x28);
            result.Add(')', 0x29);
            result.Add('*', 0x2A);
            result.Add('+', 0x2B);
            result.Add(',', 0x2C);
            result.Add('-', 0x2D);
            result.Add('.', 0x2E);
            result.Add('/', 0x2F);

            result.Add('0', 0x30);
            result.Add('1', 0x31);
            result.Add('2', 0x32);
            result.Add('3', 0x33);
            result.Add('4', 0x34);
            result.Add('5', 0x35);
            result.Add('6', 0x36);
            result.Add('7', 0x37);
            result.Add('8', 0x38);
            result.Add('9', 0x39);
            result.Add(':', 0x3A);
            result.Add(';', 0x3B);
            result.Add('<', 0x3C);
            result.Add('=', 0x3D);
            result.Add('>', 0x3E);
            result.Add('?', 0x3F);

            result.Add('@', 0x40);
            result.Add('A', 0x41);
            result.Add('B', 0x42);
            result.Add('C', 0x43);
            result.Add('D', 0x44);
            result.Add('E', 0x45);
            result.Add('F', 0x46);
            result.Add('G', 0x47);
            result.Add('H', 0x48);
            result.Add('I', 0x49);
            result.Add('J', 0x4A);
            result.Add('K', 0x4B);
            result.Add('L', 0x4C);
            result.Add('M', 0x4D);
            result.Add('N', 0x4E);
            result.Add('O', 0x4F);
            result.Add('P', 0x50);
            result.Add('Q', 0x51);
            result.Add('R', 0x52);
            result.Add('S', 0x53);
            result.Add('T', 0x54);
            result.Add('U', 0x55);
            result.Add('V', 0x56);
            result.Add('W', 0x57);
            result.Add('X', 0x58);
            result.Add('Y', 0x59);
            result.Add('Z', 0x5A);
            result.Add('[', 0x5B);
            result.Add('¥', 0x5C);
            result.Add(']', 0x5D);
            result.Add('^', 0x5E);
            result.Add('_', 0x5F);
                
            result.Add('`', 0x60);
            result.Add('a', 0x61);
            result.Add('b', 0x62);
            result.Add('c', 0x63);
            result.Add('d', 0x64);
            result.Add('e', 0x65);
            result.Add('f', 0x66);
            result.Add('g', 0x67);
            result.Add('h', 0x68);
            result.Add('i', 0x69);
            result.Add('j', 0x6A);
            result.Add('k', 0x6B);
            result.Add('l', 0x6C);
            result.Add('m', 0x6D);
            result.Add('n', 0x6E);
            result.Add('o', 0x6F);
            result.Add('p', 0x70);
            result.Add('q', 0x71);
            result.Add('r', 0x72);
            result.Add('s', 0x73);
            result.Add('t', 0x74);
            result.Add('u', 0x75);
            result.Add('v', 0x76);
            result.Add('w', 0x77);
            result.Add('x', 0x78);
            result.Add('y', 0x79);
            result.Add('z', 0x7A);
            result.Add('{', 0x7B);
            result.Add('|', 0x7C);
            result.Add('}', 0x7D);
            result.Add('~', 0x7E);
            result.Add('—', 0x7F);

            result.Add('Œ', 0x8C);
            result.Add('œ', 0x9C);

            result.Add('‘', 0x91);
            result.Add('’', 0x92);
            result.Add('“', 0x93);
            result.Add('”', 0x94);

            result.Add('¡', 0xA1); // positioned so theyre at +0x80 from regular ! and ?
            result.Add('¿', 0xBF);

            result.Add('¤', 0xAA);
            result.Add('°', 0xBA);
            result.Add('«', 0xAB);
            result.Add('»', 0xBB);



            result.Add('À', 0xC0);
            result.Add('Á', 0xC1);
            result.Add('Â', 0xC2);
            //
            result.Add('Ä', 0xC4);
            //
            //
            result.Add('Ç', 0xC7);
            result.Add('È', 0xC8);
            result.Add('É', 0xC9);
            result.Add('Ê', 0xCA);
            result.Add('Ë', 0xCB);
            result.Add('Ì', 0xCC);
            result.Add('Í', 0xCD);
            result.Add('Î', 0xCE);
            result.Add('Ï', 0xCF);
            //
            result.Add('Ñ', 0xD1);
            result.Add('Ò', 0xD2);
            result.Add('Ó', 0xD3);
            result.Add('Ô', 0xD4);
            //
            result.Add('Ö', 0xD6);
            //
            //
            result.Add('Ù', 0xD9);
            result.Add('Ú', 0xDA);
            result.Add('Û', 0xDB);
            result.Add('Ü', 0xDC);
            //
            //
            result.Add('β', 0xDF);
                


            result.Add('à', 0xE0);
            result.Add('á', 0xE1);
            result.Add('â', 0xE2);
            //
            result.Add('ä', 0xE4);
            //
            //
            result.Add('ç', 0xE7);
            result.Add('è', 0xE8);
            result.Add('é', 0xE9);
            result.Add('ê', 0xEA);
            result.Add('ë', 0xEB);
            result.Add('ì', 0xEC);
            result.Add('í', 0xED);
            result.Add('î', 0xEE);
            result.Add('ï', 0xEF);
            //
            result.Add('ñ', 0xF1);
            result.Add('ò', 0xF2);
            result.Add('ó', 0xF3);
            result.Add('ô', 0xF4);
            //
            result.Add('ö', 0xF6);
            //
            //
            result.Add('ù', 0xF9);
            result.Add('ú', 0xFA);
            result.Add('û', 0xFB);
            result.Add('ü', 0xFC);
            //
            //
            //

            return result;
        }
        public static Dictionary<System.Char, System.Byte> GetFontMap(System.Boolean textBubbleFont)
        {
            Dictionary<System.Char, System.Byte> result = new Dictionary<System.Char, System.Byte>();

            if (textBubbleFont)
            {
                result.Add('0', 0x00); // NUMBER 0
                result.Add('1', 0x01); // NUMBER 1 
                result.Add('2', 0x02); // NUMBER 2
                result.Add('3', 0x03); // NUMBER 3
                result.Add('4', 0x04); // NUMBER 4
                result.Add('5', 0x05); // NUMBER 5
                result.Add('6', 0x06); // NUMBER 6
                result.Add('7', 0x07); // NUMBER 7
                result.Add('8', 0x08); // NUMBER 8
                result.Add('9', 0x09); // NUMBER 9
                result.Add('-', 0x14); // MINUS
                result.Add('+', 0x15); // PLUS
                result.Add('/', 0x16); // SLASH
                result.Add('~', 0x17); // TILDE
                result.Add('S', 0x18); // LETTER S
                result.Add('A', 0x19); // LETTER A
                result.Add('B', 0x1A); // LETTER B
                result.Add('C', 0x1B); // LETTER C
                result.Add('D', 0x1C); // LETTER D
                result.Add('E', 0x1D); // LETTER E
                result.Add('G', 0x1E); // LETTER G
                result.Add('e', 0x1F); // SMALL LETTER E
                result.Add(':', 0x20); // COLON
                result.Add('.', 0x21); // DOT
                result.Add('H', 0x22); // LETTER H
                result.Add('P', 0x23); // LETTER P
                result.Add('L', 0x24); // LETTER L
                result.Add('V', 0x25); // LETTER V
                result.Add('>', 0x26); // ARROW
                result.Add('_', 0x27); // BLACK BOX CHAR
                result.Add('[', 0x28); // 100 (first half)
                result.Add(']', 0x29); // 100 (second half)
                result.Add('%', 0x2A); // PERCENT SYMBOL
                result.Add('ァ', 0x2B); // KATAKANA LETTER SMALL A
                result.Add('ア', 0x2C); // KATAKANA LETTER A
                result.Add('ィ', 0x2D); // KATAKANA LETTER SMALL I
                result.Add('イ', 0x2E); // KATAKANA LETTER I
                result.Add('ゥ', 0x2F); // KATAKANA LETTER SMALL U
                result.Add('ウ', 0x30); // KATAKANA LETTER U
                result.Add('ェ', 0x31); // KATAKANA LETTER SMALL E
                result.Add('エ', 0x32); // KATAKANA LETTER E
                result.Add('ォ', 0x33); // KATAKANA LETTER SMALL O
                result.Add('オ', 0x34); // KATAKANA LETTER O
                result.Add('カ', 0x35); // KATAKANA LETTER KA
                result.Add('ガ', 0x36); // KATAKANA LETTER GA
                result.Add('キ', 0x37); // KATAKANA LETTER KI
                result.Add('ギ', 0x38); // KATAKANA LETTER GI
                result.Add('ク', 0x39); // KATAKANA LETTER KU
                result.Add('グ', 0x3A); // KATAKANA LETTER GU
                result.Add('ケ', 0x3B); // KATAKANA LETTER KE
                result.Add('ゲ', 0x3C); // KATAKANA LETTER GE
                result.Add('コ', 0x3D); // KATAKANA LETTER KO
                result.Add('ゴ', 0x3E); // KATAKANA LETTER GO
                result.Add('サ', 0x3F); // KATAKANA LETTER SA
                result.Add('ザ', 0x40); // KATAKANA LETTER ZA
                result.Add('シ', 0x41); // KATAKANA LETTER SI
                result.Add('ジ', 0x42); // KATAKANA LETTER ZI
                result.Add('ス', 0x43); // KATAKANA LETTER SU
                result.Add('ズ', 0x44); // KATAKANA LETTER ZU
                result.Add('セ', 0x45); // KATAKANA LETTER SE
                result.Add('ゼ', 0x46); // KATAKANA LETTER ZE
                result.Add('ソ', 0x47); // KATAKANA LETTER SO
                result.Add('ゾ', 0x48); // KATAKANA LETTER ZO
                result.Add('タ', 0x49); // KATAKANA LETTER TA
                result.Add('ダ', 0x4A); // KATAKANA LETTER DA
                result.Add('チ', 0x4B); // KATAKANA LETTER TI
                result.Add('ヂ', 0x4C); // KATAKANA LETTER DI
                result.Add('ッ', 0x4D); // KATAKANA LETTER SMALL T
                result.Add('ツ', 0x4E); // KATAKANA LETTER TU
                result.Add('ヅ', 0x4F); // KATAKANA LETTER DU
                result.Add('テ', 0x50); // KATAKANA LETTER TE
                result.Add('デ', 0x51); // KATAKANA LETTER DE
                result.Add('ト', 0x52); // KATAKANA LETTER TO
                result.Add('ド', 0x53); // KATAKANA LETTER DO
                result.Add('ナ', 0x54); // KATAKANA LETTER NA
                result.Add('ニ', 0x55); // KATAKANA LETTER NI
                result.Add('ヌ', 0x56); // KATAKANA LETTER NU
                result.Add('ネ', 0x57); // KATAKANA LETTER NE
                result.Add('ノ', 0x58); // KATAKANA LETTER NO
                result.Add('ハ', 0x59); // KATAKANA LETTER HA
                result.Add('バ', 0x5A); // KATAKANA LETTER BA
                result.Add('パ', 0x5B); // KATAKANA LETTER PA
                result.Add('ヒ', 0x5C); // KATAKANA LETTER HI
                result.Add('ビ', 0x5D); // KATAKANA LETTER BI
                result.Add('ピ', 0x5E); // KATAKANA LETTER PI
                result.Add('フ', 0x5F); // KATAKANA LETTER HU
                result.Add('ブ', 0x60); // KATAKANA LETTER BU
                result.Add('プ', 0x61); // KATAKANA LETTER PU
                result.Add('ヘ', 0x62); // KATAKANA LETTER HE
                result.Add('ベ', 0x63); // KATAKANA LETTER BE
                result.Add('ペ', 0x64); // KATAKANA LETTER PE
                result.Add('ホ', 0x65); // KATAKANA LETTER HO
                result.Add('ボ', 0x66); // KATAKANA LETTER BO
                result.Add('ポ', 0x67); // KATAKANA LETTER PO
                result.Add('マ', 0x68); // KATAKANA LETTER MA
                result.Add('ミ', 0x69); // KATAKANA LETTER MI
                result.Add(' ', 0x6A); // space ?
                result.Add('ム', 0x6B); // KATAKANA LETTER MU
                result.Add('メ', 0x6C); // KATAKANA LETTER ME
                result.Add('モ', 0x6D); // KATAKANA LETTER MO
                result.Add('ャ', 0x6E); // KATAKANA LETTER SMALL YA
                result.Add('ヤ', 0x6F); // KATAKANA LETTER YA
                result.Add('ュ', 0x70); // KATAKANA LETTER SMALL YU
                result.Add('ユ', 0x71); // KATAKANA LETTER YU
                result.Add('ョ', 0x72); // KATAKANA LETTER SMALL YO
                result.Add('ヨ', 0x73); // KATAKANA LETTER YO
                result.Add('ラ', 0x74); // KATAKANA LETTER RA
                result.Add('リ', 0x75); // KATAKANA LETTER RI
                result.Add('ル', 0x76); // KATAKANA LETTER RU
                result.Add('レ', 0x77); // KATAKANA LETTER RE
                result.Add('ロ', 0x78); // KATAKANA LETTER RO
                result.Add('n', 0x79); // SMALL WA ?
                result.Add('ワ', 0x7A); // KATAKANA LETTER WA
                result.Add('p', 0x7B); // WI ?
                result.Add('q', 0x7C); // WE ?
                result.Add('r', 0x7D); // WO ?
                result.Add('ン', 0x7E); // KATAKANA LETTER N
                result.Add('ヴ', 0x7F); // KATAKANA LETTER VU
                result.Add('u', 0x80); // SMALL KA ?
                result.Add('v', 0x81); // SMALL KE ?
                result.Add('w', 0x82); // VA ?
                result.Add('x', 0x83); // VI ?
                result.Add('y', 0x84); // VE ?
                result.Add('z', 0x85); // VO ?
                result.Add('・', 0x86); // MIDDLE DOT ? or space ?
                result.Add('復', 0x87); // KANJI ??? (RESUME)
                result.Add('持', 0x88); // KANJI ??? (HAVE)
                result.Add('込', 0x89); // KANJI ??? (VARIOUS)
                result.Add('ぁ', 0x8A); // HIRAGANA SMALL A
                result.Add('あ', 0x8B); // HIRAGANA A
                result.Add('ぃ', 0x8C); // HIRAGANA SMALL I
                result.Add('い', 0x8D); // HIRAGANA I
                result.Add('ぅ', 0x8E); // HIRAGANA SMALL U
                result.Add('う', 0x8F); // HIRAGANA U
                result.Add('ぇ', 0x90); // HIRAGANA SMALL E
                result.Add('え', 0x91); // HIRAGANA E
                result.Add('ぉ', 0x92); // HIRAGANA SMALL O
                result.Add('お', 0x93); // HIRAGANA O
                result.Add('か', 0x94); // HIRAGANA KA
                result.Add('が', 0x95); // HIRAGANA GA
                result.Add('き', 0x96); // HIRAGANA KI
                result.Add('ぎ', 0x97); // HIRAGANA GI
                result.Add('く', 0x98); // HIRAGANA KU
                result.Add('ぐ', 0x99); // HIRAGANA GU
                result.Add('け', 0x9A); // HIRAGANA KE
                result.Add('げ', 0x9B); // HIRAGANA GE
                result.Add('こ', 0x9C); // HIRAGANA KO
                result.Add('ご', 0x9D); // HIRAGANA GO
                result.Add('さ', 0x9E); // HIRAGANA SA
                result.Add('ざ', 0x9F); // HIRAGANA ZA
                result.Add('し', 0xA0); // HIRAGANA SHI
                result.Add('じ', 0xA1); // HIRAGANA JI
                result.Add('す', 0xA2); // HIRAGANA SU
                result.Add('ず', 0xA3); // HIRAGANA ZU
                result.Add('せ', 0xA4); // HIRAGANA SE
                result.Add('ぜ', 0xA5); // HIRAGANA ZE
                result.Add('そ', 0xA6); // HIRAGANA SO
                result.Add('ぞ', 0xA7); // HIRAGANA ZO
                result.Add('た', 0xA8); // HIRAGANA TA
                result.Add('だ', 0xA9); // HIRAGANA DA
                result.Add('ち', 0xAA); // HIRAGANA CHI
                result.Add('ぢ', 0xAB); // HIRAGANA DJI
                result.Add('っ', 0xAC); // HIRAGANA SMALL TSU
                result.Add('つ', 0xAD); // HIRAGANA TSU
                result.Add('づ', 0xAE); // HIRAGANA DZU
                result.Add('て', 0xAF); // HIRAGANA TE
                result.Add('で', 0xB0); // HIRAGANA DE
                result.Add('と', 0xB1); // HIRAGANA TO
                result.Add('ど', 0xB2); // HIRAGANA DO
                result.Add('な', 0xB3); // HIRAGANA NA
                result.Add('に', 0xB4); // HIRAGANA NI
                result.Add('ぬ', 0xB5); // HIRAGANA NU
                result.Add('ね', 0xB6); // HIRAGANA NE
                result.Add('の', 0xB7); // HIRAGANA NO
                result.Add('は', 0xB8); // HIRAGANA HA
                result.Add('ば', 0xB9); // HIRAGANA BA
                result.Add('ぱ', 0xBA); // HIRAGANA PA
                result.Add('ひ', 0xBB); // HIRAGANA HI
                result.Add('び', 0xBC); // HIRAGANA BI
                result.Add('ぴ', 0xBD); // HIRAGANA PI
                result.Add('ふ', 0xBE); // HIRAGANA FU
                result.Add('ぶ', 0xBF); // HIRAGANA BU
                result.Add('ぷ', 0xC0); // HIRAGANA PU
                result.Add('へ', 0xC1); // HIRAGANA HE
                result.Add('べ', 0xC2); // HIRAGANA BE
                result.Add('ぺ', 0xC3); // HIRAGANA PE
                result.Add('ほ', 0xC4); // HIRAGANA HO
                result.Add('ぼ', 0xC5); // HIRAGANA BO
                result.Add('ぽ', 0xC6); // HIRAGANA PO
                result.Add('ま', 0xC7); // HIRAGANA MA
                result.Add('み', 0xC8); // HIRAGANA MI
                result.Add('む', 0xC9); // HIRAGANA MU
                result.Add('め', 0xCA); // HIRAGANA ME
                result.Add('も', 0xCB); // HIRAGANA MO
                result.Add('ゃ', 0xCC); // HIRAGANA SMALL YA
                result.Add('や', 0xCD); // HIRAGANA YA
                result.Add('ゅ', 0xCE); // HIRAGANA SMALL YU
                result.Add('ゆ', 0xCF); // HIRAGANA YU
                result.Add('ょ', 0xD0); // HIRAGANA SMALL YO
                result.Add('よ', 0xD1); // HIRAGANA YO
                result.Add('ら', 0xD2); // HIRAGANA RA
                result.Add('り', 0xD3); // HIRAGANA RI
                result.Add('る', 0xD4); // HIRAGANA RU
                result.Add('れ', 0xD5); // HIRAGANA RE
                result.Add('ろ', 0xD6); // HIRAGANA RO
                result.Add('ゎ', 0xD7); // HIRAGANA SMALL WA
                result.Add('わ', 0xD8); // HIRAGANA WA
                result.Add('ゐ', 0xD9); // ??? (kanji in font)
                result.Add('ゑ', 0xDA); // ??? (kanji in font) 
                result.Add('を', 0xDB); // HIRAGANA WO
                result.Add('ん', 0xDC); // HIRAGANA N
            }
            else
            {
                result.Add('ァ', 0x00); // KATAKANA SMALL A
                result.Add('ア', 0x01); // KATAKANA A
                result.Add('ィ', 0x02); // KATAKANA SMALL I
                result.Add('イ', 0x03); // KATAKANA I
                result.Add('ゥ', 0x04); // KATAKANA SMALL U
                result.Add('ウ', 0x05); // KATAKANA U
                result.Add('ェ', 0x06); // KATAKANA SMALL E
                result.Add('エ', 0x07); // KATAKANA E
                result.Add('ォ', 0x08); // KATAKANA SMALL O
                result.Add('オ', 0x09); // KATAKANA O
                result.Add('カ', 0x0A); // KATAKANA KA
                result.Add('ガ', 0x0B); // KATAKANA GA
                result.Add('キ', 0x0C); // KATAKANA KI
                result.Add('ギ', 0x0D); // KATAKANA GI
                result.Add('ク', 0x0E); // KATAKANA KU
                result.Add('グ', 0x0F); // KATAKANA GU
                result.Add('ケ', 0x10); // KATAKANA KE
                result.Add('ゲ', 0x11); // KATAKANA GE
                result.Add('コ', 0x12); // KATAKANA KO
                result.Add('ゴ', 0x13); // KATAKANA GO
                result.Add('サ', 0x14); // KATAKANA SA
                result.Add('ザ', 0x15); // KATAKANA ZA
                result.Add('シ', 0x16); // KATAKANA SI
                result.Add('ジ', 0x17); // KATAKANA ZI
                result.Add('ス', 0x18); // KATAKANA SU
                result.Add('ズ', 0x19); // KATAKANA ZU
                result.Add('セ', 0x1A); // KATAKANA SE
                result.Add('ゼ', 0x1B); // KATAKANA ZE
                result.Add('ソ', 0x1C); // KATAKANA SO
                result.Add('ゾ', 0x1D); // KATAKANA ZO
                result.Add('タ', 0x1E); // KATAKANA TA
                result.Add('ダ', 0x1F); // KATAKANA DA
                result.Add('チ', 0x20); // KATAKANA TI
                result.Add('ヂ', 0x21); // KATAKANA DI
                result.Add('ッ', 0x22); // KATAKANA SMALL TU
                result.Add('ツ', 0x23); // KATAKANA TU
                result.Add('ヅ', 0x24); // KATAKANA DU
                result.Add('テ', 0x25); // KATAKANA TE
                result.Add('デ', 0x26); // KATAKANA DE
                result.Add('ト', 0x27); // KATAKANA TO
                result.Add('ド', 0x28); // KATAKANA DO
                result.Add('ナ', 0x29); // KATAKANA NA
                result.Add('ニ', 0x2A); // KATAKANA NI
                result.Add('ヌ', 0x2B); // KATAKANA NU
                result.Add('ネ', 0x2C); // KATAKANA NE
                result.Add('ノ', 0x2D); // KATAKANA NO
                result.Add('ハ', 0x2E); // KATAKANA HA
                result.Add('バ', 0x2F); // KATAKANA BA
                result.Add('パ', 0x30); // KATAKANA PA
                result.Add('ヒ', 0x31); // KATAKANA HI
                result.Add('ビ', 0x32); // KATAKANA BI
                result.Add('ピ', 0x33); // KATAKANA PI
                result.Add('フ', 0x34); // KATAKANA HU
                result.Add('ブ', 0x35); // KATAKANA BU
                result.Add('プ', 0x36); // KATAKANA PU
                result.Add('ヘ', 0x37); // KATAKANA HE
                result.Add('ベ', 0x38); // KATAKANA BE
                result.Add('ペ', 0x39); // KATAKANA PE
                result.Add('ホ', 0x3A); // KATAKANA HO
                result.Add('ボ', 0x3B); // KATAKANA BO
                result.Add('ポ', 0x3C); // KATAKANA PO
                result.Add('マ', 0x3D); // KATAKANA MA
                result.Add('ミ', 0x3E); // KATAKANA MI
                result.Add(' ', 0x3F); // space ?
                result.Add('ム', 0x40); // KATAKANA MU
                result.Add('メ', 0x41); // KATAKANA ME
                result.Add('モ', 0x42); // KATAKANA MO
                result.Add('ャ', 0x43); // KATAKANA SMALL YA
                result.Add('ヤ', 0x44); // KATAKANA YA
                result.Add('ュ', 0x45); // KATAKANA SMALL YU
                result.Add('ユ', 0x46); // KATAKANA YU
                result.Add('ョ', 0x47); // KATAKANA SMALL YO
                result.Add('ヨ', 0x48); // KATAKANA YO
                result.Add('ラ', 0x49); // KATAKANA RA
                result.Add('リ', 0x4A); // KATAKANA RI
                result.Add('ル', 0x4B); // KATAKANA RU
                result.Add('レ', 0x4C); // KATAKANA RE
                result.Add('ロ', 0x4D); // KATAKANA RO
                result.Add('N', 0x4E); // SMALL WA ?
                result.Add('ワ', 0x4F); // KATAKANA WA
                result.Add('P', 0x50); // WI ?
                result.Add('Q', 0x51); // WE ?
                result.Add('R', 0x52); // WO ?
                result.Add('ン', 0x53); // KATAKANA N
                result.Add('ヴ', 0x54); // KATAKANA VU
                result.Add('U', 0x55); // SMALL KA ?
                result.Add('V', 0x56); // SMALL KE ?
                result.Add('W', 0x57); // VA ?
                result.Add('X', 0x58); // VI ?
                result.Add('Y', 0x59); // VE ?
                result.Add('Z', 0x5A); // VO ?
                result.Add('・', 0x5B); // KATAKANA MIDDLE DOT
            }

            return result;
        }
    }
}