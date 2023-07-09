using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Pacman;
using System.Net.NetworkInformation;

namespace LePacman.Screens.MainGame
{
    public abstract class Entity : SpriteBase
    {
        private protected Dictionary<EntityStates, Point> Textures = new Dictionary<EntityStates, Point>()
        {
            [EntityStates.ClosedPacman] = new Point(1, 23),
            [EntityStates.Pacman] = new Point(15, 23),
            [EntityStates.OpenPacman] = new Point(29, 23),
        };

        EntityStates entityState;

        public override Vector2 Origin { get => SourceRectangle.Size.ToVector2() / 2; }

        private protected int defaultSize;
        public Rectangle SourceRectangle => new Rectangle(Textures[entityState], new Point(defaultSize));
        public Rectangle DestinationRectangle => new Rectangle(Position.ToPoint(), new Point((int)(defaultSize * Scale.X)));

        public Entity(Vector2 Position, Color Tint, Vector2 Scale, EntityStates EntityState) : base(MainGame.spriteSheet, Position, Tint)
        {
            this.Scale = Scale;
            entityState = EntityState;
            Rotation = 0;
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(Image, DestinationRectangle, SourceRectangle, Tint, Rotation, Origin, SpriteEffects.None, 0);
        }
    }
}
