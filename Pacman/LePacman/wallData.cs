using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;



namespace Pacman
{
    using Neighbor = System.ValueTuple<Point, bool>;
   
    public class wallData : AbstractData<(Point Index, bool isWall)>
    {
        public WallStates WallState = WallStates.Empty;
        
        public override (Point Index, bool isWall)[] Neighbors { get; set; }

        public wallData()
        {           
            Neighbors = new (Point, bool)[8];
        }
    }
}
