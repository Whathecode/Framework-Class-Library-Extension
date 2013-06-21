using System.Windows.Forms;

namespace Whathecode.System.Windows.Interop
{
    /// <summary>
    /// Internal class use to hook into the Windows Message Loop
    /// Needs System.Windows.Forms
    /// </summary>
    internal class NativeWindowEx : NativeWindow
    {
        public delegate void MessageRecievedEventHandler(ref Message m);
        public event MessageRecievedEventHandler MessageRecieved;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (MessageRecieved != null)
            {
                MessageRecieved(ref m);
            }
        }
    }
}
