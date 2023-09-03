using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Blinky : Ghost
    {
        protected override int animationMin
        {
            get 
            {
                switch (currDirection)
                {
                    case Directions.Up:
                        return (int)EntityStates.BlinkyUp;
                        break;
                    case Directions.Down:
                        return (int)EntityStates.BlinkyDown;
                        break;
                    case Directions.Left:
                        return (int)EntityStates.BlinkyLeft;
                        break;
                    case Directions.Right:
                        return (int)EntityStates.BlinkyRight;
                        break;
                    default:
                        return -1;
                        break;
                }
            }
        }

        protected override int animationMax
        {
            get
            {
                switch (currDirection)
                {
                    case Directions.Up:
                        return (int)EntityStates.BlinkyUpShifty;
                        break;
                    case Directions.Down:
                        return (int)EntityStates.BlinkyDownShifty;
                        break;
                    case Directions.Left:
                        return (int)EntityStates.BlinkyLeftShifty;
                        break;
                    case Directions.Right:
                        return (int)EntityStates.BlinkyRightShifty;
                        break;
                    default:
                        return -1;
                        break;
                }
            }
        }

        public Blinky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.BlinkyLeftShifty, Coord)
        {
            currGhostState = GhostStates.Scatter;

            currDirection = Directions.Left;

            //animationMin = (int)EntityStates.BlinkyLeft;
            //animationMax = (int)EntityStates.BlinkyLeftShifty;

            scatterTarget = PelletGrid.Instance.gridTiles[26, 0].Position.ToPoint() - new Point(30, 60);//new Point(30, 60);
            ////new Point(29, 32);
        }

        protected override void AnimationLogic()
        {
            if (animationTimer > animationLimit)
            {
                if (entityState.HasFlag(EntityStates.Shifty))
                {
                    entityState &= ~EntityStates.Shifty;
                }
                else
                {
                    entityState |= EntityStates.Shifty;
                }

                do animations

                animationTimer = TimeSpan.Zero;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Point PacmanPosition = PelletGrid.Instance.pacmanPos;
            base.EddenUpdate(gameTime);

        }
    }
}
