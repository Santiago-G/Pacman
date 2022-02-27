using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class MapEditor : Screen
    {
        Vector2 position;
        GraphicsDeviceManager graphics;

        Graph<>

        Image mapEditorImage;
        Texture2D mapEditorSprite;

        public MapEditor((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            graphics = Graphics;
        }

        public override void LoadContent(ContentManager Content)
        {
            mapEditorSprite = Content.Load<Texture2D>("mapEditorText");
            mapEditorImage = new Image(mapEditorSprite, new Vector2(800 - mapEditorSprite.Width/2 , 10), Color.White);
            objects.Add(mapEditorImage);
        }

        
    }
}
