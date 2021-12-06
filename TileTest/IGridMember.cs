using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    interface IGridMember
    {
        Point CurrentGridPosition { get; set; }
        Point CorrectGridPosition { get; set; }

        Rectangle TileBounds { get; }
        bool IsCurrentlySwappable { get; set; }
        int GridSize { get; set; }

        void UpdateIt(GameTime gameTime);
        void DrawIt(SpriteBatch spriteBatch);
    }
}
