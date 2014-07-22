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
using SDraw = System.Drawing;

namespace DockingAIGame.UI
{
    public class TextButton : IVisible
    {
        #region Fields        
        private State m_state;
        private string m_text;
        private SDraw.FontFamily m_ffont;
        private float m_normal_size;
        private float m_hover_size;
        private Rectangle m_box;
        private Texture2D[] m_text_texture;
        private Vector2 m_position;
        private SoundEffect m_sound_tick;
        private SoundEffectInstance m_sound_inst_tick;
        private bool m_is_sound_played;
        private SDraw.Color m_sdraw_hover_color;
        #endregion

        #region Properties
        public Action Action { get; set; }
        /// <summary>
        /// Задает цвет кнопки в состоянии HOVER
        /// </summary>
        public SDraw.Color SDrawHoverColor
        {
            get { return m_sdraw_hover_color; }
            set { m_sdraw_hover_color = value; }
        }
        /// <summary>
        /// Цвет Color для метода Draw
        /// </summary>
        public Color DrawColor { get; set; }
        #endregion

        public TextButton(Vector2 position, SDraw.FontFamily font, float NormalSize, float HoverSize, string text)
        {
            this.m_position = position;
            this.m_text = text;
            this.m_ffont = font;
            using (SDraw.Graphics grx = SDraw.Graphics.FromImage(new SDraw.Bitmap(1,1)))
            {
                this.m_box = new Rectangle((int)position.X, (int)position.Y, (int)grx.MeasureString(text, new SDraw.Font(font, HoverSize)).Width, (int)(1.33f * HoverSize));
            }
            this.m_text_texture = new Texture2D[2];
            this.m_normal_size = NormalSize;
            this.m_hover_size = HoverSize;
            this.m_state = State.BTN_NORMAL;
            this.m_is_sound_played = false;
            m_sdraw_hover_color = SDraw.Color.OrangeRed;
            DrawColor = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            var mouse_state = Mouse.GetState();
            if (this.m_box.Contains(mouse_state.X, mouse_state.Y))
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

        public void Draw(SpriteBatch sbatch)
        {
            sbatch.Draw(this.m_text_texture[(int)this.m_state], this.m_box, DrawColor);
        }

        public void LoadContent(GraphicsDevice g_device, SoundEffect snd)
        {
            CreateTextSprite(g_device);
            m_sound_tick = snd;
            m_sound_inst_tick = m_sound_tick.CreateInstance();
        }

        public void UnloadContent()
        {
            this.m_text_texture[0].Dispose();
            this.m_text_texture[1].Dispose();
            this.m_ffont.Dispose();
            this.m_sound_inst_tick.Dispose();
            this.m_sound_tick.Dispose();
        }

        private void CreateTextSprite(GraphicsDevice g_device)
        {
            using (var bitmap = new SDraw.Bitmap(this.m_box.Width, this.m_box.Height))
            using (var g = SDraw.Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = SDraw.Text.TextRenderingHint.AntiAlias;
                using (var font_1 = new SDraw.Font(this.m_ffont, this.m_normal_size, SDraw.FontStyle.Regular, SDraw.GraphicsUnit.Pixel))
                using (var font_2 = new SDraw.Font(this.m_ffont, this.m_hover_size, SDraw.FontStyle.Regular, SDraw.GraphicsUnit.Pixel))
                {
                    var strformat = new SDraw.StringFormat
                    {
                        Alignment = SDraw.StringAlignment.Center,
                        LineAlignment = SDraw.StringAlignment.Center
                    };
                    const int BORDER = 4;
                    var SurRectangle = new SDraw.RectangleF(BORDER, BORDER,
                                                            this.m_box.Width - 2 * BORDER,
                                                            this.m_box.Height - 2 * BORDER);
                    g.DrawString(this.m_text, font_1, SDraw.Brushes.Black, SurRectangle, strformat);
                    using (var stream = new System.IO.MemoryStream())
                    {
                        bitmap.Save(stream, SDraw.Imaging.ImageFormat.Png);
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        this.m_text_texture[0] = Texture2D.FromStream(g_device, stream);
                    }
                    g.Clear(SDraw.Color.Transparent);
                    using (SDraw.SolidBrush a = new SDraw.SolidBrush(this.m_sdraw_hover_color))
                    {
                        g.DrawString(this.m_text, font_2, a, SurRectangle, strformat);
                    }                    
                    using (var stream = new System.IO.MemoryStream())
                    {
                        bitmap.Save(stream, SDraw.Imaging.ImageFormat.Png);
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        this.m_text_texture[1] = Texture2D.FromStream(g_device, stream);
                    }
                }
            }
        }
    }
}