// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.HighlightingHelper
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Utility.Primitives;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;

namespace Nintenlord.Event_Assembler.Core
{
  public static class HighlightingHelper
  {
    private static readonly string[] preprocessorDirectives = new string[12]
    {
      "Words3",
      "#ifdef",
      "#define",
      "#pool",
      "#else",
      "#endif",
      "#ifndef",
      "#include",
      "#incbin",
      "#undef",
      "#incext",
      "#easteregg"
    };
    private static readonly string[] builtInCodes = new string[8]
    {
      "CURRENTOFFSET",
      "MESSAGE",
      "ERROR",
      "WARNING",
      "ALIGN",
      "ORG",
      "PUSH",
      "POP"
    };

    public static void GetNotepadPlusPluslanguageDoc(EACodeLanguage language, string outputFile)
    {
      using (XmlWriter writer = XmlWriter.Create(outputFile, new XmlWriterSettings()
      {
        Encoding = Encoding.ASCII,
        Indent = true,
        OmitXmlDeclaration = true,
        IndentChars = "    "
      }))
      {
        writer.WriteStartElement("NotepadPlus");
        writer.WriteStartElement("UserLang");
        writer.WriteAttributeString("name", language.Name + " Event Assembly");
        writer.WriteAttributeString("ext", "event");
        writer.WriteStartElement("Settings");
        writer.WriteStartElement("Global");
        writer.WriteAttributeString("caseIgnored", "yes");
        writer.WriteEndElement();
        writer.WriteStartElement("TreatAsSymbol");
        writer.WriteAttributeString("comment", "yes");
        writer.WriteAttributeString("commentLine", "yes");
        writer.WriteEndElement();
        writer.WriteStartElement("Prefix");
        writer.WriteAttributeString("words1", "no");
        writer.WriteAttributeString("words2", "no");
        writer.WriteAttributeString("words3", "no");
        writer.WriteAttributeString("words4", "no");
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteStartElement("KeywordLists");
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Delimiters", new string[1]{ "<>" });
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Folder+", new string[1]{ "{" });
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Folder-", new string[1]{ "}" });
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Operators", "(", ")", "[", "]", "+", "-", "*", "/", "%", ">>", "<<", "&", "|", "^", ",", ";");
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Comment", "1/*", "2*/", "0//");
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Words1", language.GetCodeNames());
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Words2");
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Words3", "#ifdef", "#define", "#pool", "#else", "#endif", "#ifndef", "#include", "#incbin", "#undef");
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, "Words4");
        writer.WriteEndElement();
        writer.WriteStartElement("Styles");
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "DEFAULT", 11, Color.Black, Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "FOLDEROPEN", 12, Color.Black, Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "FOLDERCLOSE", 13, Color.Black, Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "KEYWORD1", 5, Color.FromArgb(0, 0, (int) byte.MaxValue), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "KEYWORD2", 6, Color.FromArgb(0, 128, (int) byte.MaxValue), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "KEYWORD3", 7, Color.FromArgb(0, 64, 128), Color.White, "", 1, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "KEYWORD4", 8, Color.FromArgb(64, 128, 128), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "COMMENT", 1, Color.FromArgb(0, 159, 0), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "COMMENT LINE", 2, Color.FromArgb(0, 128, 0), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "NUMBER", 4, Color.FromArgb(128, 0, 128), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "OPERATOR", 10, Color.FromArgb((int) byte.MaxValue, 0, 0), Color.White, "", 1, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "DELIMINER1", 14, Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "DELIMINER2", 15, Color.FromArgb((int) byte.MaxValue, 0, 128), Color.White, "", 0, new int?());
        HighlightingHelper.NotepadPlusPlus.WriteStyle(writer, "DELIMINER3", 16, Color.Black, Color.White, "", 0, new int?());
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
    }

    public static void GetProgrammersNotepadlanguageDoc(IEnumerable<EACodeLanguage> languages, string outputFile)
    {
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.OmitXmlDeclaration = false;
      settings.IndentChars = "  ";
      string str = "EA-language-base";
      using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
      {
        writer.WriteStartElement("Scheme");
        writer.WriteComment("Codes used by each language");
        writer.WriteStartElement("keyword-classes");
        foreach (EACodeLanguage language in languages)
        {
          writer.WriteStartElement("keyword-class");
          writer.WriteAttributeString("name", language.Name);
          writer.WriteString(language.GetCodeNames().ToElementWiseString<string>(" ", "", ""));
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteStartElement("base-language");
        writer.WriteAttributeString("name", str);
        writer.WriteStartElement("lexer");
        writer.WriteAttributeString("name", "cpp");
        writer.WriteEndElement();
        writer.WriteStartElement("property");
        writer.WriteAttributeString("name", "lexer.cpp.track.preprocessor");
        writer.WriteAttributeString("value", "0");
        writer.WriteEndElement();
        writer.WriteStartElement("use-styles");
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Default", 32, (string) null, new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Whitespace", 0, "whitespace", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Comment", 1, "commentbox", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Comment Line", 2, "commentline", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Number", 4, "number", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Keyword", 5, "keyword", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "String", 6, "string", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Character", 7, "string", new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Operator", 10, (string) null, new Color?(), new Color?(), false, true);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Identifier", 11, (string) null, new Color?(), new Color?(), false, false);
        HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "End of line string", 12, (string) null, new Color?(Color.Black), new Color?(Color.FromArgb(224, 192, 224)), true, false);
        writer.WriteEndElement();
        writer.WriteEndElement();
        foreach (EACodeLanguage language in languages)
        {
          writer.WriteStartElement("language");
          writer.WriteAttributeString("base", str);
          writer.WriteAttributeString("name", language.Name);
          writer.WriteAttributeString("title", language.Name + " Event Assembly");
          writer.WriteAttributeString("folding", "true");
          writer.WriteAttributeString("foldcomments", "true");
          writer.WriteAttributeString("foldelse", "true");
          writer.WriteAttributeString("foldcompact", "true");
          writer.WriteAttributeString("foldpreproc", "true");
          writer.WriteStartElement("lexer");
          writer.WriteAttributeString("name", "cpp");
          writer.WriteEndElement();
          writer.WriteStartElement("property");
          writer.WriteAttributeString("name", "lexer.cpp.track.preprocessor");
          writer.WriteAttributeString("value", "0");
          writer.WriteEndElement();
          writer.WriteStartElement("comments");
          writer.WriteAttributeString("line", "//");
          writer.WriteAttributeString("streamStart", "/*");
          writer.WriteAttributeString("streamEnd", "*/");
          writer.WriteEndElement();
          writer.WriteStartElement("use-keywords");
          HighlightingHelper.ProgrammersNotepad.UseKeywords(writer, 0, "Code names", language.Name);
          writer.WriteEndElement();
          writer.WriteStartElement("use-styles");
          HighlightingHelper.ProgrammersNotepad.WriteStyle(writer, "Preprocessor", 9, "preprocessor", new Color?(), new Color?(), false, false);
          writer.WriteEndElement();
          writer.WriteEndElement();
        }
        writer.WriteEndElement();
      }
    }

    private static string GetRPGString(Color fgColor)
    {
      return (fgColor.ToArgb() & 16777215).ToHexString("").PadLeft(6, '0');
    }

    private static class NotepadPlusPlus
    {
      public static void WriteKeywords(XmlWriter writer, string name)
      {
        writer.WriteStartElement("Keywords");
        writer.WriteAttributeString("name", name);
        writer.WriteEndElement();
      }

      public static void WriteKeywords(XmlWriter writer, string name, params string[] keyWords)
      {
        HighlightingHelper.NotepadPlusPlus.WriteKeywords(writer, name, (IEnumerable<string>) keyWords);
      }

      public static void WriteKeywords(XmlWriter writer, string name, IEnumerable<string> keyWords)
      {
        writer.WriteStartElement("Keywords");
        writer.WriteAttributeString("name", name);
        writer.WriteValue(keyWords.ToElementWiseString<string>(" ", "", ""));
        writer.WriteEndElement();
      }

      public static void WriteStyle(XmlWriter writer, string name, int styleID, Color fgColor, Color bgColor, string fontName, int fontStyle, int? fontSize)
      {
        writer.WriteStartElement("WordsStyle");
        writer.WriteAttributeString("name", name);
        writer.WriteAttributeString("styleID", styleID.ToString());
        writer.WriteAttributeString("fgColor", HighlightingHelper.GetRPGString(fgColor));
        writer.WriteAttributeString("bgColor", HighlightingHelper.GetRPGString(bgColor));
        writer.WriteAttributeString("fontName", fontName);
        writer.WriteAttributeString("fontStyle", fontStyle.ToString());
        if (fontSize.HasValue)
          writer.WriteAttributeString("fontSize", fontSize.Value.ToString());
        writer.WriteEndElement();
      }
    }

    private static class ProgrammersNotepad
    {
      public static void WriteStyle(XmlWriter writer, string name, int key, string className = null, Color? foreGround = null, Color? background = null, bool eolFilled = false, bool bold = false)
      {
        writer.WriteStartElement("style");
        writer.WriteAttributeString("name", name);
        writer.WriteAttributeString("key", key.ToString());
        if (className != null)
          writer.WriteAttributeString("class", className);
        if (foreGround.HasValue)
          writer.WriteAttributeString("fore", HighlightingHelper.GetRPGString(foreGround.Value));
        if (background.HasValue)
          writer.WriteAttributeString("back", HighlightingHelper.GetRPGString(background.Value));
        if (eolFilled)
          writer.WriteAttributeString("eolfilled", eolFilled.ToString().ToLower());
        if (bold)
          writer.WriteAttributeString("bold", bold.ToString().ToLower());
        writer.WriteEndElement();
      }

      public static void UseKeywords(XmlWriter writer, int key, string name, string className)
      {
        writer.WriteStartElement("keyword");
        writer.WriteAttributeString("key", key.ToString());
        writer.WriteAttributeString("name", name);
        writer.WriteAttributeString("class", className);
        writer.WriteEndElement();
      }
    }
  }
}
