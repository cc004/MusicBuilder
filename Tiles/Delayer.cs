using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MusicBuilder;
using MusicBuilder.Utils;
using MusicBuilder.Registry;
using System;
using System.Reflection;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MusicBuilder.Tiles
{
    public class Delayer : ModTile
    {
        
        private static int[,] direction = new int[,]{{0, -1}, {1, 0}, {0, 1}, {-1, 0}};
        public static Texture2D TextureBorder;
        public static Texture2D TextureInside;

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "MusicBuilder/Tiles/DelayerBorder";
            return DELAY != -1;
        }

        public override void RightClick(int i, int j)
        {
            if (!Main.player[Main.myPlayer].mouseInterface)
            {
                byte num;
                DataCore.extField[i, j].data1 = (byte) ((num = DataCore.extField[i, j].data1) + 1);
                if (num >= 4)
                {
                    DataCore.extField[i, j].data1 = 1;
                }
            }
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void HitWire(int i, int j)
        {
            if (DataCore.extField[i, j].data1 == 0) return;

            Scheduler.Schedule(i, j, this.DELAY, delegate(int x, int y, object param)
            {
                byte dir = (byte) param;
                for (int k = 0; k < 4; ++k)
                {
                    if (((k + 3 - dir) & 0x03) == 0) continue;

                    int xt = x + direction[k, 0], yt = y + direction[k, 1];
                    Wiring.TripWire(xt, yt, 1, 1);
                    if (k + 1 == dir && Registries.delayers.Contains(Main.tile[xt, yt].type))
                        TileLoader.HitWire(xt, yt, Main.tile[xt, yt].type);
                }
            }, DataCore.extField[i, j].data1);
        }

        public static void Load()
        {
            TextureBorder = ModContainer.instance.GetTexture("Tiles/DelayerBorder");
            TextureInside = ModContainer.instance.GetTexture("Tiles/DelayerInside");
        }

        public override void PostSetDefaults()
        {
            Main.tileNoSunLight[base.Type] = false;
            Registries.delayers.Add(base.Type);
        }

        public override void SetDefaults()
        {
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            base.disableSmartCursor = true;
            Main.tileFrameImportant[base.Type] = true;
            Main.tileSolid[base.Type] = true;
            base.drop = base.mod.ItemType("Delayer" + this.DELAY);
        }

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            Color color = ColorUtils.ColorBlend(Registries.delayData[DataCore.extField[i, j].data0].lit, new Color(0, 0, 0), Scheduler.GetProgress(i, j));
            r = color.R / 256.0f;
            g = color.G / 256.0f;
            b = color.B / 256.0f;
		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2((float) (((i * 0x10) - ((int) Main.screenPosition.X)) + Main.offScreenRange), (float) (((j * 0x10) - ((int) Main.screenPosition.Y)) + Main.offScreenRange));
            DelayData data = Registries.delayData[DataCore.extField[i, j].data0];
            Color c = ColorUtils.ColorBlend(data.lit, data.off, Scheduler.GetProgress(i, j));
            spriteBatch.Draw(TextureBorder, position, Lighting.GetColor(i, j, data.bgc));
            spriteBatch.Draw(TextureInside, position, new Rectangle(0x12 * DataCore.extField[i, j].data1, 0, 0x10, 0x10), Lighting.GetColor(i, j, c));
            return false;
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            if (DataCore.extField[i, j].data1 == 0)
                if (DataCore.extField[i, j].data0 != ((byte) this.DELAY))
                    DataCore.extField[i, j].data0 = (byte) this.DELAY;
            return true;
        }
        public static void Unload()
        {
            TextureBorder = null;
            TextureInside = null;
        }

        public virtual int DELAY
        {
            get
            {
                return -1;
            }
        }
    }

    public class Delayer0 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 0;
            }
        }
    }

    public class Delayer1 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 1;
            }
        }
    }

    public class Delayer2 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 2;
            }
        }
    }

    public class Delayer3 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 3;
            }
        }
    }

    public class Delayer4 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 4;
            }
        }
    }

    public class Delayer5 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 5;
            }
        }
    }

    public class Delayer6 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 6;
            }
        }
    }

    public class Delayer7 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 7;
            }
        }
    }

    public class Delayer8 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 8;
            }
        }
    }

    public class Delayer9 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 9;
            }
        }
    }

    public class Delayer10 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 10;
            }
        }
    }

    public class Delayer11 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 11;
            }
        }
    }

    public class Delayer12 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 12;
            }
        }
    }

    public class Delayer13 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 13;
            }
        }
    }

    public class Delayer14 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 14;
            }
        }
    }

    public class Delayer15 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 15;
            }
        }
    }

    public class Delayer16 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 16;
            }
        }
    }
    
    public class Delayer32 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 32;
            }
        }
    }

    public class Delayer64 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 64;
            }
        }
    }

    public class Delayer128 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 128;
            }
        }
    }

    public class Delayer256 : Delayer
    {
        public override int DELAY
        {
            get
            {
                return 256;
            }
        }
    }

}