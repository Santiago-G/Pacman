using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Pacman : Entity
    {
        private Directions pendingDirection;
        private float pendingRotation;
        private bool unMomentoDePrince = false;

        private TimeSpan skyWasAllPurpleThereWerePeopleRunningEverywhere = TimeSpan.FromMilliseconds(500);
        private TimeSpan tryingToRunFromTheDestruction_YouKnowIDidntEvenCare;

        public Pacman(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.ClosedPacman, Coord)
        {
            defaultSize = new Point(13);
            speed = TimeSpan.FromMilliseconds(100);

            animationLimit = TimeSpan.FromMilliseconds(75);
            animationMin = 0;
            animationMax = 2;

            currDirection = Directions.Right;
        }

        public PelletTileVisual tileInFront(bool pending)
        {
            if (pending)
            {
                return MainGame.pelletGrid[GridPosition.X + directions[pendingDirection].X, GridPosition.Y + directions[pendingDirection].Y];
            }

            return MainGame.pelletGrid[GridPosition.X + directions[currDirection].X, GridPosition.Y + directions[currDirection].Y];
        }

        public PelletTileVisual currPelletTile => MainGame.pelletGrid[GridPosition.X, GridPosition.Y];

        //Ask if I should keep track of the tiles in the Pacman class or in MainGame, like if I should check for pellets or if a wall is infront of pacman in the class
        //cuz If i do i have to make a lot of stuff public static and idk if thats the right way to go about it

        //also clean up

        private void iWasDreamingWhenIWroteThis_ForgiveMeIfThisGoesAstray(GameTime gameTime)
        {
            tryingToRunFromTheDestruction_YouKnowIDidntEvenCare += gameTime.ElapsedGameTime;

            PelletTileVisual imLiterallyPrince = tileInFront(true);

            if (imLiterallyPrince.currentState != States.Occupied)
            {
                currDirection = pendingDirection;
                Rotation = pendingRotation;
                unMomentoDePrince = false;
            }

            if (tryingToRunFromTheDestruction_YouKnowIDidntEvenCare > skyWasAllPurpleThereWerePeopleRunningEverywhere)
            {
                unMomentoDePrince = false;
                tryingToRunFromTheDestruction_YouKnowIDidntEvenCare = TimeSpan.Zero;
                pendingDirection = currDirection;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            if (currPelletTile.currentState == States.Pellet)
            {
                currPelletTile.currentState = States.Empty;
            }



            if (kb.IsKeyDown(Keys.Up))
            {
                pendingDirection = Directions.Up;
                pendingRotation = (float)(Math.PI * 1.5);
                unMomentoDePrince = true;
            }
            else if (kb.IsKeyDown(Keys.Right))
            {
                pendingDirection = Directions.Right;
                pendingRotation = 0;
                unMomentoDePrince = true;
            }
            else if (kb.IsKeyDown(Keys.Down))
            {
                pendingDirection = Directions.Down;
                pendingRotation = (float)(Math.PI * .5);
                unMomentoDePrince = true;
            }
            else if (kb.IsKeyDown(Keys.Left))
            {
                pendingDirection = Directions.Left;
                pendingRotation = (float)(Math.PI);
                unMomentoDePrince = true;
            }

            if (unMomentoDePrince)
            {
                iWasDreamingWhenIWroteThis_ForgiveMeIfThisGoesAstray(gameTime);
            }

            #region Timer Based Movement
            base.Update(gameTime);

            if (timer > speed)
            {
                if (tileInFront(false).currentState != States.Occupied)
                {
                    GridPosition += directions[currDirection];
                    animate = true;
                }
                else
                {
                    GridPosition = GridPosition;
                    animate = false;
                }

                timer = TimeSpan.Zero;
            }
            #endregion
        }


    }
}
