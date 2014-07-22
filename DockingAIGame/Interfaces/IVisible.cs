using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DockingAIGame.Interfaces
{
    public interface IVisible
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch sbatch);
        void UnloadContent();
    }
}
