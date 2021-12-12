using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using MusicBuilder.Registry;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public class PlayingSound
    {
        public struct Channel
        {
            public int[] key;
            public Prog program;
            public int activeCount;
        }
        public static Channel[] channels;
        private static int nextID;
        public Point16 point;
        private int length, keyID, channel;
        private Prog program;
        private byte pitch, velocity;
        public static void WorldReset()
        {
            channels = new Channel[16];
            for (int i = 0; i < 16; ++i)
            {
                channels[i].activeCount = 0;
                channels[i].key = new int[128];
            }
            channels[9].activeCount = 1;
            channels[9].program = (Prog) 1152;
        }

        public PlayingSound(Point16 point, ushort length, Prog program, byte pitch, byte velocity)
        {
            this.point = point;
            this.length = length;
            this.program = program;
            this.pitch = pitch;
            this.velocity = velocity;
            this.keyID = nextID++;
        }

        public bool Play()
        {
            uint msg;
            int channel = 0;
            for (channel = 0; channel < 16; ++channel)
                if (channels[channel].program == program && channels[channel].key[pitch] == 0)
                    break;
            if (channel == 16)
                for (channel = 0; channel < 16; ++channel)
                    if (channels[channel].activeCount == 0)
                    {
                        channels[channel].program = program;
                        msg = (uint)(0xc0 | (((int) program - 1024) << 8) | channel);
                        DLLContainer.midiOutShortMsg(ModContainer.midiHandle, msg);
                        break;
                    }
            if (channel == 16)
            {
                Main.NewText("SoundManager : MIDI supports 16 channels at most.");
                return false;
            }
            this.channel = channel;
            channels[channel].activeCount++;
            channels[channel].key[pitch] = keyID;
            msg = (uint)(0x90 | (pitch << 8) | (velocity << 16) | channel);
            DLLContainer.midiOutShortMsg(ModContainer.midiHandle, msg);
            return true;
        }

        public void Stop()
        {
            if (channels[channel].key[pitch] == keyID)
            {
                uint msg = (uint)(0x80 | (pitch << 8) | channel);
                DLLContainer.midiOutShortMsg(ModContainer.midiHandle, msg);
                channels[channel].activeCount--;
                channels[channel].key[pitch] = 0;
            }
        }

        public bool CountDown()
        {
            if (--length == 0)
                Stop();
            return length > -ModContainer.tailLength;
        }

        public double GetProgress()
        {
            return length > 0 ? 0.0 : -length / (double) ModContainer.tailLength;
        }
    }

    public class SoundManager : ModWorld
    {
        public static List<PlayingSound> sounds, psounds;
        public override void Initialize()
        {
            sounds = new List<PlayingSound>();
            psounds = new List<PlayingSound>();
            PlayingSound.WorldReset();
        }

        public static double GetProgress(Point16 point)
        {
            foreach (PlayingSound sound in sounds)
                if (sound.point == point) return sound.GetProgress();
            return 1.0;
        }

        public static void PlaySound(Point16 point, Prog program, byte pitch, ushort length, byte velocity)
        {
            psounds.Add(new PlayingSound(point, length, program, pitch, velocity));
        }

        public override void PostUpdate()
        {
            List<PlayingSound> temp = new List<PlayingSound>();
            foreach (PlayingSound sound in sounds)
                if (sound.CountDown())
                    temp.Add(sound);
            foreach (PlayingSound sound in psounds)
                if (sound.Play())
                    temp.Add(sound);
            sounds = temp;
            psounds = new List<PlayingSound>();
        }
    }
}