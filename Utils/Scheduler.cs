using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public delegate void SchedulerCallback(int x, int y, object param);

    public class Schedule
    {
        public int gametick, x, y;
        public SchedulerCallback callback;
        public object param;

        public Schedule(int x, int y, int gametick, SchedulerCallback callback, object param)
        {
            this.x = x;
            this.y = y;
            this.gametick = gametick;
            this.callback = callback;
            this.param = param;
        }

        public bool CountDown()
        {
            if (--gametick == 0)
            {
                try
                {
                    callback(x, y, param);
                }
                catch (Exception e)
                {
                    Main.NewText("Exception caught in callback : " + e);
                }
            }
            return gametick > -ModContainer.tailLength;
        }

        public double GetProgress()
        {
            return gametick > 0 ? 0.0 : -gametick / (double) ModContainer.tailLength;
        }
    }

    public class Scheduler : ModWorld
    {
        private static List<Schedule> schedules;
        private static List<Schedule> newSchedules;

        public static void Schedule(int x, int y, int gametick, SchedulerCallback callback, object param)
        {
            if (gametick == 0)
                schedules.Add(new Schedule(x, y, 1, callback, param));
            else
                newSchedules.Add(new Schedule(x, y, gametick, callback, param));
        }

        public static double GetProgress(int x, int y)
        {
            double result = 1.0;
            Point16 p = new Point16(x, y);
            foreach (Schedule schedule in newSchedules)
                if (schedule.x == x && schedule.y == y) return 0.0;
            foreach (Schedule schedule in schedules)
                if (schedule.x == x && schedule.y == y) result = Math.Min(schedule.GetProgress(), result);
            return result;
        }

        public override void PostUpdate()
        {
            List<Schedule> temp = new List<Schedule>();
            List<Schedule> schedules_ = schedules;
            foreach (Schedule schedule in schedules_)
                if (schedule.CountDown())
                    temp.Add(schedule);

            foreach (Schedule schedule in newSchedules)
                    temp.Add(schedule);
            
            newSchedules = new List<Schedule>();
            schedules = temp;
        }

        public override void Initialize()
        {
            schedules = new List<Schedule>();
            newSchedules = new List<Schedule>();
        }
    }
}