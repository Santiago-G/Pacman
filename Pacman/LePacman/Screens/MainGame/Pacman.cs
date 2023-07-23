using Microsoft.Xna.Framework;
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
        public Pacman(Vector2 Position, Color Tint, Vector2 Scale, Point Coord) : base(Position, Tint, Scale, EntityStates.ClosedPacman, Coord)
        {
            defaultSize = new Point(13);
            speed = TimeSpan.FromMilliseconds(700);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
