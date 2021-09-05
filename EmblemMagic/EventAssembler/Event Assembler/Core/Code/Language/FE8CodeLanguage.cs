// Decompiled with JetBrains decompiler
// Type: Nintenlord.Event_Assembler.Core.Code.Language.FE8CodeLanguage
// Assembly: Core, Version=9.10.4713.28131, Culture=neutral, PublicKeyToken=null
// MVID: 65F61606-8B59-4B2D-B4B2-32AA8025E687
// Assembly location: E:\crazycolorz5\Dropbox\Unified FE Hacking\ToolBox\EA V9.12.1\Core.exe

using System;
using System.Collections.Generic;

namespace Nintenlord.Event_Assembler.Core.Code.Language
{
    using PriorityList = Tuple<string, List<Priority>>;

    public static class FE8CodeLanguage
    {
        public static readonly string Name = "FE8";
    
        public static readonly PriorityList[][] PointerList = new PriorityList[10][]
        {
            new PriorityList[1]{ new PriorityList("TurnBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("CharacterBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("LocationBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[1]{ new PriorityList("MiscBasedEvents", EACodeLanguage.MainPriorities) },
            new PriorityList[4]{ new PriorityList("Dunno1", EACodeLanguage.MainPriorities), new PriorityList("Dunno2", EACodeLanguage.MainPriorities), new PriorityList("Dunno3", EACodeLanguage.MainPriorities), new PriorityList("Tutorial", EACodeLanguage.MainPriorities) },
            new PriorityList[2]{ new PriorityList("Traps1", EACodeLanguage.TrapPriorities), new PriorityList("Traps2", EACodeLanguage.TrapPriorities) },
            new PriorityList[2]{ new PriorityList("Units1", EACodeLanguage.UnitPriorities), new PriorityList("Units2", EACodeLanguage.UnitPriorities) },
            new PriorityList[3]{ new PriorityList("SkirmishUnitsAlly1", EACodeLanguage.UnitPriorities), new PriorityList("SkirmishUnitsAlly2", EACodeLanguage.UnitPriorities), new PriorityList("SkirmishUnitsAlly3", EACodeLanguage.UnitPriorities) },
            new PriorityList[3]{ new PriorityList("SkirmishUnitsEnemy1", EACodeLanguage.UnitPriorities), new PriorityList("SkirmishUnitsEnemy2", EACodeLanguage.UnitPriorities), new PriorityList("SkirmishUnitsEnemy3", EACodeLanguage.UnitPriorities) },
            new PriorityList[2]{ new PriorityList("BeginningScene", EACodeLanguage.NormalPriorities), new PriorityList("EndingScene", EACodeLanguage.NormalPriorities) }
        };
        public static readonly string[] Types = new string[32]
        {
            "Offset",
            "Character",
            "Class",
            "Item",
            "AI",
            "MiscUnitData",
            "UnitAffiliation",
            "Frames",
            "Text",
            "TileXCoord",
            "TileYCoord",
            "Turn",
            "TurnMoment",
            "EventID",
            "ConditionalID",
            "MapChangeID",
            "ChapterID",
            "Background",
            "Cutscene",
            "Music",
            "Weather",
            "VisionRange",
            "BubbleType",
            "AmountOfMoney",
            "VillageOrMoney",
            "MenuCommand",
            "ChestData",
            "BallistaType",
            "MoveManualAction",
            "WorldMapID",
            "PixelXCoord",
            "PixelYCoord"
        };
    }
}
