using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using MusicBuilder.Registry;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public class PlayingSound
    {
        public Point16 point;
        public readonly INoteKey key;

        public PlayingSound(Point16 point, Prog program, byte pitch, byte velocity)
        {
            this.point = point;
            key = ModContainer.device.Play(program, pitch, velocity);
        }

        public void Stop()
        {
            ModContainer.device.Stop(key);
        }
    }

    public class SoundManager : ModSystem
    {
        private static Dictionary<Point16, PlayingSound> sounds;
        
        public override void OnWorldLoad()
        {
            sounds = new Dictionary<Point16, PlayingSound>();
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
                if (snd.key != null)
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