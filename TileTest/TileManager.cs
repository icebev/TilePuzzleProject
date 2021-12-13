using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
   
    public class TileManager
    {
        #region Member Variables

        private int m_gridSize;

        private IGridMember[,] m_tilesArray;
        private List<IGridMember> m_tilesList;

        private Texture2D m_puzzleImage;
        private Texture2D m_tileShadow;
        private SoundEffect m_tileSlideSfx;
        private SpriteFont m_font;

        private bool m_shouldPlaySfx;
        public bool m_puzzleComplete;

        private int m_moves = 0;

        private readonly Random m_random;

        #endregion

        #region Properties

        public Point BlankTilePosition
        {
            get { return this.FindBlankTile(); }
        }

        public int GridSize
        {
            get { return this.m_gridSize; }
            private set { this.m_gridSize = value; }
        }

        public IGridMember[,] TilesArray
        {
            get { return this.m_tilesArray; }
            private set { this.m_tilesArray = value; }
        }   
        
        public List<IGridMember> TilesList
        {
            get { return this.m_tilesList; }
            private set { this.m_tilesList = value; }
        }

        public int MoveCount 
        {
            get { return this.m_moves; }
            private set { this.m_moves = value; }
        }

        #endregion

        public TileManager(int gridSize, Texture2D picture, SoundEffect slideSFX, SpriteFont font, Texture2D tileshadow)
        {
            this.m_random = new Random();

            this.m_puzzleImage = picture;
            this.m_tileShadow = tileshadow;
            this.m_tileSlideSfx = slideSFX;
            this.m_font = font;

            this.GridSize = gridSize;
            this.TilesArray = new IGridMember[this.m_gridSize, this.m_gridSize];
            this.TilesList = new List<IGridMember>();
            
        }

        public void GenerateTiles()
        {
            int maxArrayValue = this.GridSize - 1;
            int counter = 1;
            for (int y = 0; y < this.GridSize; y++)
            {
                for (int x = 0; x < this.GridSize; x++)
                {
                    Tile newTile = new Tile(new Point(x, y), counter, this.m_puzzleImage, this.GridSize, this.m_font, this.m_tileShadow);
                    counter++;
                    if (!(x == maxArrayValue && y == maxArrayValue))
                    {
                        this.m_tilesArray[x, y] = newTile;
                        this.m_tilesList.Add(newTile);
                    }
                    else
                    {
                        BlankTile newBlankTile = new BlankTile(new Point(x, y), this.GridSize);
                        this.m_tilesArray[x, y] = newBlankTile;
                        this.m_tilesList.Add(newBlankTile);
                    }
                }
            }
            this.DetermineSwappableTiles();
        }
        public void UpdateTiles(GameTime gameTime)
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile.GetType().ToString() == "TileTest.Tile")
                    tile.UpdateIt(gameTime);
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

        public void DrawReferenceImage(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.m_tileShadow, new Rectangle(905, 105, 300, 300), Color.White);
            spriteBatch.Draw(this.m_puzzleImage, new Rectangle(900, 100, 300, 300), Color.White);
        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this.m_font, $"Moves: {this.m_moves}", new Vector2(900, 600), Color.White);
            if (this.m_puzzleComplete)
                spriteBatch.DrawString(this.m_font, $"Puzzle complete!", new Vector2(900, 500), Color.White);
        }

        public Point FindBlankTile()
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "TileTest.BlankTile")
                    {
                        return new Point(tile.CurrentGridPosition.X, tile.CurrentGridPosition.Y);
                    }    
            }
            Debug.WriteLine("Unable to locate the blank tile.");
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
                            //Debug.WriteLine($"Tile at {tile.CurrentGridPosition.X}, {tile.CurrentGridPosition.Y} set as swappable!");
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
            if (this.m_shouldPlaySfx)
                this.m_tileSlideSfx.Play();

            Point newBlankPosition = new Point(tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y);

            tileToSwap.CurrentGridPosition = this.FindBlankTile();
            this.m_tilesArray[tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y] = tileToSwap;

            BlankTile newBlankTile = new BlankTile(new Point(this.m_gridSize - 1, this.m_gridSize - 1), this.m_gridSize);
            newBlankTile.CurrentGridPosition = newBlankPosition;
            this.m_tilesArray[newBlankPosition.X, newBlankPosition.Y] = newBlankTile;

            this.m_moves++;

            this.DetermineSwappableTiles();
        }

        public void CheckForTileClick(MouseState currentMouseState, MouseState previousMouseState)
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile.IsCurrentlySwappable && tile.GetType().ToString() == "TileTest.Tile")
                {
                    if (tile.TileBounds.Contains(currentMouseState.Position) 
                        && currentMouseState.LeftButton == ButtonState.Pressed 
                        && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        this.SwapTile(tile);
                    }
                }
            } 
        }    
        public void CheckForArrowKey(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
            {
                if (this.BlankTilePosition.Y > 0)
                {
                    this.SwapTile(this.m_tilesArray[this.BlankTilePosition.X, this.BlankTilePosition.Y - 1]);
                }
            } 
            
            if (currentKeyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (this.BlankTilePosition.Y < this.m_gridSize - 1)
                {
                    this.SwapTile(this.m_tilesArray[this.BlankTilePosition.X, this.BlankTilePosition.Y + 1]);
                }
            }
            
            if (currentKeyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
            {
                if (this.BlankTilePosition.X > 0)
                {
                    this.SwapTile(this.m_tilesArray[this.BlankTilePosition.X - 1, this.BlankTilePosition.Y]);
                }
            } 
            
            if (currentKeyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
            {
                if (this.BlankTilePosition.X < this.m_gridSize - 1)
                {
                    this.SwapTile(this.m_tilesArray[this.BlankTilePosition.X + 1, this.BlankTilePosition.Y]);
                }
            }
        }
        public void JumbleTiles()
        {
            this.m_shouldPlaySfx = false;
            for (int i = 0; i < 500; i++)
            {
                IEnumerable<IGridMember> moveableTiles =
                    from tile in this.m_tilesList
                    where tile.IsCurrentlySwappable
                    select tile;

                int randomSelector = this.m_random.Next(0, moveableTiles.Count());
                this.SwapTile(moveableTiles.ToList()[randomSelector]);
            }
            this.m_shouldPlaySfx = true;
            this.MoveCount = 0;
        }

        public bool CheckPuzzleCompletion()
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile.CurrentGridPosition != tile.CorrectGridPosition)
                {
                    return false;
                }
            }
            this.m_puzzleComplete = true;
            return true;

        }
    }
}
