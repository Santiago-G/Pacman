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
        protected Point currTargetTile;
        protected Point scatterTarget;

        public static GhostStates currGhostState;

        //the change between ghost states is gonna be in MainGame based on a timer and if pacman ate power pellet

        public Ghost(Vector2 Position, Color Tint, Vector2 Scale, EntityStates EntityState, Point Coord/*, Point ScatterTarget*/) : base(Position, Tint, Scale, EntityState, Coord)
        {
            defaultSize = new Point(14);
        }

        public void Update(GameTime gameTime, Point pacmanPosition)
        {
            //they're thing for movement should be a stack of directions that get popped into pending direction
            //this stack should be updated every movement tick (timer)
            Update(pacmanPosition);
            base.Update(gameTime);

            if (timer > ļSpeed)
            {


                timer = TimeSpan.Zero;
            }
        }

        public abstract void Update(Point PacmanPosition);
    }
}
