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
    /* TILE MANAGER CLASS
     * Last modified by Joe Bevis 13/01/2022
     ****************************************/

    /// <summary>
    /// The tile manager is used to keep track of each tile in the tile grid during gameplay
    /// When the grid size changes, a new tile manager can be created specifying a different grid size
    /// Tile puzzle game functionality is contained within the class methods
    /// </summary>
    public class TileManager
    {
        #region Member Variables

        private int m_gridSize;

        private IGridMember[,] m_tilesArray;
        private List<IGridMember> m_tilesList;

        public Texture2D m_puzzleImage;
        private Texture2D m_tileShadow;
        private SpriteFont m_font;

        public bool m_completeDelayFlag;
        public bool m_puzzleComplete;
        
        private int m_moves = 0;
        private float m_secondsElapsed = 0;

        // How many seconds before moving to the the completion screen after the puzzle has been completed
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

        #region Constructor
        public TileManager(TileGame mainGame, int gridSize, Texture2D picture, SpriteFont font, Texture2D tileShadow)
        {
            this.m_random = new Random();
            this.m_mainGame = mainGame;
            this.m_puzzleImage = picture;
            this.m_tileShadow = tileShadow;
            this.m_font = font;

            this.GridSize = gridSize;
            
            /*
             * The tiles array can be used to keep track of positions and update the tile current positions.
             * The tiles list contains references to the same tile objects
             * and is used to iterate easily through each tile currently in the grid
             * *************************************************************************************************/
            this.TilesArray = new IGridMember[this.m_gridSize, this.m_gridSize];
            this.TilesList = new List<IGridMember>();
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// Fills the tiles array and list with the tile objects 
        /// This must be done before attemting to swap any tiles
        /// </summary>
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
                    // The blank tile will always be created at the bottom left of the array - at the maximum values
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

        /// <summary>
        /// Called as part of the game update cycle
        /// Each tile needs to be updated for their animations 
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateTiles(GameTime gameTime)
        {
            // A delay before switching to the puzzle complete screen
            // allows the player to take time to recognise completion
            if (!this.m_puzzleComplete && !this.m_completeDelayFlag)
                this.m_secondsElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!this.m_completeDelayFlag && this.m_puzzleComplete)
                this.m_completeAnimationDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                this.m_completeAnimationDelay = COMPLETION_FLAG_DELAY_SECONDS;

            if (this.m_completeAnimationDelay <= 0 && !this.m_completeDelayFlag)
            {
                this.MainGame.ActiveGameState = GameState.PuzzleComplete;
                if (!AudioStore.IsMuted)
                    AudioStore.m_puzzleCompleteSFX.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);
                this.m_completeDelayFlag = true;
            }

            foreach (IGridMember tile in this.m_tilesArray)
            {
                if (tile.GetType().ToString() == "AmonkhetTilePuzzles.Tile")
                    tile.UpdateIt(gameTime);
            }
        }

        /// <summary>
        /// Draws the tiles via each tile's draw method
        /// </summary>
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

        /// <summary>
        /// Gets the location of the blank tile within the tiles array
        /// </summary>
        /// <returns>A point containing the array location of the blank tile</returns>
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
            throw new NullReferenceException("Blank tile not found in m_tilesArray");
            //return new Point(0, 0);
        }
        /// <summary>
        /// Uses the blank tile position to determine which tiles should be swappable
        /// Only directly adjacent tiles (to the blank) should have IsCurrentlySwappable set to true
        /// </summary>
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
                        }
                        else
                        {
                            tile.IsCurrentlySwappable = false;
                        }
                    }
            }
        }

        /// <summary>
        /// Swaps the tile with the blank space in the array and updates its current position
        /// </summary>
        /// <param name="tileToSwap">The tile to be moved into the blank space</param>
        public void SwapTile(IGridMember tileToSwap)
        {
            // Do not play slide sfx when shuffling
            if (!this.IsShuffling && !AudioStore.IsMuted)
                AudioStore.m_tileSlideSFX.Play(volume: 0.5f, pitch: 0.0f, pan: 0.0f);

            // The blank tile will move into the selected tile's current position - this is recorded before the move
            Point newBlankPosition = new Point(tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y);

            // Sets the position of the tile to be moved to the blank tile position and update the array to keep track of all tiles
            tileToSwap.CurrentGridPosition = this.FindBlankTile();
            this.TilesArray[tileToSwap.CurrentGridPosition.X, tileToSwap.CurrentGridPosition.Y] = tileToSwap;

            // Creates a new blank tile in the old position of the moved tile and finally overwrites the array 
            BlankTile newBlankTile = new BlankTile(new Point(this.m_gridSize - 1, this.m_gridSize - 1), this.m_gridSize);
            newBlankTile.CurrentGridPosition = newBlankPosition;
            this.TilesArray[newBlankPosition.X, newBlankPosition.Y] = newBlankTile;

            this.MoveCount++;

            // After each move this needs to be called because different tiles are now swappable
            this.DetermineSwappableTiles();

            // Do not check for puzzle completion during shuffling
            if (!this.IsShuffling)
                this.CheckPuzzleCompletion();
        }

        /// <summary>
        /// For shuffling the tiles - 500 random moves are made on the puzzle.
        /// Each move needs to be legal to prevent the puzzle from becoming unsolveable
        /// Will be called after the shuffle / reset button is clicked and during instantiation
        /// </summary>
        public void JumbleTiles()
        {
            this.IsShuffling = true;
            for (int i = 0; i < 500; i++)
            {
                IEnumerable<IGridMember> moveableTiles =
                    from tile in this.m_tilesList
                    where tile.IsCurrentlySwappable
                    select tile;

                int randomSelector = this.m_random.Next(0, moveableTiles.Count());
                this.SwapTile(moveableTiles.ToList()[randomSelector]);
            }
            this.IsShuffling = false;
            this.MoveCount = 0;
            this.m_secondsElapsed = 0;
            this.m_puzzleComplete = false;
            this.m_completeDelayFlag = false;
        }
        /// <summary>
        /// After a tile swap, each tile will be checked to see it if it is in the correct position
        /// If all tiles are in their correct positions within the array, the puzzle is complete
        /// </summary>
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
            this.MainGame.ActiveHighscoreTracker.UpdateScoreEntry(this.GridSize, this.PuzzleImageIndex, this.MoveCount, this.TotalSecondsElapsed);
            HighscoreTracker.Save(this.MainGame.ActiveHighscoreTracker);
            return true;
        }
        #endregion
    }
}
