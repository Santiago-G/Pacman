using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman.Screens.MainGame
{
    public class PelletTileVisual : SpriteBase
    {
        public States currentState;
        public Point coord;

        public static Point pacmanPos = new Point(-1000);

        public static int defaultSize => 10;
        public Rectangle SourceRectangle => new Rectangle(Textures[currentState], new Point(defaultSize));
        public Rectangle DestinationRectangle => new Rectangle(Position.ToPoint(), new Point((int)(defaultSize * scale.X)));

        //int changed the scale of the sprite sheet, fix that man

        private Vector2 origin => SourceRectangle.Size.ToVector2() / 2;

        Vector2 scale;

        Dictionary<States, Point> Textures = new Dictionary<States, Point>()
        {
            [States.Empty] = new Point(122, 1),

            [States.Pellet] = new Point(1, 12),
            [States.PowerPellet] = new Point(12, 12),

            [States.Pacman] = new Point(122, 1),
            [States.Fruit] = new Point(122, 1),
            [States.Occupied] = new Point(122, 1),
        };

        public PelletTileVisual(Vector2 Position, Color Tint, States CurrentState, Point Coord, Vector2 Scale) : base(MainGame.spriteSheet, Position, Tint)
        {
            currentState = CurrentState;
            coord = Coord;
            scale = Scale;

            if (coord == pacmanPos)
            {
                currentState = States.Pacman;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, DestinationRectangle, SourceRectangle, Tint, 0, origin, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
