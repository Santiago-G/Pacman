using System;

namespace Pacman
{

    public enum SelectedType
    {
        Default,
        Pellet,
        PowerPellet,
        Wall,
        OuterWall,
        Eraser,
    }

    [Flags]
    public enum WallStates
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

        //////////////

        OuterWall = 32 | isWall,

        OuterUp = Up | OuterWall,
        OuterDown = Down | OuterWall,
        OuterLeft = Left | OuterWall,
        OuterRight = Right | OuterWall,

        OuterVerti = OuterUp | OuterDown,
        OuterHoriz = OuterRight | OuterLeft,

        OuterTopLeftCorner = OuterDown | OuterRight,
        OuterTopRightCorner = OuterDown | OuterLeft,
        OuterBottomRightCorner = OuterUp | OuterLeft,
        OuterBottomLeftCorner = OuterUp | OuterRight,

        TopIntersectingOuterWall = OuterWall | TopEdge,
        RightIntersectingOuterWall = OuterWall | RightEdge,
        BottomIntersectingOuterWall = OuterWall | BottomEdge,
        LeftIntersectingOuterWall = OuterWall | LeftEdge,

    }

    [Flags]
    public enum OuterWallStates
    {






    }

    public enum States
    {
        Empty,
        Occupied,
        Pellet,
        PowerPellet,
        Fruit,
        Wall,
        GhostChamber,
        Pacman,
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
