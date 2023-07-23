﻿using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Inky : Ghost
    {
        public Inky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.InkyUpShifty, Coord)
        {
        }

        public override void Update(Point PacmanPosition)
        {
            throw new NotImplementedException();
        }
    }
}
