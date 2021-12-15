﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    class InputManager
    {
        private TileTestGame m_mainGame;

        private TileTestGame MainGame
        {
            get { return this.m_mainGame; }
        }
        private TileManager ActiveTileManager
        {
            get { return this.MainGame.ActiveTileManager;  }
        }
        
        private GameState ActiveGameState
        {
            get { return this.m_mainGame.ActiveGameState; }
            set { this.m_mainGame.ActiveGameState = value; }
        }

        public InputManager(TileTestGame mainGame, TileManager tileManager)
        {
            this.m_mainGame = mainGame;
            
        }

        public void ProcessControls(MouseState previousMouseState, MouseState currentMouseState, 
            KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                this.MouseClickResponse(currentMouseState, gameTime);
            }

            if (currentKeyboardState != previousKeyboardState)
            {
                this.KeyPressResonse(previousKeyboardState, currentKeyboardState, gameTime);
            }
        }

        private void MouseClickResponse(MouseState currentMouseState, GameTime gameTime)
        {
            Debug.WriteLine("Click detected!");
            if (this.ActiveGameState == GameState.PuzzleActive)
            {
                foreach (IGridMember tile in this.ActiveTileManager.TilesArray)
                {
                    if (tile.IsCurrentlySwappable && tile.GetType().ToString() == "TileTest.Tile")
                    {
                        if (tile.TileBounds.Contains(currentMouseState.Position))
                        {
                            this.ActiveTileManager.SwapTile(tile);
                        }
                    }
                }
            }
        }

        private void KeyPressResonse(KeyboardState previousKeyboardState, KeyboardState currentKeyboardState, GameTime gameTime)
        {
            if (this.ActiveGameState == GameState.TitleScreen || this.ActiveGameState == GameState.PuzzleActive)
            {
                if (currentKeyboardState.IsKeyDown(Keys.NumPad2) && !previousKeyboardState.IsKeyDown(Keys.NumPad2))
                {
                    this.MainGame.SetupTileGrid(2, this.MainGame.RandomPuzzleTexture);
                }

                if (currentKeyboardState.IsKeyDown(Keys.NumPad3) && !previousKeyboardState.IsKeyDown(Keys.NumPad3))
                {
                    this.MainGame.SetupTileGrid(3, this.MainGame.RandomPuzzleTexture);
                }

                if (currentKeyboardState.IsKeyDown(Keys.NumPad4) && !previousKeyboardState.IsKeyDown(Keys.NumPad4))
                {
                    this.MainGame.SetupTileGrid(4, this.MainGame.RandomPuzzleTexture);
                }
            }

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
                if (this.ActiveGameState == GameState.TitleScreen)
                    this.MainGame.Exit();
                else if (this.ActiveGameState == GameState.PuzzleActive)
                    this.ActiveGameState = GameState.TitleScreen;
                    
            }
        }

    }
}