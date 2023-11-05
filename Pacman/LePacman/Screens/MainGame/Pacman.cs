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

        #region Movement

        int startingThreshold;
        int endingTreshold;
        bool horizontal;
        EntityStates diagonalMovement;
        #endregion

        public Pacman(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.ClosedPacman, Coord)
        {
            defaultSize = new Point(13);

            maxSpeed = TimeSpan.FromMilliseconds(200);
            ļSpeed = maxSpeed * 1.2;

            animationLimit = TimeSpan.FromMilliseconds(25);
            animationMin = 0;
            animationMax = 2;

            currDirection = EntityStates.Right;

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
            if (horizontal)
            {
                if (Position.X >= startingThreshold && Position.X <= endingTreshold)
                {
                    diagonal = true;
                    diagonalDirection = pendingDirection;
                    //MODERN LOVE //doing preturns, you'll know what to do...     right?
                }
                else
                {
                    diagonal = false;
                    currDirection = pendingDirection;
                    Rotation = pendingRotation;
                    movementWindow = false;
                }
            }
            else
            {
                if (Position.Y >= startingThreshold && Position.Y <= endingTreshold)
                {
                    diagonal = true;
                    diagonalDirection = pendingDirection;
                    //MODERN LOVE //doing preturns, you'll know what to do...     right?
                }
                else
                {
                    diagonal = false;
                    currDirection = pendingDirection;
                    Rotation = pendingRotation;
                    movementWindow = false;
                }
            }
           

            //movementCounter += gameTime.ElapsedGameTime;

            //if (TileInFront(true).currentState != States.Occupied)
            //{
            //    currDirection = pendingDirection;
            //    Rotation = pendingRotation;
            //    movementWindow = false;
            //}

            //if (movementCounter > cushionTime)
            //{
            //    movementWindow = false;
            //    movementCounter = TimeSpan.Zero;
            //    pendingDirection = currDirection;
            //}
        }


        #endregion

        //https://gameinternals.com/understanding-pac-man-ghost-behavior
        //https://pacman.holenet.info/

        private void CheckTurn ()
        {
            if (currDirection == pendingDirection) { return; }

            if (GridPosition == GridPosition + directions[currDirection] + directions[pendingDirection]) //reverse direction
            {
                return;
            }

            //FOR NOW: Check if the tile forward in the currDirection and one to the pending direction is empty. If it is, turn.

            int x = Math.Clamp(GridPosition.X + directions[currDirection].X + directions[pendingDirection].X, 0, 28);
            int y = Math.Clamp(GridPosition.Y + directions[currDirection].Y + directions[pendingDirection].Y, 0, 31);

            if (MainGame.pelletGrid[x, y].currentState == States.Empty)
            {
                x = Math.Clamp(GridPosition.X + directions[currDirection].X, 0, 28);
                y = Math.Clamp(GridPosition.Y + directions[currDirection].Y, 0, 31);
                horizontal = true;
                endingTreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.X;

                switch (currDirection)
                {
                    case EntityStates.Left:
                        startingThreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.X + 4 * 3;
                        break;
                    case EntityStates.Right:
                        startingThreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.X - 3 * 3;
                        endingTreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.X;
                        break;
                    case EntityStates.Up:
                        startingThreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.Y;
                        endingTreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.X + 4 * 3;

                        horizontal = false;
                        break;
                    case EntityStates.Down:
                        startingThreshold = MainGame.pelletGrid[x, y].DestinationRectangle.Center.Y - 4 * 3;

                        horizontal = false;
                        break;
                }

                movementWindow = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Up))
            {
                pendingDirection = EntityStates.Up;
                pendingRotation = (float)(Math.PI * 1.5);
                
                CheckTurn();
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                pendingDirection = EntityStates.Right;
                pendingRotation = 0;

                CheckTurn();
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                pendingDirection = EntityStates.Down;
                pendingRotation = (float)(Math.PI * .5);

                CheckTurn();
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                pendingDirection = EntityStates.Left;
                pendingRotation = (float)(Math.PI);

                CheckTurn();
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
