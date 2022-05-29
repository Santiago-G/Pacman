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

    public enum States
    {
        Empty,
        Pellet,
        PowerPellet,
        Fruit,
        Wall
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
        notAWall
    }

    public enum temp
    {
        d90,
        d180,
        d270,
        d360
    }
}
