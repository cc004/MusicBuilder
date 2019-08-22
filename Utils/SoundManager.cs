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
        private SoundEffectInstance instance;
        public Point16 point;
        private int length;
        
        public PlayingSound(SoundEffectInstance instance, int length, Point16 point, byte velocity)
        {
            this.length = length;
            this.instance = instance;
            this.point = point;
            instance.Volume = velocity / 128.0f;
            Main.PlaySoundInstance(instance);
        }

        public bool CountDown()
        {
            if (length-- == 0) instance.Stop();
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
        }

        public static double GetProgress(Point16 point)
        {
            foreach (PlayingSound sound in sounds)
                if (sound.point == point) return sound.GetProgress();
            return 1.0;
        }

        public static void PlaySound(int length, Point16 point, SoundEffectInstance instance, byte velocity)
        {
            sounds.Add(new PlayingSound(instance, length, point, velocity));
        }

        public static SoundEffectInstance GetSound(Prog note, byte pitch)
        {
            SoundEffectInstance instance = Terraria.ModLoader.ModLoader.GetSound(NoteReg.noteData[note].path + ((sbyte) (0.08333f * (pitch - NoteReg.noteData[note].mid)))).CreateInstance();
            instance.Volume = 1f;
            instance.Pan = 0f;
            instance.Pitch = (0.08333f * (pitch - NoteReg.noteData[note].mid)) % 1f;
            return instance;
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