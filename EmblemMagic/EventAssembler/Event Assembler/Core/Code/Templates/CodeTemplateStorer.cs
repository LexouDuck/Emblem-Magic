// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Templates.CodeTemplateStorer
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using Nintenlord.Collections;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Event_Assembler.Core.IO.Input;
using Nintenlord.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nintenlord.Event_Assembler.Core.Code.Templates
{
  public class CodeTemplateStorer : ICodeTemplateStorer, IEnumerable<ICodeTemplate>, IEnumerable<KeyValuePair<Priority, ICodeTemplate>>, IEnumerable
  {
    private IDictionary<string, List<ICodeTemplate>> assemblyCodes;
    private IDictionary<KeyValuePair<Priority, int>, List<ICodeTemplate>> disassemblyCodes;
    private ICollection<ICodeTemplate> codes;
    private IComparer<ICodeTemplate> templateComparer;

    public CodeTemplateStorer(IComparer<ICodeTemplate> templateComparer)
      : this(templateComparer, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
    }

    public CodeTemplateStorer(IComparer<ICodeTemplate> templateComparer, IEqualityComparer<string> nameComparer)
    {
      this.templateComparer = templateComparer;
      this.assemblyCodes = (IDictionary<string, List<ICodeTemplate>>) new Dictionary<string, List<ICodeTemplate>>(nameComparer);
      this.disassemblyCodes = (IDictionary<KeyValuePair<Priority, int>, List<ICodeTemplate>>) new Dictionary<KeyValuePair<Priority, int>, List<ICodeTemplate>>();
      this.codes = (ICollection<ICodeTemplate>) new List<ICodeTemplate>();
    }

    public void AddCode(ICodeTemplate code, Priority priority)
    {
      this.assemblyCodes.GetOldOrSetNew<string, List<ICodeTemplate>>(code.Name).Add(code);
      this.disassemblyCodes.GetOldOrSetNew<KeyValuePair<Priority, int>, List<ICodeTemplate>>(new KeyValuePair<Priority, int>(priority, CodeTemplateStorer.GetID(code))).Add(code);
      this.codes.Add(code);
    }

    public CanCauseError<ICodeTemplate> FindTemplate(string name, Priority priority)
    {
      List<ICodeTemplate> codeTemplateList = this.assemblyCodes[name];
      foreach (KeyValuePair<KeyValuePair<Priority, int>, List<ICodeTemplate>> disassemblyCode in (IEnumerable<KeyValuePair<KeyValuePair<Priority, int>, List<ICodeTemplate>>>) this.disassemblyCodes)
      {
        if (disassemblyCode.Key.Key == priority)
        {
          foreach (ICodeTemplate result in disassemblyCode.Value)
          {
            if (codeTemplateList.Contains(result))
              return CanCauseError<ICodeTemplate>.NoError(result);
          }
        }
      }
      return CanCauseError<ICodeTemplate>.Error("No code named {0} found in priority {1}", (object) name, (object) priority);
    }

    public CanCauseError<ICodeTemplate> FindTemplate(byte[] code, int index, IEnumerable<Priority> allowedPriorities)
    {
      int num = (int) code[index] + (int) code[index + 1] * 256;
      foreach (Priority allowedPriority in allowedPriorities)
      {
        List<ICodeTemplate> codeTemplateList;
        if (this.disassemblyCodes.TryGetValue(new KeyValuePair<Priority, int>(allowedPriority, num), out codeTemplateList) ||
                    num != 0 && this.disassemblyCodes.TryGetValue(new KeyValuePair<Priority, int>(allowedPriority, 0), out codeTemplateList))
        {
            List<ICodeTemplate> collection = new List<ICodeTemplate>();
            foreach (ICodeTemplate codeTemplate in codeTemplateList)
            {
                if (codeTemplate.CanBeDisassembled && codeTemplate.Matches(code, index))
                    collection.Add(codeTemplate);
            }
            if (collection.Count > 0)
                return CanCauseError<ICodeTemplate>.NoError(collection.Max<ICodeTemplate>(this.templateComparer));
        }
      }
      return CanCauseError<ICodeTemplate>.Error("No code found.");
    }

    public CanCauseError<ICodeTemplate> FindTemplate(IInputByteStream reader, IEnumerable<Priority> allowedPriorities)
    {
      throw new NotImplementedException();
    }

    public CanCauseError<ICodeTemplate> FindTemplate(string codeName, Language.Types.Type[] parameterTypes)
    {
      List<ICodeTemplate> templates;
      if (this.assemblyCodes.TryGetValue(codeName, out templates))
        return this.GetTemplateFrom(codeName, parameterTypes, templates);
      if ((int) codeName[0] == '_')
      {
        if (this.assemblyCodes.TryGetValue(codeName.TrimStart('_'), out templates))
          return this.GetTemplateFrom(codeName, parameterTypes, templates);
      }
      return CanCauseError<ICodeTemplate>.Error("No code named {0} found.", (object) codeName);
    }

    public IEnumerable<string> GetNames()
    {
      return (IEnumerable<string>) this.assemblyCodes.Keys;
    }

    public bool IsUsedName(string name)
    {
      return this.assemblyCodes.ContainsKey(name);
    }

    public IEnumerator<ICodeTemplate> GetEnumerator()
    {
      return this.codes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    IEnumerator<KeyValuePair<Priority, ICodeTemplate>> IEnumerable<KeyValuePair<Priority, ICodeTemplate>>.GetEnumerator()
    {
      foreach (KeyValuePair<KeyValuePair<Priority, int>, List<ICodeTemplate>> disassemblyCode in (IEnumerable<KeyValuePair<KeyValuePair<Priority, int>, List<ICodeTemplate>>>) this.disassemblyCodes)
      {
        foreach (ICodeTemplate codeTemplate in disassemblyCode.Value)
          yield return new KeyValuePair<Priority, ICodeTemplate>(disassemblyCode.Key.Key, codeTemplate);
      }
    }

    private static int GetID(ICodeTemplate code)
    {
      int num = 0;
      if (code is INamed<int>)
        num = (code as INamed<int>).Name;
      return num;
    }

    private CanCauseError<ICodeTemplate> GetTemplateFrom(string codeName, Language.Types.Type[] parameterTypes, List<ICodeTemplate> templates)
    {
      IEnumerable<ICodeTemplate> codeTemplates = templates.Where<ICodeTemplate>((Func<ICodeTemplate, bool>) (template => template.Matches(parameterTypes)));
      if (codeTemplates.Any<ICodeTemplate>())
        return CanCauseError<ICodeTemplate>.NoError(codeTemplates.Min<ICodeTemplate>(this.templateComparer));
      return CanCauseError<ICodeTemplate>.Error(parameterTypes.Length != 0 ? "Incorrect parameters in code {0} {1}" : "Incorrect parameters in code {0}", (object) codeName, (object) ((IEnumerable<Language.Types.Type>) parameterTypes).ToElementWiseString<Nintenlord.Event_Assembler.Core.Code.Language.Types.Type>(" ", "", ""));
    }
  }
}
