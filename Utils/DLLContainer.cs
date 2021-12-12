using System;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public static class DLLContainer
    {
        
        [DllImport("winmm.dll")]
        public static extern uint midiOutOpen(out IntPtr lphMidiOut, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, UInt32 dwFlags);

        [DllImport("winmm.dll")]
        public static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

        [DllImport("winmm.dll")]
        public static extern uint midiOutClose(IntPtr hMidiOut);
    }
}