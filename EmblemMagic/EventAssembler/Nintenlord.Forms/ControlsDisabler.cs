using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nintenlord.Forms
{
    public class ControlsDisabler : IDisposable
    {
        Dictionary<Control, bool> oldStates;

        public ControlsDisabler(IEnumerable<Control> controls)
        {
            oldStates = controls.ToDictionary(x => x, x => x.Enabled);
            foreach (var item in controls)
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
