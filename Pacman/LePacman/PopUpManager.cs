using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LePacman
{
    public class PopUpManager
    {   
        public Queue<PopUp> PopUps = new Queue<PopUp>();
        public PopUp currentPopUp => PopUps.Peek();

        public bool pauseScene { get; private set; }

        private PopUpManager() { }

        public static PopUpManager Instance { get; } = new PopUpManager();

        public void EnqueuePopUp(PopUp newPopUp)
        {
            newPopUp.setVisable(true);
            newPopUp.setBodyText(newPopUp.getBodyText());

            PopUps.Enqueue(newPopUp);
            pauseScene = newPopUp.getPauseScreen();
        }

        public void DequeuePopUp()
        {
            PopUps.Dequeue();
            pauseScene = false;
        }

        public void LoadContent(ContentManager Content)
        {
        //    Texture2D errorBackground = Content.Load<Texture2D>("outerWallErrorMSGBG");
        //    SpriteFont errorHeaderFont = Content.Load<SpriteFont>("ErrorHeaderFont");
        //    SpriteFont errorBodyFont = Content.Load<SpriteFont>("ErrorBodyText");

        //    popUps.Add(PopUpStates.PortalError, new ErrorPopUp(errorBackground, new Point(500), new Vector2(-100), errorHeaderFont, errorBodyFont));

        }

        public void Update(GameTime GameTime) 
        {
            if (PopUps.Count == 0) return;

            currentPopUp.Update(GameTime);
        }

        public void Draw(SpriteBatch batch) 
        {
            if (PopUps.Count == 0) return;

            currentPopUp.Draw(batch);
        }
    }
}
