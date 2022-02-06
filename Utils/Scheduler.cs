using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public class Schedule
    {
        public int gametick;
        public readonly Action callback;

        public Schedule(int gametick, Action callback)
        {
            this.gametick = gametick;
            this.callback = callback;
        }

        public bool CountDown()
        {
            if (--gametick <= 0)
            {
                try
                {
                    callback();
                }
                catch (Exception e)
                {
                    Main.NewText("Exception caught in callback : " + e);
                }
            }

            return gametick > 0;
        }
    }

    public class Scheduler : ModSystem
    {
        private static List<Schedule> schedules;

        public static void Schedule(int gametick, Action callback)
        {
            schedules.Add(new Schedule(gametick, callback));
        }
        
        public override void PostUpdateWorld()
        {
            int n = schedules.Count, i = 0;
            while (i < n)
            {
                var t = schedules[i];
                if (!t.CountDown())
                {
                    schedules.Remove(t);
                    --n;
                }
                else ++i;
            }
        }

        public override void Load()
        {
            schedules = new List<Schedule>();
        }
    }
}