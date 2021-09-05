using Nintenlord.Event_Assembler.Core.Code.Language.Expression;
using Nintenlord.Event_Assembler.Core.Code.Language.Lexer;
using Nintenlord.Event_Assembler.Core.Code.Templates;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Event_Assembler.Core.IO.Logs;
using Nintenlord.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
  public sealed class EACodeLanguage
  {
    private const string offsetChanger = "ORG";
    private const string currentOffset = "CURRENTOFFSET";
    private const string messagePrinter = "MESSAGE";
    private const string errorPrinter = "ERROR";
    private const string warningPrinter = "WARNING";
    private const string alignOffset = "ALIGN";
    private const string pushOffset = "PUSH";
    private const string popOffset = "POP";
    private const string assertion = "ASSERT";
    private const string protectCode = "PROTECT";
    private string name;
    private ICodeTemplateStorer codeStorage;
    private List<string> reservedWords;
    private EAExpressionAssembler assembler;
    private EACodeLanguageDisassembler disassembler;

    internal ICodeTemplateStorer CodeStorage
    {
      get
      {
        return this.codeStorage;
      }
      set
      {
        this.codeStorage = value;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public static List<Priority> MainPriorities
    {
      get
      {
        return new List<Priority>((IEnumerable<Priority>) new Priority[2]{ Priority.main, Priority.low });
      }
    }

    public static List<Priority> UnitPriorities
    {
      get
      {
        return new List<Priority>((IEnumerable<Priority>) new Priority[2]{ Priority.unit, Priority.low });
      }
    }

    public static List<Priority> TrapPriorities
    {
      get
      {
        return new List<Priority>((IEnumerable<Priority>) new Priority[2]{ Priority.ballista, Priority.low });
      }
    }

    public static List<Priority> NormalPriorities
    {
      get
      {
        return new List<Priority>((IEnumerable<Priority>) new Priority[2]{ Priority.none, Priority.low });
      }
    }

    public EACodeLanguage(string name, IPointerMaker pointerMaker, Tuple<string, List<Priority>>[][] pointerList, ICodeTemplateStorer codeStorer, StringComparer stringComparer)
    {
      this.name = name;
      this.codeStorage = codeStorer;
      foreach (ICodeTemplate codeTemplate1 in (IEnumerable<ICodeTemplate>) codeStorer)
      {
        CodeTemplate codeTemplate2 = codeTemplate1 as CodeTemplate;
        if (codeTemplate2 != null)
          codeTemplate2.PointerMaker = pointerMaker;
      }
      this.reservedWords = new List<string>()
      {
        offsetChanger,
        alignOffset,
        currentOffset,
        messagePrinter,
        errorPrinter,
        warningPrinter,
        pushOffset,
        popOffset,
        assertion,
        protectCode
    };
      this.assembler = new EAExpressionAssembler(this.codeStorage, (IParser<Token, IExpression<int>>) null);
      this.disassembler = new EACodeLanguageDisassembler(this.codeStorage, pointerMaker, pointerList);
    }

    public void Assemble(IPositionableInputStream input, BinaryWriter output, ILog messageLog)
    {
      this.assembler.Assemble(input, output, messageLog);
    }

    public IEnumerable<string[]> Disassemble(byte[] code, int offset, int length, Priority priority, bool addEndingLinest, ILog messageLog)
    {
      return this.disassembler.Disassemble(code, offset, length, priority, messageLog, addEndingLinest);
    }

    public IEnumerable<string[]> DisassembleChapter(byte[] code, int offset, bool addEndingLinest, ILog messageLog)
    {
      return this.disassembler.DisassembleChapter(code, offset, messageLog, addEndingLinest);
    }

    public IEnumerable<string[]> DisassembleToEnd(byte[] code, int offset, Priority priority, bool addEndingLinest, ILog messageLog)
    {
      return this.disassembler.DisassembleToEnd(code, offset, priority, messageLog, addEndingLinest);
    }

    public bool IsReserved(string word)
    {
      if (this.codeStorage.IsUsedName(word))
        return true;
      foreach (string reservedWord in this.reservedWords)
      {
        if (reservedWord.Equals(word))
          return true;
      }
      return false;
    }

    public IEnumerable<string> GetCodeNames()
    {
      return this.codeStorage.GetNames().Concat<string>((IEnumerable<string>) this.reservedWords);
    }

    private bool IsValidLableName(string label)
    {
      if (!this.IsReserved(label) && label.All<char>((Func<char, bool>) (x => char.IsLetterOrDigit(x) | (int) x == 95)))
        return label.Any<char>((Func<char, bool>) (x => char.IsLetter(x)));
      return false;
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
