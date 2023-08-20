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
        public Blinky(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.BlinkyLeftShifty, Coord, new Point(26,-4))
        {
            currGhostState = GhostStates.Scatter;

            currDirection = Directions.Left;

            animationMin = (int)EntityStates.BlinkyLeft;
            animationMax = (int)EntityStates.BlinkyLeftShifty;
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
