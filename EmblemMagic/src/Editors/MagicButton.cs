using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using EmblemMagic;
using EmblemMagic.Editors;
using Magic;
using Magic.Editors;

namespace EmblemMagic
{
    public class MagicButton : Magic.Components.MagicButton
    {
        public MagicButton(App app) : base(app)
        {
            if (base.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            Image = app.Icon.ToBitmap();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Editor editor;
            switch (EditorToOpen)
            {
                case             "Text Editor": editor = new       TextEditor(App); break;
                case            "Music Editor": editor = new      MusicEditor(App); break;
                case         "Portrait Editor": editor = new   PortraitEditor(App); break;
                case       "Map Sprite Editor": editor = new  MapSpriteEditor(App); break;
                case "Battle Animation Editor": editor = new BattleAnimEditor(App); break;
                default:
                    if (EditorToOpen != null &&
                        EditorToOpen.Length > 7 &&
                        EditorToOpen.Substring(0, 7) == "Module:")
                    {
                        string module = EditorToOpen.Substring(7, EditorToOpen.Length - 7);
                        editor = new ModuleEditor(App);
                        ((ModuleEditor)editor).Core_OpenFile(Core.Path_Modules + module +".emm");
                    }
                    else editor = null;
                    break;
            }
            if (editor != null)
            {
                App.Core_OpenEditor(editor);
                switch (EditorToOpen)
                {
                    case             "Text Editor":       ((TextEditor)editor).Core_SetEntry((UInt16)EntryToSelect); break;
                    case            "Music Editor":      ((MusicEditor)editor).Core_SetEntry((byte)  EntryToSelect); break;
                    case         "Portrait Editor":   ((PortraitEditor)editor).Core_SetEntry((UInt16)EntryToSelect); break;
                    case       "Map Sprite Editor":  ((MapSpriteEditor)editor).Core_SetEntry((byte)  EntryToSelect); break;
                    case "Battle Animation Editor": ((BattleAnimEditor)editor).Core_SetEntry(        EntryToSelect); break;
                    default:                            ((ModuleEditor)editor).Core_SetEntry(        EntryToSelect); break;
                }
            }
        }
    }
}
