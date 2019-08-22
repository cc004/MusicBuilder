using System;
using System.Windows.Forms;

namespace MusicBuilder.Utils
{    
    public static class UserInterface
    {
        public static string GetFilenameFromUI()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "MIDI文件(*.mid;*.midi)|*.mid;*.midi";
            dialog.Multiselect = false;
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }
    }
}