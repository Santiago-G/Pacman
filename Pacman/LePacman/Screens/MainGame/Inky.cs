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
        public override Point currTargetTile
        {
            get
            {
                if (currGhostState == GhostStates.Scatter)
                {
                    return scatterTarget;
                }
                PelletGrid pelletGrid = PelletGrid.Instance;

                Vector2 pacmanTarget = pelletGrid.Blinky.Position;
                Vector2 linearDistance;


                if (pelletGrid.Pacman.currDirection == EntityStates.Up)
                {
                    pacmanTarget = pelletGrid.CoordToPostion(pelletGrid.pacmanPos - new Point(2, 2));
                }
                else
                {
                    pacmanTarget = pelletGrid.CoordToPostion(pelletGrid.pacmanPos + (directions[pelletGrid.Pacman.currDirection] * new Point(2)));
                }

                linearDistance = new Vector2(MathHelper.Distance(pelletGrid.Blinky.Position.X, pacmanTarget.X),
                    MathHelper.Distance(pelletGrid.Blinky.Position.Y, pacmanTarget.Y));

                Point JoeTheLion = (pelletGrid.Blinky.Position + (linearDistance * 2)).ToPoint();

                return JoeTheLion;
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
