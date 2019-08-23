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
        private struct Channel
        {
            public int[] key;
            public Prog program;
            public int activeCount;
        }
        private static Channel[] channels;
        private static int nextID;
        public Point16 point;
        private int length, keyID, channel;
        private Prog program;
        private byte pitch;
        public bool playing{get;internal set;}
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

        public PlayingSound(Point16 point, byte length, Prog program, byte pitch, byte velocity)
        {
            uint msg;
            int channel = 0;
            this.point = point;
            this.length = length;
            this.program = program;
            this.pitch = pitch;
            this.keyID = nextID++;
            for (channel = 0; channel < 16; ++channel)
                if (channels[channel].program == program)
                    break;
            if (channel == 16)
                for (channel = 0; channel < 16; ++channel)
                    if (channels[channel].activeCount == 0)
                    {
                        channels[channel].program = program;
                        uint pmsg = (uint)(0xc0 | (((int) program - 1024) << 8) | channel);
                        DLLContainer.midiOutShortMsg(ModContainer.midiHandle, pmsg);
                        break;
                    }
            if (channel == 16)
            {
                Main.NewText("SoundManager : MIDI supports 16 channels at most.");
                return;
            }
            playing = true;
            this.channel = channel;
            if (channels[channel].key[pitch] != 0)
                ++channels[channel].activeCount;
            channels[channel].key[pitch] = keyID;
            msg = (uint)(0x90 | (pitch << 8) | (velocity << 16) | channel);
            DLLContainer.midiOutShortMsg(ModContainer.midiHandle, msg);
        }

        public bool CountDown()
        {
            if (length-- == 0)
            {
                if (channels[channel].key[pitch] == keyID)
                {
                    uint msg = (uint)(0x80 | (pitch << 8) | channel);
                    DLLContainer.midiOutShortMsg(ModContainer.midiHandle, msg);
                    channels[channel].key[pitch] = 0;
                    --channels[channel].activeCount;
                }
            }
            return length > -ModContainer.tailLength;
        }

        public double GetProgress()
        {
            return length > 0 ? 0.0 : -length / (double) ModContainer.tailLength;
        }
    }

    public class SoundManager : ModWorld
    {
        public static List<PlayingSound> sounds;

        public override void Initialize()
        {
            sounds = new List<PlayingSound>();
            PlayingSound.WorldReset();
        }

        public static double GetProgress(Point16 point)
        {
            foreach (PlayingSound sound in sounds)
                if (sound.point == point) return sound.GetProgress();
            return 1.0;
        }

        public static void PlaySound(Point16 point, Prog program, byte pitch, byte length, byte velocity)
        {
            PlayingSound sound = new PlayingSound(point, length, program, pitch, velocity);
            if (sound.playing)
                sounds.Add(sound);
        }

        public override void PostUpdate()
        {
            List<PlayingSound> temp = new List<PlayingSound>();
            foreach (PlayingSound sound in sounds)
                if (sound.CountDown())
                    temp.Add(sound);
            sounds = temp;
        }
    }
}