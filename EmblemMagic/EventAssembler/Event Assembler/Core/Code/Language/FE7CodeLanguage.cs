// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.FE7CodeLanguage
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
    using PriorityList = Tuple<string, List<Priority>>;

    public static class FE7CodeLanguage
    {
        public static readonly string Name = "FE7";

        public static readonly PriorityList[][] PointerList = new PriorityList[8][]
        {
            new PriorityList[1]{ new PriorityList("TurnBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("CharacterBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("LocationBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("MiscBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[2]{ new PriorityList("TrapsEliwoodMode", EACodeLanguage.TrapPriorities), new PriorityList("TrapsHectorMode", EACodeLanguage.TrapPriorities) },
            new PriorityList[4]{ new PriorityList("EnemyUnitsENM", EACodeLanguage.UnitPriorities), new PriorityList("EnemyUnitsEHM", EACodeLanguage.UnitPriorities), new PriorityList("EnemyUnitsHNM", EACodeLanguage.UnitPriorities), new PriorityList("EnemyUnitsHHM", EACodeLanguage.UnitPriorities) },
            new PriorityList[4]{ new PriorityList("AllyUnitsENM", EACodeLanguage.UnitPriorities), new PriorityList("AllyUnitsEHM", EACodeLanguage.UnitPriorities), new PriorityList("AllyUnitsHNM", EACodeLanguage.UnitPriorities), new PriorityList("AllyUnitsHHM", EACodeLanguage.UnitPriorities) },
            new PriorityList[2]{ new PriorityList("BeginningScene", EACodeLanguage.NormalPriorities), new PriorityList("EndingScene", EACodeLanguage.NormalPriorities) }
        };

        private static readonly PriorityList[][] TutorialPointerList = new PriorityList[12][]
        {
            new PriorityList[4]{ new PriorityList("PrologueTutorial1", EACodeLanguage.MainPriorities), new PriorityList("PrologueTutorial2", EACodeLanguage.MainPriorities), new PriorityList("PrologueTutorial3", EACodeLanguage.MainPriorities), new PriorityList("PrologueTutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch1Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch1Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch1Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch1Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch2Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch2Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch2Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch2Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch3Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch3Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch3Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch3Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch4Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch4Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch4Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch4Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch5Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch5Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch5Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch5Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch6Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch6Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch6Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch6Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch7Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch7Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch7Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch7Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch7xTutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch7xTutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch7xTutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch7xTutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch8Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch8Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch8Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch8Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch9Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch9Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch9Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch9Tutorial4", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Ch10Tutorial1", EACodeLanguage.MainPriorities), new PriorityList("Ch10Tutorial2", EACodeLanguage.MainPriorities), new PriorityList("Ch10Tutorial3", EACodeLanguage.MainPriorities), new PriorityList("Ch10Tutorial4", EACodeLanguage.MainPriorities) }
        };
    }
}
