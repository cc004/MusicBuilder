using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MusicBuilder.Utils;
using MusicBuilder.Registry;
using MusicBuilder;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using MusicBuilder.Utils;

namespace MusicBuilder.Tiles
{
    public class Noteblock : ModTile
    {
        const int min = 0, max = 127;
        public static Texture2D TextureBorder;
        public static Texture2D TexturePitch;
        public static Texture2D TextureOctave;
        public static Point16 selection;
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "MusicBuilder/Tiles/NoteblockBorder";
            return (this.NOTE != Prog.None);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void SetDefaults()
        {
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            base.disableSmartCursor = true;
            Main.tileFrameImportant[base.Type] = true;
            base.drop = base.mod.ItemType(Registries.noteData[this.NOTE].name);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault(Registries.noteData[this.NOTE].name);
			AddMapEntry(Registries.noteData[this.NOTE].bgc, name);
        }

        public override void HitWire(int i, int j)
        {
            SoundManager.PlaySound(
                new Point16(i, j),
                NOTE,
                DataCore.extField[i, j].pitch,
                DataCore.extField[i, j].velocity
            );
        }

        public static void Load()
        {
            TextureBorder = ModContainer.instance.GetTexture("Tiles/NoteblockBorder");
            TexturePitch = ModContainer.instance.GetTexture("Tiles/NoteblockPitch");
            TextureOctave = ModContainer.instance.GetTexture("Tiles/NoteblockOctave");
        }

        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[base.Type] = false;
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
            Color c = Registries.noteData[this.NOTE].bgc;
            c = ColorUtils.ColorBlend(c, new Color(c.R / 2, c.G / 2, c.B / 2), SoundManager.GetProgress(new Point16(i, j)));

            int num = ((i * 0x10) - ((int) Main.screenPosition.X)) + Main.offScreenRange;
            int num2 = ((j * 0x10) - ((int) Main.screenPosition.Y)) + Main.offScreenRange;
            spriteBatch.Draw(TextureBorder, new Vector2((float) num, (float) num2), Lighting.GetColor(i, j, c));
            spriteBatch.Draw(TexturePitch, new Vector2((float) num, (float) num2), new Rectangle(0x12 * (DataCore.extField[i, j].pitch % 12), 0, 0x10, 0x10), Lighting.GetColor(i, j, Registries.noteData[this.NOTE].txt));
            spriteBatch.Draw(TextureOctave, new Vector2((float) (num + 8), (float) (num2 + 8)), new Rectangle(6 * (DataCore.extField[i, j].pitch / 12), 0, 6, 6), Lighting.GetColor(i, j, Registries.noteData[this.NOTE].txt));
            return false;
        }

        //TODO: add velocity change support.
        public override void RightClick(int i, int j)
        {
            if (!Main.player[Main.myPlayer].mouseInterface)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    Main.NewText("Note block selected (" + i + "," + j + ")");
                    selection = new Point16(i, j);
                    this.HitWire(i, j);
                }
                else if (Main.keyState.IsKeyDown(Keys.LeftShift))
                {
                    DataCore.extField[i, j].pitch = (byte) ((DataCore.extField[i, j].pitch + 12) % max);
                    this.HitWire(i, j);
                }
                else
                {
                    DataCore.extField[i, j].pitch = (byte) ((DataCore.extField[i, j].pitch + 1) % max);
                    this.HitWire(i, j);
                }
            }
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

        public static void Unload()
        {
            TextureBorder = null;
            TexturePitch = null;
            TextureOctave = null;
        }

        public virtual Prog NOTE => Prog.None;
    }
}