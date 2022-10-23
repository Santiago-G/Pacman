﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;



namespace Pacman
{
    public class wallData : AbstractData
    {
        public WallStates WallState = WallStates.Empty;
        
        public override Point[] Neighbors { get; set; }

        public wallData()
        {           
            Neighbors = new Point[8];
        }
    }
}
