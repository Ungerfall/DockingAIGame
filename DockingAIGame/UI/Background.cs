using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Text;
using DockingAIGame.Interfaces;

namespace DockingAIGame.UI
{
    class Background : IVisible
    {
        #region Fields
        private Texture2D m_bg_texture;
        private Rectangle m_rect;
        #endregion

        public Texture2D Bg_Texture
        {
            get { return m_bg_texture; }
            set { m_bg_texture = value; }
        }

        public Background(int wi, int he)
        {
            this.m_rect = new Rectangle(0, 0, wi, he);
        }
        
        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(m_bg_texture, m_rect, Color.LightGray);
        }


        public void UnloadContent()
        {
            m_bg_texture.Dispose();
        }
    }
}
