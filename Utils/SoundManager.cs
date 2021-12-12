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
        private int keyID, channel;
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

        public PlayingSound(Point16 point, Prog program, byte pitch, byte velocity)
        {
            this.point = point;
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
    }

    public class SoundManager : ModWorld
    {
        private static Dictionary<Point16, PlayingSound> sounds;
        
        public override void Initialize()
        {
            sounds = new Dictionary<Point16, PlayingSound>();
            PlayingSound.WorldReset();
        }

        public static double GetProgress(Point16 point)
        {
            return sounds.ContainsKey(point) ? 0.0 : 1.0;
        }

        public static void PlaySound(Point16 point, Prog program, byte pitch, byte velocity)
        {
            if (sounds.TryGetValue(point, out var snd))
            {
                snd.Stop();
                sounds.Remove(point);
            }
            else
            {
                snd = new PlayingSound(point, program, pitch, velocity);
                if (snd.Play())
                    sounds.Add(point, snd);
            }
        }
        
        public static void StopAll()
        {
            foreach (var pair in sounds)
            {
                pair.Value.Stop();
            }
            sounds.Clear();
        }
    }
}