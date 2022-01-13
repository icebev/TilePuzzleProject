using Microsoft.Xna.Framework;
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
    /* INPUT MANAGER CLASS
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/

    /// <summary>
    /// The input manager is instantiated by the game and is used to process player inputs
    /// Mouse clicks and key presses are detected using state comparisons and then handled according to the manager
    /// </summary>
    public class InputManager
    {
        #region Variables

        private TileGame m_mainGame;
        private ButtonManager m_buttonManager;
        #endregion

        #region Properties
        private TileGame MainGame
        {
            get { return this.m_mainGame; }
        }
        private TileManager ActiveTileManager
        {
            get { return this.MainGame.ActiveTileManager;  }
        }
        public ButtonManager ActiveButtonManager
        {
            get { return this.m_buttonManager;  }
        }
        private GameState ActiveGameState
        {
            get { return this.m_mainGame.ActiveGameState; }
            set { this.m_mainGame.ActiveGameState = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// The input manager needs a reference to the main game so that it can communicate
        /// A button manager instance is created inside the manager because buttons are related directly to input
        /// </summary>
        public InputManager(TileGame mainGame)
        {
            this.m_mainGame = mainGame;
            this.m_buttonManager = new ButtonManager(mainGame);
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Currently the only visible part of the input manager system is the buttons
        /// </summary>
        public void DrawIt(SpriteBatch spriteBatch)
        {
            this.m_buttonManager.DrawButtons(spriteBatch);
        }

        /// <summary>
        /// The process controls is called once per game update cycle 
        /// it will check wether the mouse has been clicked or a keyboard button has been pressed
        /// </summary>
        public void ProcessControls(MouseState previousMouseState, MouseState currentMouseState, 
            KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, GameTime gameTime)
        {
            // Buttons need to be updated as part of the game update cycle
            this.ActiveButtonManager.UpdateButtons(gameTime, currentMouseState);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                this.MouseClickResponse(currentMouseState);
            }

            if (currentKeyboardState != previousKeyboardState)
            {
                this.KeyPressResonse(previousKeyboardState, currentKeyboardState);
            }
        }

        /// <summary>
        /// Called when the left mouse button is pressed
        /// </summary>
        private void MouseClickResponse(MouseState currentMouseState)
        {
            this.ActiveButtonManager.CheckIfButtonsClicked(currentMouseState);

            // Only check if a tile has been clicked if the puzzle is not complete
            if (this.ActiveGameState == GameState.PuzzleActive && !this.ActiveTileManager.m_puzzleComplete)
            {
                foreach (IGridMember tile in this.ActiveTileManager.TilesArray)
                {
                    if (tile.IsCurrentlySwappable && tile.GetType().ToString() == "AmonkhetTilePuzzles.Tile")
                    {
                        if (tile.TileBounds.Contains(currentMouseState.Position))
                        {
                            this.ActiveTileManager.SwapTile(tile);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when any keyboard key is pressed (causing a change in Keyboard state)
        /// </summary>
        private void KeyPressResonse(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState)
        {
            // To be used for debug (create a 2x2 which is very quick to solve)
            if (this.ActiveGameState == GameState.MainTitleScreen || this.ActiveGameState == GameState.PuzzleActive)
            {
                if (currentKeyboardState.IsKeyDown(Keys.NumPad2) && !previousKeyboardState.IsKeyDown(Keys.NumPad2))
                {
                    this.MainGame.SetupTileGrid(this.MainGame.RandomPuzzleTexture, 2);
                }
            }


            // Arrow key press checks
            if (this.ActiveGameState == GameState.PuzzleActive)
            {
                if (currentKeyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                {
                    if (this.ActiveTileManager.BlankTilePosition.Y > 0)
                    {
                        this.ActiveTileManager.SwapTile(this.ActiveTileManager.TilesArray[this.ActiveTileManager.BlankTilePosition.X, this.ActiveTileManager.BlankTilePosition.Y - 1]);
                    }
                }

                else if (currentKeyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                {
                    if (this.ActiveTileManager.BlankTilePosition.Y < this.ActiveTileManager.GridSize - 1)
                    {
                        this.ActiveTileManager.SwapTile(this.ActiveTileManager.TilesArray[this.ActiveTileManager.BlankTilePosition.X, this.ActiveTileManager.BlankTilePosition.Y + 1]);
                    }
                }

                else if (currentKeyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                {
                    if (this.ActiveTileManager.BlankTilePosition.X > 0)
                    {
                        this.ActiveTileManager.SwapTile(this.ActiveTileManager.TilesArray[this.ActiveTileManager.BlankTilePosition.X - 1, this.ActiveTileManager.BlankTilePosition.Y]);
                    }
                }

                else if (currentKeyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
                {
                    if (this.ActiveTileManager.BlankTilePosition.X < this.ActiveTileManager.GridSize - 1)
                    {
                        this.ActiveTileManager.SwapTile(this.ActiveTileManager.TilesArray[this.ActiveTileManager.BlankTilePosition.X + 1, this.ActiveTileManager.BlankTilePosition.Y]);
                    }
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
            {
                if (this.ActiveGameState == GameState.MainTitleScreen)
                    this.MainGame.Exit();
                else if (this.ActiveGameState == GameState.PuzzleActive)
                {
                    this.ActiveGameState = GameState.AnimatedTitleScreen;
                }
            }
        }

        #endregion

    }
}
