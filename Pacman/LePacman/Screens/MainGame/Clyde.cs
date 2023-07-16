using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class Clyde : Entity
    {
        public Clyde(Vector2 Position, Color Tint, Vector2 Scale) : base(Position, Tint, Scale, EntityStates.ClydeDown)
        {
        }
    }
}
