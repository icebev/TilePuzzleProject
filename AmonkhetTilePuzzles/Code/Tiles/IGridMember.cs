using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* GRID MEMBER INTERFACE
     * Last modified by Joe Bevis 13/01/2022
     ****************************************/

    /// <summary>
    /// Each object within the tiles array and tiles list is a member of the puzzle grid
    /// Therefore both the blank tile and regular tile implement the grid member interface
    /// this allows the two data types to be included in the same collections
    /// </summary>
    public interface IGridMember
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
