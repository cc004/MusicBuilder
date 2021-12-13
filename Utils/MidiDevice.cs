using System;
using System.IO;
using System.Runtime.InteropServices;
using Terraria;
using MusicBuilder;
using MusicBuilder.Registry;

namespace MusicBuilder.Utils
{
    public interface INoteKey
    {
        
    }

    public sealed class MidiDevice : IDisposable
    {
        
        [DllImport("winmm.dll")]
        private static extern uint midiOutOpen(out IntPtr lphMidiOut, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, UInt32 dwFlags);

        [DllImport("winmm.dll")]
        private static extern uint midiOutShortMsg(IntPtr hMidiOut, uint dwMsg);

        [DllImport("winmm.dll")]
        private static extern uint midiOutClose(IntPtr hMidiOut);

        private readonly IntPtr handle;

        internal class Channel
        {
            public Guid[] user = new Guid[128];
            public Prog program;
            public int activeCount;
        }

        private class NoteKey : INoteKey
        {
            public Guid guid;
            public int pitch;
            public int channel;
        }

        internal readonly Channel[] channels;

        public MidiDevice()
        {
            var result = MidiDevice.midiOutOpen(out handle, 0u, IntPtr.Zero, IntPtr.Zero, 0);
            if (result != 0)
                throw new SystemException("Failed to open midi device. code " + result);
            channels = new Channel[16];
            for (int i = 0; i < 16; ++i) channels[i] = new Channel();
            channels[9].activeCount = 1;
            channels[9].program = (Prog)1152;
        }

        public INoteKey Play(Prog program, byte pitch, byte velocity)
        {
            var channel = 0;
            for (; channel < 16; ++channel)
                if (channels[channel].program == program && channels[channel].user[pitch] == Guid.Empty)
                    break;
            if (channel == 16)
                for (channel = 0; channel < 16; ++channel)
                    if (channels[channel].activeCount == 0)
                    {
                        channels[channel].program = program;
                        midiOutShortMsg(handle, (uint)(0xc0 | (((int)program - 1024) << 8) | channel));
                        break;
                    }
            if (channel == 16)
            {
                Main.NewText("SoundManager : MIDI supports 16 channels at most.");
                return null;
            }

            var key = Guid.NewGuid();
            channels[channel].activeCount++;
            channels[channel].user[pitch] = key;
            midiOutShortMsg(handle, (uint)(0x90 | (pitch << 8) | (velocity << 16) | channel));
            return new NoteKey
            {
                channel = channel,
                pitch = pitch,
                guid = key
            };
        }

        public void Stop(INoteKey key)
        {
            var k = key as NoteKey;
            if (channels[k.channel].user[k.pitch] != k.guid) return;
            midiOutShortMsg(handle, (uint)(0x80 | (k.pitch << 8) | k.channel));
            channels[k.channel].activeCount--;
            channels[k.channel].user[k.pitch] = Guid.Empty;
        }

        public void Dispose()
        {
            midiOutClose(handle);
        }
    }
}