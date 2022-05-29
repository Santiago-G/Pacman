using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pacman
{
    public abstract class Sprite
    {
        [JsonIgnore]
        public Texture2D Image { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual Color Tint { get; set; }

        public virtual Vector2 Scale { get; set; } = Vector2.One;
        public virtual Vector2 Origin { get; set; } = Vector2.Zero;
        public virtual float Rotation { get; set; } = 0f;
        public virtual SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

        public virtual Rectangle Hitbox => new Rectangle((int)(Position.X - Origin.X * Scale.X), (int)(Position.Y - Origin.Y * Scale.Y), (int)(Image.Width * Scale.X), (int)(Image.Height * Scale.Y));

        public Sprite(Texture2D Image, Vector2 Position, Color Tint)
        {
            this.Image = Image;
            this.Position = Position;
            this.Tint = Tint;
        }

        public Sprite(
            //making another constructor)

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch batch);
    }

    //Add scale, origin, rotation, spriteeffects, hitbox
    //Scale is default 1, origin is default top left, rotation is default 0, spriteffects is default None
    //hitbox is auto calculated
}
