using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using DockingAIGame.Interfaces;

namespace DockingAIGame.UI
{
    public enum State
    {
        BTN_NORMAL = 0,
        BTN_HOVER,
    }

    public class Button : IVisible
    {
        #region Fields
        private State m_state = State.BTN_NORMAL;
        private Rectangle[] m_box = new Rectangle[2];
        private Texture2D m_texture;
        private Vector2 m_position;
        private SoundEffect m_sound_tick;
        private SoundEffectInstance m_sound_inst_tick;
        private bool m_is_sound_played;
        private byte m_alpha;
        private bool m_is_disabled;
        #endregion

        #region Properties
        public Action Action { get; set; }
        public Texture2D Texture
        {
            get { return this.m_texture; }
            set { this.m_texture = value; }
        }
        public bool IsDisabled
        {
            get { return this.m_is_disabled; }
            set 
            {
                this.m_is_disabled = value;
                if (this.m_is_disabled)
                    this.m_alpha = 100;
                else
                    this.m_alpha = 255;
            }
        }
        #endregion

        public Button(Vector2 pos, int width, int height)
        {
            this.m_box[0] = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            this.m_box[1] = new Rectangle(
                                        (int)pos.X - (int)((width * 1.56f - width) / 2),
                                        (int)pos.Y - (int)((height * 1.56f - height) / 2),
                                        (int)(width * 1.56f),
                                        (int)(height * 1.56f));
            this.m_position = pos;
            this.IsDisabled = false;
        }
        public void Update(GameTime gameTime)
        {
            if (!this.m_is_disabled)
            {
                var mouse_state = Mouse.GetState();
                if (this.m_box[1].Contains(mouse_state.X, mouse_state.Y))
                    if (mouse_state.LeftButton == ButtonState.Pressed)
                    {
                        if (Action != null && this.m_state == State.BTN_HOVER)
                        {
                            Action.Invoke();
                            this.m_state = State.BTN_NORMAL;
                        }
                    }
                    else
                    {
                        if (!this.m_is_sound_played)
                        {
                            m_sound_inst_tick.Play();
                            this.m_is_sound_played = true;
                        }
                        this.m_state = State.BTN_HOVER;
                    }
                else
                {
                    this.m_state = State.BTN_NORMAL;
                    this.m_is_sound_played = false;
                }
            }
        }

        public void LoadContent(SoundEffect snd)
        {
            m_sound_tick = snd;
            m_sound_inst_tick = m_sound_tick.CreateInstance();
        }

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(this.m_texture, this.m_box[(int)this.m_state], Color.FromNonPremultiplied(121, 88, 51, this.m_alpha));
        }

        public void UnloadContent()
        {
            this.m_texture.Dispose();
            this.m_sound_inst_tick.Dispose();
            this.m_sound_tick.Dispose();
        }
    }
}
