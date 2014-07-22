using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockingAIGame.UI
{
    public class Checker : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Checker(Microsoft.Xna.Framework.Game game) : base(game)
        {
 
        }
        /// <summary>
        /// Типы шашек
        /// </summary>
        public enum CheckerType
        {
            Black,
            White
        };

        /// <summary>
        /// Тип шашки
        /// </summary>
        public CheckerType Type { get; set; }

        private int x_board;
        /// <summary>Х-координата на поле</summary>
        public int XBoard
        {
            get { return x_board; }
            set 
            {
                if (value > 0 && value < 9)
                    x_board = value;
                else
                    throw new Exception("Ошибка. Доска имеет размеры 8х8");
            }
        }

        private int y_board;
        /// <summary>Y-координата на поле</summary>
        public int YBoard
        {
            get { return y_board; }
            set
            {
                if (value > 0 && value < 9)
                    y_board = value;
                else
                    throw new Exception("Ошибка. Доска имеет размеры 8х8");
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}