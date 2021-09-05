// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.FE6CodeLanguage
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
    using PriorityList = Tuple<string, List<Priority>>;

    public static class FE6CodeLanguage
    {
        public static readonly string Name = "FE6";

        public static readonly PriorityList[][] PointerList = new PriorityList[6][]
        {
            new PriorityList[1]{ new PriorityList("TurnBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("CharacterBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("LocationBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("MiscBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[2]{ new PriorityList("EnemyUnits", EACodeLanguage.UnitPriorities), new PriorityList("AllyUnits", EACodeLanguage.UnitPriorities) },
            new PriorityList[1]{ new PriorityList("EndingScene", EACodeLanguage.NormalPriorities) }
        };
    }
}
