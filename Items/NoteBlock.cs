using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MusicBuilder.Registry;
using System;
using Terraria;
using Terraria.ModLoader;

namespace MusicBuilder.Items
{
    public class Noteblock : ModItem
    {
        public override void AddRecipes()
        {
            if (this.NOTE != Prog.None)
            {
                ModRecipe recipe = new ModRecipe(base.mod);
                recipe.SetResult(this, 1);
                recipe.AddIngredient(base.mod, "WireComponentBasic", 1);
                recipe.AddTile(0x72);
                recipe.AddTile(0x8b);
                recipe.AddRecipe();
                recipe = new ModRecipe(base.mod);
                recipe.SetResult(base.mod, "WireComponentBasic", 1);
                recipe.AddIngredient(this, 1);
                recipe.AddTile(0x72);
                recipe.AddRecipe();
            }
        }

        public override bool Autoload(ref string name)
        {
            return this.NOTE != Prog.None;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            spriteBatch.Draw(base.mod.GetTexture("Items/NoteblockBorder"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.noteData[this.NOTE].txt);
            spriteBatch.Draw(base.mod.GetTexture("Items/NoteblockInside"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.noteData[this.NOTE].bgc);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Vector2 position = base.item.Center - Main.screenPosition;
            Vector2 origin = new Vector2(base.item.width * 0.5f, base.item.height * 0.5f);
            SpriteEffects effects = (base.item.direction == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(base.mod.GetTexture("Items/NoteblockBorder"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.noteData[this.NOTE].txt, rotation, origin, scale, effects, 0f);
            Main.spriteBatch.Draw(base.mod.GetTexture("Items/NoteblockInside"), position, new Rectangle(0, 0, 0x10, 0x10), Registries.noteData[this.NOTE].bgc, rotation, origin, scale, effects, 0f);
            return true;
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
            base.item.createTile = base.mod.TileType(Registries.noteData[this.NOTE].name);
            base.item.placeStyle = 0;
        }

        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault(Registries.noteData[this.NOTE].name + " Noteblock");
            base.Tooltip.SetDefault("Right click to increase pitch, hit with a hammer to decrease pitch.\nHolding shift makes it jump by an octave instead.");
        }

        public virtual Prog NOTE => Prog.None;

        public override string Texture => "MusicBuilder/Items/NoteblockBorder";
    }
}
