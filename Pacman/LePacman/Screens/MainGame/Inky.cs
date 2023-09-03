using Microsoft.Xna.Framework;
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
        {//new Point(29, 32);
         //new Point(29, 34)
            currGhostState = GhostStates.Scatter;

            currDirection = Directions.Left;

            animationMin = (int)EntityStates.BlinkyLeft;
            animationMax = (int)EntityStates.BlinkyLeftShifty;

            scatterTarget = PelletGrid.Instance.gridTiles[27, 30].Position.ToPoint() + new Point(30, 60);//new Point(30, 60);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
