using System;
using Terraria;
using Terraria.ModLoader;
using MusicBuilder.Registry;
using MusicBuilder;

namespace MusicBuilder.Utils
{
    [FlagsAttribute]
    public enum WireType
    {
        None = 0,
        Red = 1,
        Blue = 2,
        Green = 4,
        Yellow = 8,
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
            if ((type & WireType.Red) != WireType.None)
                WorldGen.PlaceWire(x, y);
            else
                WorldGen.KillWire(x, y);
            if ((type & WireType.Blue) != WireType.None)
                WorldGen.PlaceWire2(x, y);
            else
                WorldGen.KillWire2(x, y);
            if ((type & WireType.Green) != WireType.None)
                WorldGen.PlaceWire3(x, y);
            else
                WorldGen.KillWire3(x, y);
            if ((type & WireType.Yellow) != WireType.None)
                WorldGen.PlaceWire4(x, y);
            else
                WorldGen.KillWire4(x, y);
        }

        public static void PlaceTimer(int x, int y, Direction type, int delay)
        {
            WorldGen.KillTile(x, y);
            WorldGen.PlaceTile(x, y, ModContainer.instance.GetTile("Delayer" + delay).Type);
            DataCore.extField[x, y].data1 = (byte) type;
        }

        public static void PlaceNoteBlock(int x, int y, Prog program, byte pitch, ushort lasting, byte velocity)
        {
            WorldGen.KillTile(x, y);
            WorldGen.PlaceTile(x, y, ModContainer.instance.GetTile(Registries.noteData[program].name).Type);
            DataCore.extField[x, y].data0 = pitch;
            DataCore.extField[x, y].data1 = (byte) (lasting >> 8);
            DataCore.extField[x, y].data2 = (byte) (lasting & 0xff);
            DataCore.extField[x, y].data3 = velocity;
        }
    }
}