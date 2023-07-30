using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public abstract class Ghost : Entity
    {
        public Ghost(Vector2 Position, Color Tint, Vector2 Scale, EntityStates EntityState, Point Coord) : base(Position, Tint, Scale, EntityState, Coord)
        {
            defaultSize = new Point(14);
        }

        public void Update(GameTime gameTime, Point pacmanPosition)
        {
            //they're thing for movement should be a stack of directions that get popped into pending direction
            //this stack should be updated every movement tick (timer)
            Update(pacmanPosition);
            Update(gameTime);
        }

        public abstract void Update(Point PacmanPosition);
    }
}
