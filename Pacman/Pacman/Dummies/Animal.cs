using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pacman.Dummies
{
    public class Animal
    {
        [JsonIgnore]
        public Texture2D Image { get; set; }
    }
}
