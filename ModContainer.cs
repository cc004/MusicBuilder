using System;
using System.Collections.Generic;
using System.IO;
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
        public static Mod instance;
        public static int[] time = new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 32, 64, 128};
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
            
            NoteReg.themeData = new Dictionary<Theme, NoteReg.ThemeData>();
            NoteReg.noteData = new Dictionary<Prog, NoteReg.NoteData>();
            DelayReg.delayData = new Dictionary<ushort, DelayReg.DelayData>();

            NoteReg.themeData.Add((Theme) 1024, new NoteReg.ThemeData("Midi", new uint[]{0x000000}));


            for (int i = 0; i < 129; ++i)
                NoteReg.noteData.Add((Prog) (1024 + i), new NoteReg.NoteData("WavHolder", (Theme) 1024, Instrument[i], ColorUtils.ColorHue(rand.NextDouble()).PackedValue, 0, 0, 127, 0));
            for (int i = 0; i < 12; ++i)
                NoteReg.noteData.Add((Prog) (1153 + i), new NoteReg.NoteData("WavHolder", (Theme) 1024, "drum" + i + "dummy", 0x0000000, 0, 0, 127, 0));

            for (int i = -1; i < time.Length; ++i)
            {
                Color lit = ColorUtils.ColorHue(rand.NextDouble());
                Color bg = new Color((lit.R + 0) / 4, (lit.G + 0) / 4, (lit.B + 0) / 4);
                Color off = new Color((lit.R + 0) / 2, (lit.G + 0) / 2, (lit.B + 0) / 2);
                DelayReg.delayData.Add((ushort)(i == -1 ? 0 : time[i]), new DelayReg.DelayData(bg.PackedValue, off.PackedValue, lit.PackedValue));
            }

            Delayer.Load();
            Noteblock.Load();
            DLLContainer.Load();

        }

        public override void Unload()
        {
            NoteReg.themeData = null;
            NoteReg.noteData = null;
            DelayReg.delayData = null;

            Delayer.Unload();
            Noteblock.Unload();
            DLLContainer.Unload();

            instance = null;
        }
    }

}