using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using MusicBuilder.Utils;
using MusicBuilder.Registry;
using System;

namespace MusicBuilder.Items
{
    public class BuildTool : ModItem
    {
        private const int COLMAX = 256;

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(base.mod);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        private static void DoDelay(ref int x, ref int y, int delay, ref int curmax, ref int status)
        {
            int p = ModContainer.time.Length;
            while ((p--) != 0)
                while (delay >= ModContainer.time[p])
                {
                    if (status == 0 || status == COLMAX)
                    {
                        x += status == 0 ? -1 : 1;
                        for (int i = y; i < y + curmax + 3; ++i)
                            Builder.PlaceWire(x, i, WireType.Green);
                        y += curmax + 2;
                        Builder.PlaceWire(x + (status == 0 ? 1 : -1), y, WireType.Green);
                        curmax = 0;
                    }
                    if (status < COLMAX)
                        Builder.PlaceTimer(++x, y, Direction.Right, ModContainer.time[p]);
                    else
                        Builder.PlaceTimer(--x, y, Direction.Left, ModContainer.time[p]);
                    status = (status + 1) % (2 * COLMAX);
                    delay -= ModContainer.time[p];
                }
        }

        public override bool UseItem(Player player)
        {
            int x = Player.tileTargetX, y = Player.tileTargetY;
            string filename = UserInterface.GetFilenameFromUI();
            if (filename == null) return true;
            Midi midi = MidiParser.ParseMidi(filename);
            int curnote = 1, curmax = 0, status = 1;
            uint cur = 0;
            bool startNote = true;
            //starting wire
            Builder.PlaceTimer(x, y, Direction.Right, 0);

            foreach (Note note in midi.notes)
            {
                if (note.time != cur | startNote) // this is a new note
                {
                    startNote = false;
                    DoDelay(ref x, ref y, (int)(note.time - cur), ref curmax, ref status); //place delayer and refresh x position
                    curnote = 1;
                    Builder.PlaceWire(x, y + curnote, ((x & 0x1) == 0) ? WireType.Red : WireType.Blue);
                }
                curmax = Math.Max(curmax, ++curnote);
                Builder.PlaceNoteBlock(x, y + curnote, (Prog) (1024 + note.instrument), note.pitch, note.lasting, note.velocity);
                Builder.PlaceWire(x, y + curnote, ((x & 0x1) == 0) ? WireType.Red : WireType.Blue);
                cur = note.time;

            }
            return true;
        }

        public override void SetDefaults()
        {
            base.item.consumable = true;
            base.item.width = 32;
            base.item.height = 32;
            base.item.useAnimation = 0x19;
            base.item.useTime = 30;
            base.item.useStyle = 5;
            base.item.rare = 11;
            base.item.channel = true;
            base.item.autoReuse = false;
            base.item.value = 0x0;
            base.item.expert = true;
        }

        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("小木斧");
            base.Tooltip.SetDefault("可以召唤神奇的TSPL乐队\n" +
                "也是给沉睡的年级第一的礼物");
            base.SetStaticDefaults();
        }
    }

}