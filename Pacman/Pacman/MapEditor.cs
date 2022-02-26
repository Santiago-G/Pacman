using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class MapEditor : Screen
    {
        (int, int) size;
        Vector2 position;
        GraphicsDeviceManager graphics;

        public MapEditor((int, int) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;

            //set size
        }

        public override void LoadContent(ContentManager Content)
        {
            
        }

        
    }
}
