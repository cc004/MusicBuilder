using MusicBuilder.Registry;
using Terraria;
using System;

namespace MusicBuilder.Items
{
    public class Midi_drum : Noteblock
    {
        public override void AddRecipes() {}
        public override Prog NOTE
        {
            get
            {
                return (Prog) 1152;
            }
        }
    }
}