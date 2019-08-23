using System;
using System.IO;
using System.Diagnostics;
using Terraria;
using Terraria.ModLoader;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public struct Note
    {
        public uint time;
        public ushort lasting;
        public byte instrument, pitch, velocity;
    }
    
    public struct Midi
    {
        public Note[] notes;
    }

    public static class MidiParser
    {
        private static void Callback(IntPtr mem, ref Midi res)
        {
            uint size = DLLContainer.getUInt(mem, 0);
            res.notes = new Note[size];
            for (int i = 0; i < size; ++i)
            {
                res.notes[i].time = DLLContainer.getUInt(mem, 12 * i + 4);
                res.notes[i].lasting = DLLContainer.getUShort(mem, 12 * i + 8);
                res.notes[i].instrument = DLLContainer.getByte(mem, 12 * i + 10);
                res.notes[i].pitch = DLLContainer.getByte(mem, 12 * i + 11);
                res.notes[i].velocity = DLLContainer.getByte(mem, 12 * i + 12);
            }
        }

        public static Midi ParseMidi(string filename)
        {
            Midi res = new Midi();
            DLLContainer.readMidi(filename, Callback, ref res);
            return res;
        }
    }

}