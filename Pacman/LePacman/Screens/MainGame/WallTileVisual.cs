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
    public class WallTileVisual : Sprite
    {
        public WallStates currentState;

        public static int defaultSize => 10;
        public Rectangle SourceRectangle => new Rectangle(Textures[currentState].imagePos, new Point(defaultSize));
        public Rectangle DestinationRectangle => new Rectangle(Position.ToPoint(), new Point((int)(defaultSize * scale.X)));

        private Vector2 origin => SourceRectangle.Size.ToVector2() / 2;

        Vector2 scale;

        Dictionary<WallStates, (Point imagePos, float Rotation)> Textures = new Dictionary<WallStates,  (Point, float)>()
        {
            [WallStates.Empty] = (new Point(122, 1), 0),
            [WallStates.LoneWall] = (new Point(1, 1), 0),

            [WallStates.Horiz] = (new Point(12, 1), (float)(Math.PI * .5)),
            [WallStates.HorizLeftEnd] = (new Point(23, 1), (float)(Math.PI * 1.5)),
            [WallStates.HorizRightEnd] = (new Point(23, 1), (float)(Math.PI * .5)),

            [WallStates.Verti] = (new Point(12, 1), 0),
            [WallStates.VertiTopEnd] = (new Point(23, 1), 0),
            [WallStates.VertiBottomEnd] = (new Point(23, 1), (float)(Math.PI)),

            [WallStates.TopLeftCorner] = (new Point(34, 1), 0),
            [WallStates.TopRightCorner] = (new Point(34, 1), (float)(Math.PI * .5)),
            [WallStates.BottomRightCorner] = (new Point(34, 1), (float)Math.PI),
            [WallStates.BottomLeftCorner] = (new Point(34, 1), (float)(Math.PI * 1.5)),

            [WallStates.TopEdge] = (new Point(45, 1), 0),
            [WallStates.RightEdge] = (new Point(45, 1), (float)(Math.PI * .5)),
            [WallStates.BottomEdge] = (new Point(45, 1), (float)Math.PI),
            [WallStates.LeftEdge] = (new Point(45, 1), (float)(Math.PI * 1.5)),

            [WallStates.Interior] = (new Point(122, 1), 0),

            [WallStates.OuterVerti] = (new Point(56, 1), 0),
            [WallStates.OuterHoriz] = (new Point(56, 1), (float)(Math.PI * .5)),

            [WallStates.TopLeftCornerOW] = (new Point(67, 1), 0),
            [WallStates.TopRightCornerOW] = (new Point(67, 1), (float)(Math.PI * .5)),
            [WallStates.BottomRightCornerOW] = (new Point(67, 1), (float)Math.PI),
            [WallStates.BottomLeftCornerOW] = (new Point(67, 1), (float)(Math.PI * 1.5)),

            [WallStates.TopIntersectingOW] = (new Point(78, 1), (float)(Math.PI * .5)),
            [WallStates.RightIntersectingOW] = (new Point(78, 1), (float)Math.PI),
            [WallStates.BottomIntersectingOW] = (new Point(78, 1), (float)(Math.PI * 1.5)),
            [WallStates.LeftIntersectingOW] = (new Point(78, 1), 0),

            [WallStates.MiddleLeftIntersectingOW] = (new Point(89, 1), 0),
            [WallStates.MiddleTopIntersectingOW] = (new Point(89, 1), (float)(Math.PI * .5)),
            [WallStates.MiddleRightIntersectingOW] = (new Point(89, 1), (float)Math.PI),
            [WallStates.MiddleBottomIntersectingOW] = (new Point(89, 1), (float)(Math.PI * 1.5)),

            [WallStates.TopLeftIntersectingOW] = (new Point(100, 1), (float)(Math.PI * .5)),
            [WallStates.RightTopIntersectingOW] = (new Point(100, 1), (float)Math.PI),
            [WallStates.BottomRightIntersectingOW] = (new Point(100, 1), (float)(Math.PI * 1.5)),
            [WallStates.LeftBottomIntersectingOW] = (new Point(100, 1), 0),

            [WallStates.LeftTopIntersectingOW] = (new Point(111, 1), 0),
            [WallStates.TopRightIntersectingOW] = (new Point(111, 1), (float)(Math.PI * .5)),
            [WallStates.RightBottomIntersectingOW] = (new Point(111, 1), (float)Math.PI),
            [WallStates.BottomLeftIntersectingOW] = (new Point(111, 1), (float)(Math.PI * 1.5)),


            // [WallStates.Tople]

            //midle, edge, edge2
        };

        public WallTileVisual(Vector2 Position, Color Tint, WallStates CurrentState, Vector2 Scale) : base(MainGame.spriteSheet, Position, Tint)
        {
            currentState = CurrentState;
            scale = Scale;

            if (currentState == WallStates.OuterRight || currentState == WallStates.OuterLeft)
            {
                currentState = WallStates.OuterHoriz;
            }

        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, DestinationRectangle, SourceRectangle, Tint, Textures[currentState].Rotation, origin, SpriteEffects.None, 0);
        }
    }
}
