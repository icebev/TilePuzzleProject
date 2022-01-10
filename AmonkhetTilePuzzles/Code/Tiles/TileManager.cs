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

namespace AmonkhetTilePuzzles
{
   
    public class TileManager
    {
        #region Member Variables

        private int m_gridSize;

        private IGridMember[,] m_tilesArray;
        private List<IGridMember> m_tilesList;

        public Texture2D m_puzzleImage;
        private Texture2D m_tileShadow;
        private SpriteFont m_font;

        private bool m_shouldPlaySfx;
        public bool m_completeDelayFlag;
        public bool m_puzzleComplete;
        
        private int m_moves = 0;
        private float m_secondsElapsed = 0;
        private const float COMPLETION_FLAG_DELAY_SECONDS = 1.5f;
        private float m_completeAnimationDelay;

        private Random m_random;
        private TileGame m_mainGame;

        #endregion

        #region Properties
        private TileGame MainGame
        {
            get { return this.m_mainGame; }
        }

        public bool IsShuffling { get; private set; }
        public int PuzzleImageIndex
        {
            get
            {
                return this.MainGame.PuzzleTextures.IndexOf(this.m_puzzleImage);
            }
        }
        
        public bool HighscoreSet
        {
            get
            {
                if (this.MainGame.ActiveHighscoreTracker.GetRelevantEntries(this.GridSize, this.PuzzleImageIndex).Count > 0) {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

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

        public int TotalSecondsElapsed
        {
            get { return (int)this.m_secondsElapsed; }
        }

        #endregion

        public TileManager(TileGame mainGame, int gridSize, Texture2D picture, SpriteFont font, Texture2D tileshadow)
        {
            this.m_random = new Random();
            this.m_mainGame = mainGame;
            this.m_puzzleImage = picture;
            this.m_tileShadow = tileshadow;
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
                    Tile newTile = new Tile(new Point(x, y), counter, this.m_puzzleImage, this.GridSize, this.m_font, this.m_tileShadow, this.m_mainGame);
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
            if (!this.m_puzzleComplete && !this.m_completeDelayFlag)
                this.m_secondsElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.m_completeDelayFlag && this.m_puzzleComplete)
                this.m_completeAnimationDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                this.m_completeAnimationDelay = COMPLETION_FLAG_DELAY_SECONDS;

            if (this.m_completeAnimationDelay <= 0 && !this.m_completeDelayFlag)
            {
                this.MainGame.ActiveGameState = GameState.PuzzleComplete;
                if (!this.MainGame.IsMuted)
                    AudioStore.m_puzzleCompleteSFX.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                this.m_completeDelayFlag = true;
            }

            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile.GetType().ToString() == "AmonkhetTilePuzzles.Tile")
                    tile.UpdateIt(gameTime);
            }
        }
        public void DrawTiles(SpriteBatch spriteBatch)
        {
            if (!this.m_completeDelayFlag)
            {
                foreach (IGridMember tile in this.m_tilesArray)
                {
                    if (tile != null)
                        if (tile.GetType().ToString() == "AmonkhetTilePuzzles.Tile")
                            tile.DrawIt(spriteBatch);
                }
            }
        }

        public void DrawReferenceImage(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.m_tileShadow, new Rectangle(this.MainGame.WindowCenter.X + 5 + this.MainGame.WindowHeight / 8, this.MainGame.WindowHeight / 8 + 20, this.MainGame.WindowHeight * 5 / 16 , this.MainGame.WindowHeight * 5 / 16), Color.White);
            spriteBatch.Draw(this.m_puzzleImage, new Rectangle(this.MainGame.WindowCenter.X + this.MainGame.WindowHeight / 8, this.MainGame.WindowHeight / 8 + 15, this.MainGame.WindowHeight * 5 / 16, this.MainGame.WindowHeight * 5 / 16), Color.White);
        }

        public Point FindBlankTile()
        {
            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "AmonkhetTilePuzzles.BlankTile")
                    {
                        return new Point(tile.CurrentGridPosition.X, tile.CurrentGridPosition.Y);
                    }    
            }
            Debug.WriteLine("Unable to locate the blank tile.");
            return new Point(0, 0);
        }

        public void DetermineSwappableTiles()
        {
            foreach (IGridMember tile in this.m_tilesList)
            {
                if (tile != null)
                    if (tile.GetType().ToString() == "AmonkhetTilePuzzles.Tile")
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
            if (this.m_shouldPlaySfx && !this.MainGame.IsMuted)
                AudioStore.m_tileSlideSFX.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);

            Point newBlankPosition = new Point(tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y);

            tileToSwap.CurrentGridPosition = this.FindBlankTile();
            this.TilesArray[tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y] = tileToSwap;

            BlankTile newBlankTile = new BlankTile(new Point(this.m_gridSize - 1, this.m_gridSize - 1), this.m_gridSize);
            newBlankTile.CurrentGridPosition = newBlankPosition;
            this.TilesArray[newBlankPosition.X, newBlankPosition.Y] = newBlankTile;

            this.m_moves++;

            this.DetermineSwappableTiles();
            if (!this.IsShuffling)
                this.CheckPuzzleCompletion();
        }
        public void JumbleTiles()
        {
            this.IsShuffling = true;
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
            this.IsShuffling = false;
            this.MoveCount = 0;
            this.m_secondsElapsed = 0;
            this.m_puzzleComplete = false;
            this.m_completeDelayFlag = false;
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
            Debug.WriteLine("Puzzle Complete!");
            this.MainGame.ActiveHighscoreTracker.UpdateScoreEntry(this.GridSize, this.PuzzleImageIndex, this.MoveCount, this.TotalSecondsElapsed);
            HighscoreTracker.Save(this.MainGame.ActiveHighscoreTracker);
            return true;

        }
    }
}
