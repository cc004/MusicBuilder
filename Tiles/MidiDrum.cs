using System;
using Terraria;
using Terraria.DataStructures;
using MusicBuilder.Registry;
using MusicBuilder.Utils;

namespace MusicBuilder.Tiles
{
    public class Midi_drum : Noteblock
    {
        public override void HitWire(int i, int j)
        {
            int pitch =  DataCore.extField[i, j].data0;
            Prog prog = (Prog) (pitch / 11 + 1153);
            pitch = pitch % 11 * 12;
            SoundManager.PlaySound(DataCore.extField[i, j].data1, new Point16(i, j), SoundManager.GetSound(prog, (byte) pitch), DataCore.extField[i, j].data2);
        }

        public override Prog NOTE
        {
            get
            {
                return (Prog) 1152;
            }
        }
    }
}
