using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Pinky : Ghost
    {//, new Point(2, -4)
        public override Point currTargetTile
        {
            get
            {
                if (currGhostState == GhostStates.Scatter)
                {
                    return scatterTarget;
                }

                PelletGrid pelletGrid = PelletGrid.Instance;

                if (pelletGrid.Pacman.currDirection == EntityStates.Up)
                {
                    return pelletGrid.CoordToPostion(pelletGrid.pacmanPos - new Point(4, 4)).ToPoint();
                }

                return pelletGrid.CoordToPostion(pelletGrid.pacmanPos + (directions[pelletGrid.Pacman.currDirection] * new Point(4))).ToPoint();
            }
        }

        public Pinky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.Pinky, Coord)
        {
            currGhostState = GhostStates.Chase;
            currDirection = EntityStates.Left;

            scatterTarget = PelletGrid.Instance.gridTiles[2, 0].Position.ToPoint() - new Point(0, 60);
        }

        public override void Update(GameTime gameTime)
        {
            base.EddenUpdate(gameTime);
        }
    }
}
