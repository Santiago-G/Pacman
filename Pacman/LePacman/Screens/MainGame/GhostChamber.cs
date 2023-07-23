using Microsoft.Xna.Framework;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class GhostChamber : Entity
    {
        public GhostChamber(Vector2 Position, Color Tint, Vector2 Scale) : base(Position, Tint, Scale, EntityStates.GhostChamber, new Point(0))
        {
            defaultSize = new Point(70, 40);
        }
    }
}
