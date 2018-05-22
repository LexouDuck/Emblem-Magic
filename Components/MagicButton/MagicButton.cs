using EmblemMagic.Editors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public class MagicButton : Button
    {
        public string EditorToOpen = null;
        public UInt32 EntryToSelect = 0;

        public MagicButton() : base()
        {
            Name = "";
            Text = "";
            Size = new Size(24, 24);
            MinimumSize = new Size(24, 24);
            MaximumSize = new Size(24, 24);
            if (base.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            Image = Program.Core.Icon.ToBitmap();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Editor editor;
            switch (EditorToOpen)
            {
                case             "Text Editor": editor = new TextEditor();       break;
                case            "Music Editor": editor = new MusicEditor();      break;
                case         "Portrait Editor": editor = new PortraitEditor();   break;
                case       "Map Sprite Editor": editor = new MapSpriteEditor();  break;
                case "Battle Animation Editor": editor = new BattleAnimEditor(); break;
                default:
                    if (EditorToOpen != null &&
                        EditorToOpen.Length > 7 &&
                        EditorToOpen.Substring(0, 7) == "Module:")
                    {
                        string module = EditorToOpen.Substring(7, EditorToOpen.Length - 7);
                        editor = new ModuleEditor();
                        ((ModuleEditor)editor).Core_OpenFile(Core.Path_Modules + module +".emm");
                    }
                    else editor = null; break;
            }
            if (editor != null)
            {
                Program.Core.Core_OpenEditor(editor);
                switch (EditorToOpen)
                {
                    case             "Text Editor":       ((TextEditor)editor).Core_SetEntry((UInt16)EntryToSelect); break;
                    case            "Music Editor":      ((MusicEditor)editor).Core_SetEntry((byte)EntryToSelect);   break;
                    case         "Portrait Editor":   ((PortraitEditor)editor).Core_SetEntry((UInt16)EntryToSelect); break;
                    case       "Map Sprite Editor":  ((MapSpriteEditor)editor).Core_SetEntry((byte)EntryToSelect);   break;
                    case "Battle Animation Editor": ((BattleAnimEditor)editor).Core_SetEntry(EntryToSelect);         break;
                    default:                            ((ModuleEditor)editor).Core_SetEntry(EntryToSelect); break;
                }
            }
        }
    }
}
