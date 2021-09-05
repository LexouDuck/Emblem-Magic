using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core;
using Nintenlord.Event_Assembler.Core.Code;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Preprocessors;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Event_Assembler.Core.GBA;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.Event_Assembler.UserInterface;
using Nintenlord.Utility;
using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Parser.CharacterParsers;
using Nintenlord.Parser;
using Nintenlord.IO.Scanners;

namespace Nintenlord.Event_Assembler
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //messageLog = new GUIMessageLog();


            //var commaParser = new CharacterParser(',');
            //var leftSquareParser = new CharacterParser('[');
            //var rightSquareParser = new CharacterParser(']');

            //var atomParser = new DigitParser().Many1();

            //IParser<Char, object> vectorParser = null;
            //var lazyVector = ParserHelpers.Lazy(() => vectorParser);

            //vectorParser =
            //    (lazyVector | atomParser).
            //    SepBy1(commaParser).
            //    Between(
            //        leftSquareParser,
            //        rightSquareParser).
            //    Name("Vector");

            //var atomParam = atomParser.Transform(x => (object)x);
            //var vectorParam = vectorParser;

            //var parameterParser = (atomParam | vectorParam).Name("Parameter");

            //string text =  "[[0,1,2,3]]";

            //Match<char> match;
            //var result = parameterParser.Parse(new StringScanner(text), out match);

            //string output = match.Success ? string.Format("{0}, result {1}", match, result) : "Error";

            //messageLog.AddMessage(output);
            //messageLog.PrintAll();
        }
    }
}