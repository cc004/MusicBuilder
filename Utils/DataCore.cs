using System.IO;
using MusicBuilder.Commands;
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

    public class DataCore : ModSystem
    {
        public static ExtField[,] extField;
        
        public override void OnWorldLoad()
        {
            extField = new ExtField[Main.maxTilesX, Main.maxTilesY];
            //TODO: should be somewhere else
            MyCommand.selection = new Point16(-1, -1);
        }
        
        public override void LoadWorldData(TagCompound tag)
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
        
        public override void SaveWorldData(TagCompound tag)
        {
            try
            {
                byte[] buffer = new byte[5 * Main.maxTilesX * Main.maxTilesY];
                for (int i = 0; i < Main.maxTilesX; ++i)
                    for (int j = 0; j < Main.maxTilesY; ++j)
                    {
                        buffer[2 * (i + Main.maxTilesX * j)] = extField[i, j].pitch;
                        buffer[2 * (i + Main.maxTilesX * j) + 1] = extField[i, j].velocity;
                    }
                tag.Set("musicbuilder", buffer);
            }
            catch
            {
            }
        }
    }
}
