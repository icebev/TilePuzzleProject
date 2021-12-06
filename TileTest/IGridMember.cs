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
        bool IsCurrentlySwappable { get; set; }
        int GridSize { get; set; }
        void DrawIt(SpriteBatch spriteBatch);
    }
}
