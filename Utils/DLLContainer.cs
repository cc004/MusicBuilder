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

        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static IntPtr midiParser;

        public delegate void ReadMidiCallback(IntPtr mem, ref Midi midi);
        public delegate void ReadMidi(string filename, ReadMidiCallback callback, ref Midi midi);
        public delegate uint GetUInt(IntPtr mem, int index);
        public delegate byte GetByte(IntPtr mem, int index);

        public static ReadMidi readMidi;
        public static GetUInt getUInt;
        public static GetByte getByte;

        public static void Load()
        {
            byte[] binary = ModContainer.instance.GetFileBytes("Utils\\midiParser\\midiParser.dll");
            string folder = Path.Combine(Main.SavePath, "Mods", "Cache");
            string dllfile = Path.Combine(folder, "midiParser.dll");

            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);
            FileStream stream = new FileStream(dllfile, FileMode.Create);
            stream.Write(binary, 0, binary.Length);
            stream.Close();
            
            midiParser = LoadLibrary(dllfile);
            int errCode = Marshal.GetLastWin32Error();
            readMidi = (ReadMidi) Marshal.GetDelegateForFunctionPointer(GetProcAddress(midiParser, "readMidi"), typeof(ReadMidi));
            getUInt = (GetUInt) Marshal.GetDelegateForFunctionPointer(GetProcAddress(midiParser, "getUInt"), typeof(GetUInt));
            getByte = (GetByte) Marshal.GetDelegateForFunctionPointer(GetProcAddress(midiParser, "getByte"), typeof(GetByte));
        }

        public static void Unload()
        {
            readMidi = null;
            getUInt = null;
            getByte = null;

            FreeLibrary(midiParser);
        }
    }
}