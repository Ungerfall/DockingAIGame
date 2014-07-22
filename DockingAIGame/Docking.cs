using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using DockingAIGame.UI;
namespace DockingAIGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public sealed class Docking : Microsoft.Xna.Framework.Game
    {
        #region Constants
        const int WIDTH = 800;
        const int HEIGHT = 600;
        const int MAXPAGERULES = 3;
        #endregion

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        private int m_rules_page_number = 1;

        #region Condition
        enum Conditions : byte
        {
            IsMenu = 0,
            IsGamerules,
            IsAbout,
            IsBoard
        };
        private Conditions m_cond = Conditions.IsMenu;
        #endregion

        #region Elements
        private TextButton m_menu_button_ng;
        private TextButton m_menu_button_exit;
        private TextButton m_menu_button_gamerules;
        private TextButton m_menu_button_about;
        private SpriteFont m_game_font;
        private Background m_menu_bg;
        private Texture2D m_game_logo;
        private Button m_button_back;
        private Button m_button_forward;
        private TextButton m_button_to_mainmenu;
        #endregion

        public Docking()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            Content.RootDirectory = "Content";
            this.Window.Title = "Игра Стыковка";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            m_menu_button_ng = new TextButton(
                                        new Vector2(280, 260),
                                        new System.Drawing.FontFamily("Comic Sans MS"),
                                        18, 28,
                                        "Новая игра");
            m_menu_button_ng.Action = () =>
                {
                };
            m_menu_button_gamerules = new TextButton(
                                       new Vector2(298, 320),
                                       new System.Drawing.FontFamily("Comic Sans MS"),
                                       18, 28,
                                       "Правила");
            m_menu_button_gamerules.Action = () =>
                {
                    this.m_cond = Conditions.IsGamerules;
                };
            m_menu_button_about = new TextButton(
                                            new Vector2(307, 380),
                                            new System.Drawing.FontFamily("Comic Sans MS"),
                                            18, 28,
                                            "Об игре");
            m_menu_button_about.Action = () =>
                {
                    this.m_cond = Conditions.IsAbout;
                };
            m_menu_button_exit = new TextButton(
                                            new Vector2(318, 440),
                                            new System.Drawing.FontFamily("Comic Sans MS"),
                                            18, 28,
                                            "Выход");
            m_menu_button_exit.Action = () =>
                {
                    Exit();
                };

            m_button_back = new Button(new Vector2(25, 550), 20, 20);
            m_button_back.Action = () =>
                {
                    if (m_cond == Conditions.IsAbout)
                        m_cond = Conditions.IsMenu;
                    if (m_cond == Conditions.IsGamerules)
                    {
                        if (m_rules_page_number == 1)
                            m_cond = Conditions.IsMenu;
                        else
                        {
                            if (m_rules_page_number == MAXPAGERULES)
                                m_button_forward.IsDisabled = false;
                            m_rules_page_number--;
                        }
                    }
                };

            m_button_forward = new Button(new Vector2(WIDTH - 50, 550), 20, 20);
            m_button_forward.Action = () =>
                {
                    if (m_rules_page_number == MAXPAGERULES - 1)
                        m_button_forward.IsDisabled = true;
                    m_rules_page_number++;
                };

            m_button_to_mainmenu = new TextButton(
                                            new Vector2(290, 542),
                                            new System.Drawing.FontFamily("Comic Sans MS"),
                                            16, 20,
                                            "в главное меню");
            m_button_to_mainmenu.Action = () =>
                {
                    m_button_forward.IsDisabled = false;
                    m_rules_page_number = 1;
                    this.m_cond = Conditions.IsMenu;
                };
            m_button_to_mainmenu.DrawColor = Color.FromNonPremultiplied(159, 120, 74, 255);
            m_menu_bg = new Background(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            m_game_logo = new Texture2D(GraphicsDevice, 0, 0);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            // TODO: use this.Content to load your game content here
            m_menu_button_ng.LoadContent(this.GraphicsDevice, Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_menu_button_exit.LoadContent(this.GraphicsDevice, Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_menu_button_gamerules.LoadContent(this.GraphicsDevice, Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_menu_button_about.LoadContent(this.GraphicsDevice, Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_button_back.LoadContent(Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_button_back.Texture = Content.Load<Texture2D>("arrow_left");
            m_button_to_mainmenu.LoadContent(this.GraphicsDevice, Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_button_forward.LoadContent(Content.Load<SoundEffect>("Sounds" + System.IO.Path.DirectorySeparatorChar + "Menu_Tick"));
            m_button_forward.Texture = Content.Load<Texture2D>("arrow_right");
            m_menu_bg.Bg_Texture = Content.Load<Texture2D>("menu_background");
            m_game_logo = Content.Load<Texture2D>("gamelogo");
            m_game_font = Content.Load<SpriteFont>("Fonts" + System.IO.Path.DirectorySeparatorChar + "Comic Sans MS");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            m_menu_button_ng.UnloadContent();
            m_menu_button_exit.UnloadContent();
            m_menu_button_gamerules.UnloadContent();
            m_menu_button_about.UnloadContent();
            m_button_back.UnloadContent();
            m_button_forward.UnloadContent();
            m_menu_bg.UnloadContent();
            m_button_to_mainmenu.UnloadContent();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (this.m_cond)
            {
                case Conditions.IsMenu:
                    {
                        m_menu_button_ng.Update(gameTime);
                        m_menu_button_gamerules.Update(gameTime);
                        m_menu_button_about.Update(gameTime);
                        m_menu_button_exit.Update(gameTime);
                    }
                    break;
                case Conditions.IsBoard:
                    break;
                case Conditions.IsGamerules:
                    {
                        m_button_back.Update(gameTime);
                        m_button_to_mainmenu.Update(gameTime);
                        m_button_forward.Update(gameTime);
                    }
                    break;
                case Conditions.IsAbout:
                    {
                        m_button_back.Update(gameTime);
                    }
                    break;
            }
        }
    
        private void DrawMenu()
        {
            spriteBatch.Draw(m_game_logo, new Rectangle(140, 0, 450, 250), Color.White);
            m_menu_button_ng.Draw(this.spriteBatch);
            m_menu_button_gamerules.Draw(this.spriteBatch);
            m_menu_button_about.Draw(this.spriteBatch);
            m_menu_button_exit.Draw(this.spriteBatch);
        }
        /// <summary>
        /// Выравнивает строку по центру
        /// </summary>
        /// <param name="st">Строка для выравнивания</param>
        /// <param name="offset">Смещение, зависит от количества заглавных букв входной строки st</param>
        /// <returns>Возвращает выравненную по центру строку</returns>
        static private string p_center_aligh(string st, int offset = 0)
        {
            var total_len = 110 - offset;
            return st.PadLeft(((total_len - st.Length) / 2) 
                            + st.Length);
        }

        private void DrawAbout()
        {
            string _about = "\n" + "\n" +
                p_center_aligh("МИНОБРНАУКИ РФ\n") +
                p_center_aligh("Государственное образовательное учреждение\n") +
                p_center_aligh("высшего профессионального образования\n") +
                p_center_aligh("«Ижевский государственный технический университет\n") +
                p_center_aligh("им М.Т. Калашникова»\n") +
                p_center_aligh("Факультет \"Информатика и вычислительная техника\"\n") +
                p_center_aligh("Кафедра \"Программное обеспечение\"\n") +
                "\n" +
                "\n" +
                p_center_aligh("ПОЯСНИТЕЛЬНАЯ ЗАПИСКА\n", 5) +
                p_center_aligh("к курсовой работе на тему\n") +
                p_center_aligh("\"Программирование логических игр\"\n") +
                "\n" +
                p_center_aligh("Вариант: Стыковка\n") +
                "\n" +
                "\n" +
                "      Выполнил:\n" +
                "      Ст.-гр. БО6-191-2" + new string(' ', 70) + "Петров Л.С.\n" + 
                "      Принял:\n" +
                "      к.т.н., доцент" + new string(' ', 68) + "Коробейников А.В.\n" +
                "\n" +
                p_center_aligh("   Ижевск 2014");
            this.spriteBatch.DrawString(
                this.m_game_font,
                _about,
                new Vector2(0f, 0f),
                Color.Black,
                0f,
                Vector2.Zero,
                0.7f,
                SpriteEffects.None,
                0f);
            m_button_back.Draw(this.spriteBatch);
        }

        private void DrawGameRules()
        {
            m_button_back.Draw(this.spriteBatch);
            m_button_to_mainmenu.Draw(this.spriteBatch);
            m_button_forward.Draw(this.spriteBatch);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            m_menu_bg.Draw(this.spriteBatch);
            switch (this.m_cond)
            {
                case Conditions.IsMenu:
                    {
                        DrawMenu();
                    }
                    break;
                case Conditions.IsBoard:
                    break;
                case Conditions.IsGamerules:
                    {
                        DrawGameRules();
                    }
                    break;
                case Conditions.IsAbout:
                    {
                        DrawAbout();
                    }
                    break;
            }
            spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}