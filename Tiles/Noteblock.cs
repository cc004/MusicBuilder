using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MusicBuilder.Utils;
using MusicBuilder.Registry;
using MusicBuilder;
using System;
using MusicBuilder.Commands;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MusicBuilder.Tiles
{
    public abstract class Noteblock<T> : ModTile where T : ModItem
    {
        const int min = 0, max = 127;

        public override string Texture => "MusicBuilder/Tiles/NoteblockBorder";

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        private static Color bgc;

        public override void SetStaticDefaults()
        {
            bgc = ColorUtils.ColorHue(new Random(GetType().Name.GetHashCode()).NextDouble());
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoSunLight[Type] = false;
            ItemDrop = ModContent.ItemType<T>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(GetType().Name);
            AddMapEntry(Registries.noteData[this.NOTE].bgc, name);
        }

        public override bool HasSmartInteract() => false;
        
        public override void HitWire(int i, int j)
        {
            SoundManager.PlaySound(
                new Point16(i, j),
                NOTE,
                DataCore.extField[i, j].pitch,
                DataCore.extField[i, j].velocity
            );
        }
        
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Color color = ColorUtils.ColorBlend(Registries.noteData[this.NOTE].bgc, new Color(0, 0, 0), SoundManager.GetProgress(new Point16(i, j)));
            r = color.R / 255.0f * 1.2f;
            g = color.G / 255.0f * 1.2f;
            b = color.B / 255.0f * 1.2f;
		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color c = bgc;
            c = ColorUtils.ColorBlend(c, new Color(c.R / 2, c.G / 2, c.B / 2), SoundManager.GetProgress(new Point16(i, j)));

            int num = ((i * 0x10) - ((int) Main.screenPosition.X)) + Main.offScreenRange;
            int num2 = ((j * 0x10) - ((int) Main.screenPosition.Y)) + Main.offScreenRange;
            spriteBatch.Draw(TextureContainer.TextureBorder.Value, new Vector2((float) num, (float) num2), Lighting.GetColor(i, j, c));
            spriteBatch.Draw(TextureContainer.TexturePitch.Value, new Vector2((float) num, (float) num2), new Rectangle(0x12 * (DataCore.extField[i, j].pitch % 12), 0, 0x10, 0x10), Lighting.GetColor(i, j, Color.Black));
            spriteBatch.Draw(TextureContainer.TextureOctave.Value, new Vector2((float) (num + 8), (float) (num2 + 8)), new Rectangle(6 * (DataCore.extField[i, j].pitch / 12), 0, 6, 6), Lighting.GetColor(i, j, Color.Black));
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            if (Main.player[Main.myPlayer].mouseInterface) return false;

            if (Main.keyState.IsKeyDown(Keys.LeftAlt))
            {
                Main.NewText("Note block selected (" + i + "," + j + ")");
                MyCommand.selection = new Point16(i, j);
            }
            else if (Main.keyState.IsKeyDown(Keys.LeftShift))
            {
                DataCore.extField[i, j].pitch = (byte)((DataCore.extField[i, j].pitch + 12) % max);
            }
            else
            {
                DataCore.extField[i, j].pitch = (byte)((DataCore.extField[i, j].pitch + 1) % max);
            }

            HitWire(i, j);
            Scheduler.Schedule(60, () => HitWire(i, j));
            return true;
        }
        
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (DataCore.extField[i, j].pitch > 127)
                DataCore.extField[i, j].pitch = 0;
            if (DataCore.extField[i, j].velocity > 127)
                DataCore.extField[i, j].velocity = 127;
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            base.PlaceInWorld(i, j, item);
            DataCore.extField[i, j].pitch = 48;
            DataCore.extField[i, j].velocity = 64;
        }
        public virtual Prog NOTE => Prog.None;
    }
}