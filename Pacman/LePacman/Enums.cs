using System;

namespace Pacman
{

    public enum SelectedType
    {
        Default,
        Pellet,
        PowerPellet,
        Fruit,
        Wall,
        OuterWall,
        Eraser,
        AltEraser,
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

        Error = 1024,

        ErrorOuterUp = Error | OuterUp,
        ErrorOuterDown = Error | OuterDown,
        ErrorOuterLeft = Error | OuterLeft,
        ErrorOuterRight = Error | OuterRight,

        ErrorOuterVerti = Error | OuterVerti,
        ErrorOuterHoriz = Error | OuterHoriz,

    }

    public enum States
    {
        Empty,
        Pellet,
        PowerPellet,
        Fruit,
        Pacman,
        Occupied,
        Wall,
        GhostChamber,
    }

    [Flags]
    public enum EntityStates
    {
        ClosedPacman,
        Pacman,
        OpenPacman,

        Blinky,
        Pinky,
        Inky,
        Clyde,
        GhostChamber,

        Left = 8,
        Right = 16,
        Up = 32,
        Down = 64,

        BlinkyUp = Blinky | Up,
        BlinkyRight = Blinky | Right,
        BlinkyDown = Blinky | Down,
        BlinkyLeft = Blinky | Left,

        PinkyUp = Pinky | Up,
        PinkyRight = Pinky | Right,
        PinkyDown = Pinky | Down,
        PinkyLeft = Pinky | Left,

        InkyUp = Inky | Up,
        InkyRight = Inky | Right,
        InkyDown = Inky | Down,
        InkyLeft = Inky | Left,

        ClydeUp = Clyde | Up,
        ClydeRight = Clyde | Right,
        ClydeDown = Clyde | Down,
        ClydeLeft = Clyde | Left,

        Shifty = 128,

        BlinkyUpShifty = BlinkyUp | Shifty,
        BlinkyRightShifty = BlinkyRight | Shifty,
        BlinkyDownShifty = BlinkyDown | Shifty,
        BlinkyLeftShifty = BlinkyLeft | Shifty,

        PinkyUpShifty = PinkyUp | Shifty,
        PinkyRightShifty = PinkyRight | Shifty,
        PinkyDownShifty = PinkyDown | Shifty,
        PinkyLeftShifty = PinkyLeft | Shifty,

        InkyUpShifty = InkyUp | Shifty,
        InkyRightShifty = InkyRight | Shifty,
        InkyDownShifty = InkyDown | Shifty,
        InkyLeftShifty = InkyLeft | Shifty,

        ClydeUpShifty = ClydeUp | Shifty,
        ClydeRightShifty = ClydeRight | Shifty,
        ClydeDownShifty = ClydeDown | Shifty,
        ClydeLeftShifty = ClydeLeft | Shifty,
    }

    public enum Directions
    {
        Up, Down, Left, Right,
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

        SaveMap,

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
