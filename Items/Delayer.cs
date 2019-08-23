using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MusicBuilder.Registry;
using System;
using Terraria;
using Terraria.ModLoader;

namespace MusicBuilder.Items
{
    public class Delayer : ModItem
    {
        public override bool Autoload(ref string name)
        {
            return DELAY != -1;
        }
        public override void AddRecipes() {}

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(base.mod.GetTexture("Items/DelayerBorder"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.delayData[this.DELAY].bgc);
            spriteBatch.Draw(base.mod.GetTexture("Items/DelayerInside"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.delayData[this.DELAY].lit);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 position = base.item.Center - Main.screenPosition;
            Vector2 origin = new Vector2(base.item.width * 0.5f, base.item.height * 0.5f);
            SpriteEffects effects = (base.item.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(base.mod.GetTexture("Items/DelayerBorder"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.delayData[this.DELAY].bgc, rotation, origin, scale, effects, 0f);
            Main.spriteBatch.Draw(base.mod.GetTexture("Items/DelayerInside"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.delayData[this.DELAY].lit, rotation, origin, scale, effects, 0f);
            return false;
        }

        public override void SetDefaults()
        {
            base.item.width = 0x10;
            base.item.height = 0x10;
            base.item.maxStack = 0x3e7;
            base.item.useTurn = true;
            base.item.autoReuse = true;
            base.item.useAnimation = 15;
            base.item.useTime = 10;
            base.item.useStyle = 1;
            base.item.consumable = true;
            base.item.createTile = base.mod.TileType("Delayer" + this.DELAY);
            base.item.placeStyle = 0;
        }

        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("Delayer (" + this.DELAY + ")");
            base.Tooltip.SetDefault("Right click to change facing direction.");
        }

        public virtual int DELAY
        {
            get
            {
                return -1;
            }
        }

        public override string Texture
        {
            get
            {
                return "MusicBuilder/Items/DelayerBorder";
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