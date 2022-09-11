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
        //GhostChamber,
        //PacmanSP,
    }

    [Flags]
    public enum billyMan : short
    {
        /*
         * Empty/Alive
         * 
         * 
        */

        isWall = 1,

        Up = 2,
        Down = 4,
        Left = 8,
        Right = 16,

        topLeft = 32,
        topRight = 64,
        BottomRight = 128,
        BottomLeft = 256,

        LoneWall = isWall,

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

        finish this
    }

    public enum States
    {
        Empty,
        Pellet,
        PowerPellet,
        Fruit,
        Wall,
        GhostChamber,
        Pacman,
        NoBackground,
        Occupied
    }

    public enum WallStates
    {
        LoneWall,

        Horiz,
        HorizLeftEnd,
        HorizRightEnd,

        Verti,
        VertiTopEnd,
        VertiBottomEnd,

        TopLeftCorner,
        TopRightCorner,
        BottomRightCorner,
        BottomLeftCorner,

        TopLeftCornerFilled,
        TopRightCornerFilled,
        BottomRightCornerFilled,
        BottomLeftCornerFilled,

        TopEdge,
        RightEdge,
        BottomEdge,
        LeftEdge,

        TopCross,
        RightCross,
        BottomCross,
        LeftCross,

        InteriorWall,
        InteriorCorner,

        TopLeftInteriorFilledCorner,
        TopRightInteriorFilledCorner,
        BottomRightInteriorFilledCorner,
        BottomLeftInteriorFilledCorner,

        Empty
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
