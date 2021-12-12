using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MusicBuilder.Registry
{
    public enum Prog
    {
        None = 0
    }

    public class NoteData
    {
        public readonly Color txt, bgc;
        public readonly string name;

        public NoteData(Color txt, Color bgc, string name)
        {
            this.txt = txt;
            this.bgc = bgc;
            this.name = name;
        }
    }
    public class DelayData
    {
        public readonly Color bgc;
        public readonly Color off;
        public readonly Color lit;

        public DelayData(Color bgc, Color off, Color lit)
        {
            this.bgc = bgc;
            this.off = off;
            this.lit = lit;
        }
    }
    
    public static class Registries
    {
        public static Dictionary<Prog, NoteData> noteData;
    }
}