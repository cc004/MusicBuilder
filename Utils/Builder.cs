using System;
using Terraria;
using Terraria.ModLoader;
using MusicBuilder.Registry;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    public enum WireType
    {
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
    }

    public enum Direction
    {
        Up = 1,
        Down = 3,
        Left = 4,
        Right = 2
    }

    public static class Builder
    {
        public static void PlaceWire(int x, int y, WireType type)
        {
            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();
            switch(type)
            {
                case WireType.Red: Main.tile[x, y].wire(true);break;
                case WireType.Blue: Main.tile[x, y].wire2(true);break;
                case WireType.Green: Main.tile[x, y].wire3(true);break;
                case WireType.Yellow: Main.tile[x, y].wire4(true);break;
            }
        }

        public static void PlaceTimer(int x, int y, Direction type, int delay)
        {
            WorldGen.PlaceTile(x, y, ModContainer.instance.GetTile("Delayer" + delay).Type);
            DataCore.extField[x, y].data1 = (byte) type;
        }

        public static void PlaceNoteBlock(int x, int y, Prog program, byte pitch, ushort lasting, byte velocity)
        {
            WorldGen.PlaceTile(x, y, ModContainer.instance.GetTile(Registries.noteData[program].name).Type);
            DataCore.extField[x, y].data0 = pitch;
            DataCore.extField[x, y].data1 = (byte) (lasting >> 8);
            DataCore.extField[x, y].data2 = (byte) (lasting & 0xff);
            DataCore.extField[x, y].data3 = velocity;
        }
    }
}