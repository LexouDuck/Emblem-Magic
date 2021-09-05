using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nintenlord.Forms
{
    public class ToolStripsDisabler : IDisposable
    {
        Dictionary<ToolStripItem, bool> oldStates;

        public ToolStripsDisabler(IEnumerable<ToolStripItem> toolStripts)
        {
            oldStates = toolStripts.ToDictionary(x => x, x=> x.Enabled);
            foreach (var item in toolStripts)
            {
                item.Enabled = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (var item in oldStates)
            {
                item.Key.Enabled = item.Value;
            }
        }

        #endregion
    }
}
