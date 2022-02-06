using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MusicBuilder.Registry;
using System;
using MusicBuilder.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MusicBuilder.Items
{
    public abstract class Noteblock<T> : ModItem where T : ModTile
    {
        public override void AddRecipes()
        {
            var recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wire);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
        
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(TextureContainer.NoteblockBorder.Value, position, new Rectangle(0, 0, 0x10, 0x10), Color.Black);
            spriteBatch.Draw(TextureContainer.NoteblockInside.Value, position, new Rectangle(0, 0, 0x10, 0x10), bgc);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 position = base.Item.Center - Main.screenPosition;
            Vector2 origin = new Vector2(base.Item.width * 0.5f, base.Item.height * 0.5f);
            SpriteEffects effects = (base.Item.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(TextureContainer.NoteblockBorder.Value, position, new Rectangle(0, 0, 0x10, 0x10), Color.Black, rotation, origin, scale, effects, 0f);
            Main.spriteBatch.Draw(TextureContainer.NoteblockInside.Value, position, new Rectangle(0, 0, 0x10, 0x10), bgc, rotation, origin, scale, effects, 0f);
            return true;
        }

        public override void SetDefaults()
        {
            base.Item.width = 0x10;
            base.Item.height = 0x10;
            base.Item.maxStack = 0x3e7;
            base.Item.useTurn = true;
            base.Item.autoReuse = true;
            base.Item.useAnimation = 15;
            base.Item.useTime = 10;
            base.Item.useStyle = 1;
            base.Item.consumable = true;
            base.Item.createTile = ModContent.TileType<T>();
            base.Item.placeStyle = 0;
        }

        private static Color bgc;
        public override void SetStaticDefaults()
        {
            bgc = ColorUtils.ColorHue(new Random(GetType().Name.GetHashCode()).NextDouble());
            base.DisplayName.SetDefault($"{GetType().Name} Noteblock");
            base.Tooltip.SetDefault("Right click to increase pitch, hit with a hammer to decrease pitch.\nHolding shift makes it jump by an octave instead.");
        }

        public virtual Prog NOTE => Prog.None;

        public override string Texture => "MusicBuilder/Items/NoteblockBorder";
    }
}
