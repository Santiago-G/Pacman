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

            maxSpeed = TimeSpan.FromMilliseconds(85);
            ļSpeed = maxSpeed * 1.2;

            animationLimit = TimeSpan.FromMilliseconds(30);
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

        //Ask if I should keep track of the tiles in the Pacman class or in MainGame, like if I should check for pellets or if a wall is infront of pacman in the class
        //cuz If i do i have to make a lot of stuff public static and idk if thats the right way to go about it



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

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Up))
            {
                pendingDirection = EntityStates.Up;
                pendingRotation = (float)(Math.PI * 1.5);
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                pendingDirection = EntityStates.Right;
                pendingRotation = 0;
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                pendingDirection = EntityStates.Down;
                pendingRotation = (float)(Math.PI * .5);
                movementWindow = true;
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                pendingDirection = EntityStates.Left;
                pendingRotation = (float)(Math.PI);
                movementWindow = true;
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
            

            #region Timer Based Movement

            #endregion
        }


    }
}
