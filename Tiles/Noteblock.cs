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
        
        public static Texture2D TextureBorder;
        public static Texture2D TexturePitch;
        public static Texture2D TextureOctave;
        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "MusicBuilder/Tiles/NoteblockBorder";
            return (this.NOTE != Prog.None);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool Slope(int i, int j)
        {
            byte num;
            Main.tile[i, j].halfBrick(false);
            Main.tile[i, j].slope(0);
            DataCore.extField[i, j].data0 = (byte) ((num = DataCore.extField[i, j].data0) - 1);
            if (num < NoteReg.noteData[this.NOTE].min)
            {
                DataCore.extField[i, j].data0 = NoteReg.noteData[this.NOTE].max;
            }
            this.HitWire(i, j);
            NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
            return true;
        }

        public override void SetDefaults()
        {
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            base.disableSmartCursor = true;
            Main.tileFrameImportant[base.Type] = true;
            base.drop = base.mod.ItemType(NoteReg.noteData[this.NOTE].theme.name + "_" + NoteReg.noteData[this.NOTE].name);
        }

        public override void HitWire(int i, int j)
        {
            SoundManager.PlaySound(DataCore.extField[i, j].data1, new Point16(i, j), SoundManager.GetSound(this.NOTE, DataCore.extField[i, j].data0), DataCore.extField[i, j].data2);
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
            Color color = ColorUtils.ColorBlend(NoteReg.noteData[this.NOTE].txt, new Color(0, 0, 0), SoundManager.GetProgress(new Point16(i, j)));
            r = color.R / 256.0f;
            g = color.G / 256.0f;
            b = color.B / 256.0f;
		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Color c = NoteReg.noteData[this.NOTE].txt;
            c = ColorUtils.ColorBlend(c, new Color(c.R / 2, c.G / 2, c.B / 2), SoundManager.GetProgress(new Point16(i, j)));

            int num = ((i * 0x10) - ((int) Main.screenPosition.X)) + Main.offScreenRange;
            int num2 = ((j * 0x10) - ((int) Main.screenPosition.Y)) + Main.offScreenRange;
            spriteBatch.Draw(TextureBorder, new Vector2((float) num, (float) num2), Lighting.GetColor(i, j, c));
            spriteBatch.Draw(TexturePitch, new Vector2((float) num, (float) num2), new Rectangle(0x12 * (DataCore.extField[i, j].data0 % 12), 0, 0x10, 0x10), Lighting.GetColor(i, j, NoteReg.noteData[this.NOTE].theme.color[0]));
            spriteBatch.Draw(TextureOctave, new Vector2((float) (num + 8), (float) (num2 + 8)), new Rectangle(6 * (DataCore.extField[i, j].data0 / 12), 0, 6, 6), Lighting.GetColor(i, j, NoteReg.noteData[this.NOTE].theme.color[0]));
            return false;
        }

        //TODO: add velocity change support.
        public override void RightClick(int i, int j)
        {
            if (!Main.player[Main.myPlayer].mouseInterface)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    DataCore.extField[i, j].data1 = (byte) (DataCore.extField[i, j].data1 +  (Main.keyState.IsKeyDown(Keys.LeftShift) ? -1 : 1));
                    Main.NewText("Note length changed into " + DataCore.extField[i, j].data1 + " tick(s)");
                    this.HitWire(i, j);
                }
                else if (Main.keyState.IsKeyDown(Keys.LeftShift))
                {
                    if ((DataCore.extField[i, j].data0 = (byte) (DataCore.extField[i, j].data0 + 12)) > NoteReg.noteData[this.NOTE].max)
                    {
                        DataCore.extField[i, j].data0 = NoteReg.noteData[this.NOTE].min;
                    }
                    this.HitWire(i, j);
                }
                else
                {
                    byte num3;
                    DataCore.extField[i, j].data0 = (byte) ((num3 = DataCore.extField[i, j].data0) + 1);
                    if (num3 > NoteReg.noteData[this.NOTE].max)
                    {
                        DataCore.extField[i, j].data0 = NoteReg.noteData[this.NOTE].min;
                    }
                    this.HitWire(i, j);
                }
            }
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (DataCore.extField[i, j].data0 < NoteReg.noteData[this.NOTE].min)
                DataCore.extField[i, j].data0 = NoteReg.noteData[this.NOTE].min;
            return true;
        }

        public static void Unload()
        {
            TextureBorder = null;
            TexturePitch = null;
            TextureOctave = null;
        }

        public virtual Prog NOTE
        {
            get
            {
                return Prog.None;
            }
        }
    }
}