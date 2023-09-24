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
        /*
         * Whenever Clyde needs to determine his target tile, he first calculates his distance from Pac-Man. 
         * If he is farther than eight tiles away, his targeting is identical to Blinky's, using Pac-Man's current tile as his target.
         * However, as soon as his distance to Pac-Man becomes less than eight tiles, Clyde's target is set to the same tile as his fixed one in Scatter mode,
         * just outside the bottom-left corner of the maze.
         * 
        */
        public override Point currTargetTile
        {
            get
            {
                if (currGhostState == GhostStates.Scatter)
                {
                    return scatterTarget;
                }

                PelletGrid pelletGrid = PelletGrid.Instance;

                //find distance between 8 tiles

                if (Vector2.Distance(pelletGrid.Pacman.Position, Position) > EightTileDistance)
                {
                    return PelletGrid.Instance.CoordToPostion(PelletGrid.Instance.pacmanPos).ToPoint();
                }

                return scatterTarget;
            }
        }

        private float EightTileDistance => PelletGrid.Instance.CoordToPostion(new Point(7, 0)).X - PelletGrid.Instance.CoordToPostion(new Point(0)).X;

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
