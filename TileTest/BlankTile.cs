using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class BlankTile : IGridMember
    {
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int GridSize { get; set; }
        public bool IsCurrentlySwappable { get; set; }
        public Rectangle TileBounds { get; set; }

        public void DrawIt(SpriteBatch spriteBatch)
        {
        }

        public void UpdateIt(GameTime gameTime)
        {
        }

        public BlankTile(Point correctPosition, int gridSize)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.GridSize = gridSize;
        }
    }
}
