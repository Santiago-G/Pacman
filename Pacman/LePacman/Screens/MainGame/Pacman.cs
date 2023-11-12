using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Pacman : Entity
    {
        private EntityStates pendingDirection;
        private float pendingRotation;
        private bool movementWindow = false;

        private TimeSpan cushionTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan movementCounter;

        public int freezeFrameCounter = 0;

        public TimeSpan normalSpeed;
        public TimeSpan frightSpeed;

        public Pacman(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.ClosedPacman, Coord)
        {
            defaultSize = new Point(13);

            maxSpeed = TimeSpan.FromMilliseconds(90);
            ļSpeed = maxSpeed * 1.2;

            animationLimit = TimeSpan.FromMilliseconds(25);
            animationMin = 0;
            animationMax = 2;

            currDirection = EntityStates.Right;
            startingPostion = Position;
            //Max speed

            PelletGrid.Instance.Pacman = this;
        }

        #region functions
        protected override bool NextPositionValid()
        {
            return TileInFront(false).currentState != States.Occupied;
        }

        public PelletTileVisual TileInFront(bool pending)
        {
            int x = Math.Clamp(GridPosition.X + directions[currDirection].X, 0, 28);
            int y = Math.Clamp(GridPosition.Y + directions[currDirection].Y, 0, 31);

            if (pending)
            {
                x = Math.Clamp(GridPosition.X + directions[pendingDirection].X, 0, 28);
                y = Math.Clamp(GridPosition.Y + directions[pendingDirection].Y, 0, 31);

                return MainGame.pelletGrid[x, y];
            }

            return MainGame.pelletGrid[x, y];
        }

        public PelletTileVisual currPelletTile => MainGame.pelletGrid[GridPosition.X, GridPosition.Y];


        private void CheckMovementWindow(GameTime gameTime)
        {
            movementCounter += gameTime.ElapsedGameTime;

            if (TileInFront(true).currentState != States.Occupied)
            {
                currDirection = pendingDirection;
                Rotation = pendingRotation;
                movementWindow = false;
            }

            if (movementCounter > cushionTime)
            {
                movementWindow = false;
                movementCounter = TimeSpan.Zero;
                pendingDirection = currDirection;
            }
        }


        #endregion

        //https://gameinternals.com/understanding-pac-man-ghost-behavior
        //https://pacman.holenet.info/

        private void CheckTurn (EntityStates pendingDirection)
        {
            if (currDirection == pendingDirection)  return;
            if (MainGame.pelletGrid[gridPos.X + directions[pendingDirection].X, gridPos.Y + directions[pendingDirection].Y].currentState == States.Occupied) return;

            startingPostion = DestinationRectangle.Center.ToVector2();

            PelletGrid mapGrid = PelletGrid.Instance;

            //diagonalOffset = ;
            

            movementWindow = true;
            
        }

        //if the player presses a key that isnt the current direction
        //check how far behind or forward the center of pacman is from the center of a tile. (do tile.width/8)
        //

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Up))
            {
                pendingRotation = (float)(Math.PI * 1.5);

                CheckTurn(pendingDirection = EntityStates.Up);
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                pendingRotation = 0;

                CheckTurn(pendingDirection = EntityStates.Right);
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                pendingRotation = (float)(Math.PI * .5);

                CheckTurn(pendingDirection = EntityStates.Down);
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                pendingRotation = (float)(Math.PI);

                CheckTurn(pendingDirection = EntityStates.Left);
            }
           

            if (movementWindow)
            {
                CheckMovementWindow(gameTime);
            }

            if (freezeFrameCounter <= 0)
            {
                EddenUpdate(gameTime);
            }
            else 
            {
                freezeFrameCounter--;
            }
            
        }


    }
}
