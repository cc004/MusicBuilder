using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using MusicBuilder.Tiles;
using MusicBuilder.Utils;

namespace MusicBuilder.Commands
{
    public class mb : ModCommand
    {
        public override string Command => "mb";

        public override CommandType Type => CommandType.Chat;

        public override string Description => "Used for debugging.";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (args.Length < 1) return;
            if (args[0].ToLower() == "channel")
            {
                string result = "";
                for (int i = 0; i < 16; ++i)
                {
                    result = result + PlayingSound.channels[i].activeCount + " ";
                }
                Main.NewText(result);
                return;
            }
            if (Noteblock.selection == new Point16(-1, -1))
            {
                Main.NewText("You should make a selection first.");
                return;
            }
            int x = Noteblock.selection.X, y = Noteblock.selection.Y;
            byte value;
            ushort values;
            switch (args[0].ToLower()[0])
            {
                case 'p':
                    value = (byte) Math.Max(0, Math.Min(127, int.Parse(args[0].Substring(1))));
                    DataCore.extField[x, y].pitch = value;
                    Main.NewText("pitch changed to " + value);
                    new Noteblock().HitWire(x, y);
                    break;
                case 'v':
                    value = (byte) Math.Max(0, Math.Min(127, int.Parse(args[0].Substring(1))));
                    DataCore.extField[x, y].velocity = value;
                    Main.NewText("velocity changed to " + value);
                    new Noteblock().HitWire(x, y);
                    break;
            }
        }
    }
}