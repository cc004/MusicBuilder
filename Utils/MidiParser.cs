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
        public byte instrument, pitch, velocity, lasting;
        public uint time;
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
                res.notes[i].instrument = DLLContainer.getByte(mem, 8 * i + 4);
                res.notes[i].pitch = DLLContainer.getByte(mem, 8 * i + 5);
                res.notes[i].velocity = DLLContainer.getByte(mem, 8 * i + 6);
                res.notes[i].lasting = DLLContainer.getByte(mem, 8 * i + 7);
                res.notes[i].time = (uint) DLLContainer.getUInt(mem, 8 * i + 8);
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