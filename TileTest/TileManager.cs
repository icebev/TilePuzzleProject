using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
   
    class TileManager
    {
        
        public int m_gridSize;

        //public List<IGridMember> m_tileList;
        public IGridMember[,] m_tilesArray;

        public Texture2D m_puzzleImage;

        public Point BlankTilePosition => this.FindBlankTile();
        public TileManager(int gridSize, Texture2D picture)
        {
            this.m_gridSize = gridSize;
            //this.m_tileList = new List<IGridMember>();
            this.m_tilesArray = new IGridMember[this.m_gridSize, this.m_gridSize];
            this.m_puzzleImage = picture;
        }

        public void GenerateTiles()
        {
            int maxArrayValue = this.m_gridSize - 1;

            for (int x = 0; x < this.m_gridSize; x++)
            {
                for (int y = 0; y < this.m_gridSize; y++)
                {
                    Tile newTile = new Tile(new Point(x, y), this.m_puzzleImage, this.m_gridSize);
                    if (!(x == maxArrayValue && y == maxArrayValue))
                    {
                        //this.m_tileList.Add(newTile);
                        this.m_tilesArray[x, y] = newTile;
                    }
                    else
                    {
                        this.m_tilesArray[x, y] = new EmptyTile(new Point(x, y), this.m_gridSize);
                    }
                }
            }

            
        }
        public void DrawTiles(SpriteBatch spriteBatch)
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "TileTest.Tile")
                        tile.DrawIt(spriteBatch);
            }
        }
        public Point FindBlankTile()
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "TileTest.EmptyTile")
                    {
                        //Debug.WriteLine("Found empty tile!");
                        return new Point(tile.CurrentGridPosition.X, tile.CurrentGridPosition.Y);
                    }    
            }
            Debug.WriteLine("Unable to locate the empty tile.");
            return new Point(0, 0);
        }

        public void DetermineSwappableTiles()
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "TileTest.Tile")
                    {
                        int gridXDifference = Math.Abs(this.BlankTilePosition.X - tile.CurrentGridPosition.X);
                        int gridYDifference = Math.Abs(this.BlankTilePosition.Y - tile.CurrentGridPosition.Y);

                        if (gridXDifference <= 1 && gridYDifference <= 1 && gridXDifference + gridYDifference <= 1)   
                        {
                            tile.IsCurrentlySwappable = true;
                            Debug.WriteLine($"Tile at {tile.CurrentGridPosition.X}, {tile.CurrentGridPosition.Y} set as swappable!");
                        }
                        else
                        {
                            tile.IsCurrentlySwappable = false;
                        }
                    }
            }
        }

        public void SwapTile(IGridMember tileToSwap)
        {
            Point newEmptyPosition = new Point(tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y);
            tileToSwap.CurrentGridPosition = this.FindBlankTile();
            this.m_tilesArray[tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y] = tileToSwap;

            this.m_tilesArray[newEmptyPosition.X, newEmptyPosition.Y] = new EmptyTile(new Point(this.m_gridSize - 1, this.m_gridSize - 1), this.m_gridSize);

            this.DetermineSwappableTiles();
        }
    }
}
