using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class FormLoading : Form
    {
        public FormLoading()
        {
            this.InitializeComponent();
        }

        new public void Show()
        {
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Show();
            this.Refresh();

            // force the wait window to display for at least 700ms so it doesn't just flash on the screen
            System.Threading.Thread.Sleep(700);
            Application.Idle += this.OnLoaded;
        }

        public void SetLoading(String message, Single percent)
        {
            Int32 value = (Int32)Math.Min(percent, 100);
            this.LoadingLabel.Text = message;
            this.LoadingBar.Value = value;
            this.Refresh();
        }
        public void SetMessage(String message)
        {
            this.LoadingLabel.Text = message;
            this.Refresh();
        }
        public void SetPercent(Single percent)
        {
            Int32 value = (Int32)Math.Min(percent, 100);
            this.LoadingBar.Value = value;
            this.Refresh();
        }

        private void OnLoaded(Object sender, EventArgs e)
        {
            Application.Idle -= this.OnLoaded;
            this.Close();
        }
    }
}
