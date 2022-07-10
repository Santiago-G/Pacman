using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacman
{
    public class wallData : abstractData<(Point, bool)>
    {
        public WallStates WallStates = WallStates.Empty;
        
        public override (Point, bool)[] Neighbors { get; set; }

        public wallData()
        {
            Neighbors = new (Point, bool)[8];
        }
    }
}
