using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Blinky : Ghost
    {
        //protected override EntityStates EntityState => base.EntityState ^= EntityStates.Shifty;

        protected override Point currTargetTile
        {
            get
            {
                if (currGhostState == GhostStates.Scatter)
                {
                    return scatterTarget;
                }

                return PelletGrid.Instance.CoordToPostion(PelletGrid.Instance.pacmanPos).ToPoint();
            }
        }

        public Blinky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.Blinky, Coord)
        {
            currGhostState = GhostStates.Chase;
            currDirection = EntityStates.Left;

            scatterTarget = PelletGrid.Instance.gridTiles[26, 0].Position.ToPoint() - new Point(0, 60);//new Point(30, 60);
            ////new Point(29, 32);
        }

        public override void Update(GameTime gameTime)
        {
            base.EddenUpdate(gameTime);
        }
    }
}
