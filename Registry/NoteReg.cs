using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MusicBuilder.Registry
{
    public static class NoteReg
    {
        public static Dictionary<Theme, ThemeData> themeData;
        public static Dictionary<Prog, NoteData> noteData;

        public class NoteData
        {
            public readonly string mod;
            public readonly NoteReg.ThemeData theme;
            public readonly string name;
            public readonly string path;
            public readonly Color txt;
            public readonly byte min;
            public readonly byte mid;
            public readonly byte max;
            public readonly byte sus;

            public NoteData(string mod, Theme theme, string name, uint txt, byte min = 0x30, byte mid = 60, byte max = 0x48, byte sus = 0)
            {
                this.mod = mod;
                this.theme = NoteReg.themeData[theme];
                this.name = name;
                string[] strArray = new string[] { mod, "/Sounds/Note/", this.theme.name, "/", name };
                this.path = string.Concat(strArray);
                this.txt = new Color((byte) (txt >> 0x10), (byte) (txt >> 8), (byte) txt);
                this.min = min;
                this.mid = mid;
                this.max = max;
                this.sus = sus;
            }
        }

        public class ThemeData
        {
            public readonly string name;
            public readonly byte maxTracks;
            public readonly Color[] color;

            public ThemeData(string name, params uint[] color)
            {
                this.name = name;
                this.maxTracks = (byte) color.Length;
                this.color = new Color[this.maxTracks];
                for (byte i = 0; i < this.maxTracks; i = (byte) (i + 1))
                {
                    this.color[i].PackedValue = color[i];
                }
            }
        }
    }
}
