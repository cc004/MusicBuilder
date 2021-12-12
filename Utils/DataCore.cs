using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MusicBuilder.Tiles;

namespace MusicBuilder.Utils
{
    public struct ExtField
    {
        //instructment, velocity, pitch, lasting 
        public byte pitch, velocity;
    }

    public class DataCore : ModWorld
    {
        public static ExtField[,] extField;
        
        public override void Initialize()
        {
            extField = new ExtField[Main.maxTilesX, Main.maxTilesY];
            //TODO: should be somewhere else
            Noteblock.selection = new Point16(-1, -1);
        }

        public override void Load(TagCompound tag)
        {
            try
            {
                byte[] buffer = tag.Get<byte[]>("musicbuilder");
                for (int i = 0; i < Main.maxTilesX; ++i)
                    for (int j = 0; j < Main.maxTilesY; ++j)
                    {
                        extField[i, j].pitch = buffer[2 * (i + Main.maxTilesX * j)];
                        extField[i, j].velocity = buffer[2 * (i + Main.maxTilesX * j) + 1];
                    }
            }
            catch
            {

            }
        }
        
        public override TagCompound Save()
        {
            try
            {
                byte[] buffer = new byte[5 * Main.maxTilesX * Main.maxTilesY];
                TagCompound tag = new TagCompound();
                for (int i = 0; i < Main.maxTilesX; ++i)
                    for (int j = 0; j < Main.maxTilesY; ++j)
                    {
                        buffer[2 * (i + Main.maxTilesX * j)] = extField[i, j].pitch;
                        buffer[2 * (i + Main.maxTilesX * j) + 1] = extField[i, j].velocity;
                    }
                tag.Set("musicbuilder", buffer);
                return tag;
            }
            catch
            {
                return null;
            }
        }
    }
}
