using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.ModLoader;
using MusicBuilder.Registry;
using MusicBuilder.Utils;
using MusicBuilder.Tiles;
using Microsoft.Xna.Framework;

namespace MusicBuilder
{
    class ModContainer : Mod
    {
        public static IntPtr midiHandle;
        public static Mod instance;
        public static int[] time = new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 32, 64, 128, 256};
        public static string[] Instrument = new string[]{
            "acpiano",   "britepno",  "synpiano",  "honkytonk", "epiano1",   "epiano2",
            "hrpschrd",  "clavinet",  "celeste",   "glocken",   "musicbox",  "vibes",
            "marimba",   "xylophon",  "tubebell",  "santur",    "homeorg",   "percorg",
            "rockorg",   "churchorg", "reedorg",   "accordn",   "harmonica", "concrtna",
            "nyguitar",  "acguitar",  "jazzgtr",   "cleangtr",  "mutegtr",   "odguitar",
            "distgtr",   "gtrharm",   "acbass",    "fngrbass",  "pickbass",  "fretless",
            "slapbas1",  "slapbas2",  "synbass1",  "synbass2",  "violin",    "viola",
            "cello",     "contraba",  "marcato1",   "pizzcato",  "harp",      "timpani",
            "marcato2",  "slowstr",   "synstr1",   "synstr2",   "choir",     "doo",
            "voices",    "orchhit",   "trumpet",   "trombone",  "tuba",      "mutetrum",
            "frenchorn", "hitbrass",  "synbras1",  "synbras2",  "sprnosax",  "altosax",
            "tenorsax",  "barisax",   "oboe",      "englhorn",  "bassoon",   "clarinet",
            "piccolo",   "flute",     "recorder",  "woodflut",  "bottle",    "shakazul",
            "whistle",   "ocarina",   "sqrwave",   "sawwave",   "calliope",  "chiflead",
            "charang",   "voxlead",   "lead5th",   "basslead",  "fantasia",  "warmpad",
            "polysyn",   "ghostie",   "bowglass",  "metalpad",  "halopad",   "sweeper1",
            "aurora",    "soundtrk",  "crystal",   "atmosphr",  "freshair",  "unicorn",
            "sweeper2",   "startrak",  "sitar",     "banjo",     "shamisen",  "koto",
            "kalimba",   "bagpipes",  "fiddle",    "shannai",   "carillon",  "agogo",
            "steeldrum", "woodblock", "taiko",     "toms",      "syntom",    "revcymb",
            "fxfret",    "fxblow",    "seashore",  "jungle",    "telephone", "helicptr",
            "applause",  "ringwhsl",  "drum"
        };

        public const int tailLength = 90;
        public override void Load()
        {
            Random rand = new Random();

            instance = this;
            
            Registries.noteData = new Dictionary<Prog, NoteData>();
            Registries.delayData = new Dictionary<int, DelayData>();
            Registries.delayers = new List<int>();

            for (int i = 0; i < 129; ++i)
                Registries.noteData.Add((Prog) (1024 + i), new NoteData(new Color(0, 0, 0), ColorUtils.ColorHue(i / 129.0f), Instrument[i]));

            for (int i = -1; i < time.Length; ++i)
            {
                Color lit = ColorUtils.ColorHue(i * 1.0f / time.Length);
                Color bg = new Color((lit.R + 0) / 4, (lit.G + 0) / 4, (lit.B + 0) / 4);
                Color off = new Color((lit.R + 0) / 2, (lit.G + 0) / 2, (lit.B + 0) / 2);
                Registries.delayData.Add((ushort)(i == -1 ? 0 : time[i]), new DelayData(bg, off, lit));
            }

            Delayer.Load();
            Noteblock.Load();
            DLLContainer.Load();

            IntPtr nullptr = new IntPtr(0);
            uint result = DLLContainer.midiOutOpen(out midiHandle, 0u, nullptr, nullptr, 0);
            if (result != 0)
                throw new SystemException("Failed to open midi device. code " + result);

        }

        public override void Unload()
        {
            Registries.noteData = null;
            Registries.delayers = null;
            Registries.delayData = null;

            Delayer.Unload();
            Noteblock.Unload();
            DLLContainer.midiOutClose(midiHandle);
            DLLContainer.Unload();

            instance = null;
        }

        public override void PreSaveAndQuit()
		{
            foreach (PlayingSound sound in SoundManager.sounds)
                sound.Stop();
            SoundManager.psounds = new List<PlayingSound>();
		}
    }

}