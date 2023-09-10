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
        protected override Point currTargetTile
        {
            get
            {
                if (currGhostState == GhostStates.Scatter)
                {
                    return scatterTarget;
                }
                PelletGrid pelletGrid = PelletGrid.Instance;

                Vector2 pacmanTarget = (pelletGrid.pacmanPos + directions[pelletGrid.Pacman.currDirection]).ToVector2(); 

                if (pelletGrid.Pacman.currDirection == EntityStates.Up)
                {
                    pacmanTarget = new Vector2();
                }
                
                // = pelletGrid.pacmanPos + cur

                return PelletGrid.Instance.CoordToPostion(PelletGrid.Instance.pacmanPos).ToPoint();
            }
        }

        public Inky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.Inky, Coord)
        {
            currGhostState = GhostStates.Chase;
            currDirection = EntityStates.Right;

            scatterTarget = PelletGrid.Instance.gridTiles[26, 30].Position.ToPoint() + new Point(30, 60);
        }

        public override void Update(GameTime gameTime)
        {//new Point(29, 32);
            base.EddenUpdate(gameTime);
        }
    }
}
