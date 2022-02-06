using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace MusicBuilder
{
    internal static class TextureContainer
    {
        public static readonly Asset<Texture2D> NoteblockBorder = ModContent.Request<Texture2D>("MusicBuilder/Items/NoteblockBorder");
        public static readonly Asset<Texture2D> NoteblockInside = ModContent.Request<Texture2D>("MusicBuilder/Items/NoteblockInside");
        public static readonly Asset<Texture2D> TextureBorder = ModContent.Request<Texture2D>("MusicBuilder/Tiles/NoteblockBorder");
        public static readonly Asset<Texture2D> TexturePitch = ModContent.Request<Texture2D>("MusicBuilder/Tiles/NoteblockPitch");
        public static readonly Asset<Texture2D> TextureOctave = ModContent.Request<Texture2D>("MusicBuilder/Tiles/NoteblockOctave");
    }

}
