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
        public Dictionary<PopUpStates, ErrorPopUp> popUps;

        private PopUpManager() { }

        public static PopUpManager Instance { get; } = new PopUpManager();

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime GameTime) 
        {

        }

        public void Draw(SpriteBatch batch) 
        {

        }
    }
}
