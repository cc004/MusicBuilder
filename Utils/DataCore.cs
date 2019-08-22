using System.IO;
using Terraria;
using Terraria.Utilities;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MusicBuilder.Utils
{
    public struct ExtField
    {
        //instructment, velocity, pitch, lasting 
        public byte data0, data1, data2, data3, data4;
    }

    public class DataCore : ModWorld
    {
        public static DataCore instance = new DataCore();
        public static ExtField[,] extField;
        
        public override void Initialize()
        {
            extField = new ExtField[Main.maxTilesX, Main.maxTilesY];
        }

        public override void Load(TagCompound tag)
        {
            string path = Path.ChangeExtension(Main.ActiveWorldFileData.Path, ".mb");
            if (!FileUtilities.Exists(path, false)) return;
            try
            {
                byte[] buffer = new byte[5 * Main.maxTilesX * Main.maxTilesY];
                buffer = FileUtilities.ReadAllBytes(path, false);
                FileStream fs = new FileStream(path, FileMode.Open);
                for (int i = 0; i < Main.maxTilesX; ++i)
                    for (int j = 0; j < Main.maxTilesY; ++j)
                    {
                        extField[i, j].data0 = buffer[5 * (i + Main.maxTilesX * j)];
                        extField[i, j].data1 = buffer[5 * (i + Main.maxTilesX * j) + 1];
                        extField[i, j].data2 = buffer[5 * (i + Main.maxTilesX * j) + 2];
                        extField[i, j].data3 = buffer[5 * (i + Main.maxTilesX * j) + 3];
                        extField[i, j].data4 = buffer[5 * (i + Main.maxTilesX * j) + 4];
                    }
            }
            catch
            {

            }
        }
        
        public override TagCompound Save()
        {
            string path = Path.ChangeExtension(Main.ActiveWorldFileData.Path, ".mb");
            //TODO: Delete .mb file when the world is deleted in the game.
            if (FileUtilities.Exists(path, false))
                FileUtilities.Copy(path, path + ".bak", false);
            try
            {
                byte[] buffer = new byte[5 * Main.maxTilesX * Main.maxTilesY];
                for (int i = 0; i < Main.maxTilesX; ++i)
                    for (int j = 0; j < Main.maxTilesY; ++j)
                    {
                        buffer[5 * (i + Main.maxTilesX * j)] = extField[i, j].data0;
                        buffer[5 * (i + Main.maxTilesX * j) + 1] = extField[i, j].data1;
                        buffer[5 * (i + Main.maxTilesX * j) + 2] = extField[i, j].data2;
                        buffer[5 * (i + Main.maxTilesX * j) + 3] = extField[i, j].data3;
                        buffer[5 * (i + Main.maxTilesX * j) + 4] = extField[i, j].data4;
                    }
                FileUtilities.WriteAllBytes(path, buffer, false);
            }
            catch
            {
                return null;
            }
            return new TagCompound();
        }
    }
}
