using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pacman.Screens;

namespace Pacman.Screens
{
    public class OptionsAudio : Screen
    {
        static Image background;
        static Button audioText;
        static Image masterVolumeText;
        static Image SFXVolumeText;
        static Image musicVolumeText;
        static Vector2 screenOrigin => Options.screenOrigin;

        static Slider masterVolumeControl;
        static Slider SFXVolumeControl;
        static Slider musicVolumeControl;

        public OptionsAudio((int width, int height) Size, Vector2 Position, GraphicsDeviceManager Graphics, Image BackgroundImage) : base(Size, Position, Graphics)
        {
            size = Size;
            position = Position;
            background = BackgroundImage;
        }

        public override void LoadContent(ContentManager Content)
        {
            audioText = new Button(Content.Load<Texture2D>("audioText"), new Vector2(), Color.White);
            objects.Add(audioText);

            masterVolumeText = new Image(Content.Load<Texture2D>("MasterVolumeText"), new Vector2(), Color.White);
            objects.Add(masterVolumeText);

            SFXVolumeText = new Image(Content.Load<Texture2D>("SFXVolumeText"), new Vector2(), Color.White);
            objects.Add(SFXVolumeText);

            musicVolumeText = new Image(Content.Load<Texture2D>("musicVolumeText"), new Vector2(), Color.White);
            objects.Add(musicVolumeText);

            masterVolumeControl = new Slider(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2(0), 15, 30, Color.White), new Vector2(0), 2, Color.White, 10, 3);
            masterVolumeControl.LoadContent(Content);
            masterVolumeControl.numOfVisibleBars = masterVolumeControl.numOfBars / 2 - 1;
            objects.Add(masterVolumeControl);

            SFXVolumeControl = new Slider(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2(0), 15, 30, Color.White), new Vector2(0), 2, Color.White, 10, 3);
            SFXVolumeControl.LoadContent(Content);
            SFXVolumeControl.numOfVisibleBars = 7;
            objects.Add(SFXVolumeControl);

            musicVolumeControl = new Slider(new SliderBar(Content.Load<Texture2D>("smallPixel"), new Vector2(0), 15, 30, Color.White), new Vector2(0), 2, Color.White, 10, 3);
            musicVolumeControl.LoadContent(Content);
            musicVolumeControl.numOfVisibleBars = 7;
            objects.Add(musicVolumeControl);
        }

        static public void setUpPositions()
        {
            audioText.Position = new Vector2(screenOrigin.X + (background.Image.Width / 2 - (audioText.Image.Width / 2)), screenOrigin.Y + 50);

            masterVolumeText.Position = new Vector2(screenOrigin.X + 60, screenOrigin.Y + 200);
            SFXVolumeText.Position = new Vector2(screenOrigin.X + 60, screenOrigin.Y + 300);
            musicVolumeText.Position = new Vector2(screenOrigin.X + 60, screenOrigin.Y + 400);

            masterVolumeControl.UpdatePositions(new Vector2(screenOrigin.X + 550, screenOrigin.Y + 194));
            SFXVolumeControl.UpdatePositions(new Vector2(screenOrigin.X + 550, screenOrigin.Y + 294));
            musicVolumeControl.UpdatePositions(new Vector2(screenOrigin.X + 550, screenOrigin.Y + 394));
        }
    }
}
