using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Clyde : Ghost
    {
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

        public Clyde(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.Clyde, Coord)
        {
            currGhostState = GhostStates.Chase;
            currDirection = EntityStates.Right;

            scatterTarget = PelletGrid.Instance.gridTiles[0, 30].Position.ToPoint() + new Point(-30, 60);
        }

        public override void Update(GameTime gameTime)
        {
            base.EddenUpdate(gameTime);
        }
    }
}
