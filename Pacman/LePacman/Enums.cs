using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{

    public enum SelectedType
    {
        Default,
        Pellet,
        PowerPellet,
        Wall,
        Eraser,
    }

    [Flags]
    public enum WallStates : byte
    {
        Empty = 0,
        isWall = 1,

        LoneWall = isWall,

        Up = 2 | LoneWall,
        Down = 4 | LoneWall,
        Left = 8 | LoneWall,
        Right = 16 | LoneWall,

        Horiz = Left | Right,
        HorizLeftEnd = Right,
        HorizRightEnd = Left,

        Verti = Up | Down,
        VertiTopEnd = Down,
        VertiBottomEnd = Up,

        TopLeftCorner = Down | Right,
        TopRightCorner = Down | Left,
        BottomRightCorner = Up | Left,
        BottomLeftCorner = Up | Right,

        TopEdge = Down | Left | Right,
        RightEdge = Left | Up | Down,
        BottomEdge = Up | Left | Right,
        LeftEdge = Right | Up | Down,

        Interior = Up | Down | Left | Right,
    }

    [Flags]
    public enum States
    {
        Empty = 0,
        Occupied = 1,
        Pellet = 2,
        PowerPellet = 4,
        Fruit = 8,
        Wall = 16,
        GhostChamber = 32,
        Pacman = 64,
    }

    public enum GridStates
    {
        WallGrid,
        PixelGrid
    }

    public enum temp
    {
        d90,
        d180,
        d270,
        d360
    }
}
