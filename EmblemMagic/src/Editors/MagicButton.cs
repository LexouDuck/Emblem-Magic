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
            this.Image = app.Icon.ToBitmap();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Editor editor;
            switch (this.EditorToOpen)
            {
                case             "Text Editor": editor = new       TextEditor(); break;
                case            "Music Editor": editor = new      MusicEditor(); break;
                case         "Portrait Editor": editor = new   PortraitEditor(); break;
                case       "Map Sprite Editor": editor = new  MapSpriteEditor(); break;
                case "Battle Animation Editor": editor = new BattleAnimEditor(); break;
                default:
                    if (this.EditorToOpen != null &&
                        this.EditorToOpen.Length > 7 &&
                        this.EditorToOpen.Substring(0, 7) == "Module:")
                    {
                        String module = this.EditorToOpen.Substring(7, this.EditorToOpen.Length - 7);
                        editor = new ModuleEditor();
                        ((ModuleEditor)editor).Core_OpenFile(Core.Path_Modules + module +".mmf");
                    }
                    else editor = null;
                    break;
            }
            if (editor != null)
            {
                this.App.Core_OpenEditor(editor);
                switch (this.EditorToOpen)
                {
                    case             "Text Editor":       ((TextEditor)editor).Core_SetEntry((UInt16)this.EntryToSelect); break;
                    case            "Music Editor":      ((MusicEditor)editor).Core_SetEntry((Byte)this.EntryToSelect); break;
                    case         "Portrait Editor":   ((PortraitEditor)editor).Core_SetEntry((UInt16)this.EntryToSelect); break;
                    case       "Map Sprite Editor":  ((MapSpriteEditor)editor).Core_SetEntry((Byte)this.EntryToSelect); break;
                    case "Battle Animation Editor": ((BattleAnimEditor)editor).Core_SetEntry(this.EntryToSelect); break;
                    default:                            ((ModuleEditor)editor).Core_SetEntry(this.EntryToSelect); break;
                }
            }
        }
    }
}
