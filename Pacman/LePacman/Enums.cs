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

        TopRight = 32 | LoneWall,
        BottomRight = 64 | LoneWall,
        BottomLeft = 128 | LoneWall,
        TopLeft = 256 | LoneWall,
        //CornerMasker = ~(32 | 64 | 128 | 256),

        //////////////

        OuterWall = 512 | isWall,

        OuterUp = Up | OuterWall,
        OuterDown = Down | OuterWall,
        OuterLeft = Left | OuterWall,
        OuterRight = Right | OuterWall,

        OuterVerti = OuterUp | OuterDown,
        OuterHoriz = OuterRight | OuterLeft,

        TopLeftCornerOW = OuterDown | OuterRight,
        TopRightCornerOW = OuterDown | OuterLeft,
        BottomRightCornerOW = OuterUp | OuterLeft,
        BottomLeftCornerOW = OuterUp | OuterRight,

        TopIntersectingOW = OuterWall | TopEdge,
        RightIntersectingOW = OuterWall | RightEdge,
        BottomIntersectingOW = OuterWall | BottomEdge,
        LeftIntersectingOW = OuterWall | LeftEdge,


        TopLeftIntersectingOW = TopIntersectingOW | BottomRight,
        TopRightIntersectingOW = TopIntersectingOW | BottomLeft,

        RightTopIntersectingOW = RightIntersectingOW | BottomLeft,
        RightBottomIntersectingOW = RightIntersectingOW | TopLeft,

        BottomLeftIntersectingOW = BottomIntersectingOW | TopRight,
        BottomRightIntersectingOW = BottomIntersectingOW | TopLeft,

        LeftTopIntersectingOW = LeftIntersectingOW | BottomRight,
        LeftBottomIntersectingOW = LeftIntersectingOW | TopRight,


        MiddleTopIntersectingOW = TopIntersectingOW | BottomLeft | BottomRight,
        MiddleRightIntersectingOW = RightIntersectingOW | TopLeft | BottomLeft,
        MiddleBottomIntersectingOW = BottomIntersectingOW | TopLeft | TopRight,
        MiddleLeftIntersectingOW = LeftIntersectingOW | TopRight | BottomRight,
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
        Error
    }

    public enum GridStates
    {
        WallGrid,
        PixelGrid
    }

    public enum GameStates
    {
        TitleScreen,

        Options,
        OptionsAudio,
        OptionsVisual,
        OptionsControl,

        MapEditor,
        MainGame,
    }

    public enum PopUpStates 
    {
        None,
        PortalError,
        PortalValid,
    }
}
