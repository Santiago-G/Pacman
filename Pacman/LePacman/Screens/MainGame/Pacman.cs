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
        private Directions pendingDirection;
        private float pendingRotation;
        private bool movementWindow = false;

        private TimeSpan cushionTime = TimeSpan.FromMilliseconds(500);
        private TimeSpan movementCounter;

        public Pacman(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.ClosedPacman, Coord)
        {
            defaultSize = new Point(13);

            maxSpeed = TimeSpan.FromMilliseconds(100);
            ļSpeed = maxSpeed * .8;

            animationLimit = TimeSpan.FromMilliseconds(75);
            animationMin = 0;
            animationMax = 2;

            currDirection = Directions.Right;

            //Max speed
        }

        public PelletTileVisual tileInFront(bool pending)
        {
            int x = Math.Clamp(PendingLocation.X + directions[currDirection].X, 0, 28);
            int y = Math.Clamp(PendingLocation.Y + directions[currDirection].Y, 0, 31);

            if (pending)
            {
                x = Math.Clamp(PendingLocation.X + directions[pendingDirection].X, 0, 28);
                y = Math.Clamp(PendingLocation.Y + directions[pendingDirection].Y, 0, 31);

                return MainGame.pelletGrid[x, y];
            }

            return MainGame.pelletGrid[x, y];
        }

        public PelletTileVisual currPelletTile => MainGame.pelletGrid[PendingLocation.X, PendingLocation.Y];

        //Ask if I should keep track of the tiles in the Pacman class or in MainGame, like if I should check for pellets or if a wall is infront of pacman in the class
        //cuz If i do i have to make a lot of stuff public static and idk if thats the right way to go about it

        //https://gameinternals.com/understanding-pac-man-ghost-behavior
        //https://www.gamedeveloper.com/design/the-pac-man-dossier#close-modal

        private void checkMovementWindow(GameTime gameTime)
        {
            movementCounter += gameTime.ElapsedGameTime;

            if (tileInFront(true).currentState != States.Occupied)
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

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Up))
            {
                pendingDirection = Directions.Up;
                pendingRotation = (float)(Math.PI * 1.5);
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                pendingDirection = Directions.Right;
                pendingRotation = 0;
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                pendingDirection = Directions.Down;
                pendingRotation = (float)(Math.PI * .5);
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                pendingDirection = Directions.Left;
                pendingRotation = (float)(Math.PI);
                movementWindow = true;
            }

            if (movementWindow)
            {
                checkMovementWindow(gameTime);
            }

         

            #region Timer Based Movement

            base.Update(gameTime);

            MainGame.pelletGrid[PreviousLocation.X, PreviousLocation.Y].currentState = States.Empty;
            MainGame.pelletGrid[CurrentLocation.X, CurrentLocation.Y].currentState = States.Debug;

            if (CurrentLocation != PendingLocation)
            {
                ;
            }

            if (timer > ļSpeed)
            {
                if (tileInFront(false).currentState != States.Occupied)
                {
                    PendingLocation += directions[currDirection];
                    animate = true;

                    //29, 32
                    if (PendingLocation.X == 0)
                    {
                        PendingLocation = new Point(MainGame.pelletGrid.GetLength(0) - 1, PendingLocation.Y);
                        PendingLocation = PendingLocation;
                    }
                    else if (PendingLocation.X == MainGame.pelletGrid.GetLength(0) - 1)
                    {
                        PendingLocation = new Point(0, PendingLocation.Y);
                        PendingLocation = PendingLocation;
                    }
                    else if (PendingLocation.Y <= 0)
                    {
                        PendingLocation = new Point(PendingLocation.X, MainGame.pelletGrid.GetLength(1) - 1);
                        PendingLocation = PendingLocation;
                    }
                    else if (PendingLocation.Y >= MainGame.pelletGrid.GetLength(1) - 1)
                    {
                        PendingLocation = new Point(PendingLocation.X, 0);
                        PendingLocation = PendingLocation;
                    }
                }
                else
                {
                    PendingLocation = PendingLocation;
                    animate = false;
                }

                timer = TimeSpan.Zero;
            }
            #endregion
        }


    }
}
