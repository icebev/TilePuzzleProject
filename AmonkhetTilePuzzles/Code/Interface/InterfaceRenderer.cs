using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* INTERFACE RENDERER
     * Last modified by Joe Bevis 13/01/2022
     ****************************************/

    /// <summary>
    /// A class dedicated to the diplaying of user interface (UI) textures and text
    /// Different UI textures need to be displayed depending on the state of the game
    /// </summary>
    public class InterfaceRenderer
    {
        #region Variables

        private const float CENTRAL_INTERFACE_PROPORTION_X = 0.8f;
        private const float CENTRAL_INTERFACE_PROPORTION_Y = 0.8f;
        private const int SHADOW_OFFSET = 5;

        private TileGame m_mainGame;

        private Texture2D m_obelisks;
        private Texture2D m_sandy;
        private Texture2D m_sandyBanner;
        private Texture2D m_squareContainer;
        private Texture2D m_obelisksShadow;
        private Texture2D m_sandyBannerShadow;
        private Texture2D m_sandyShadow;
        private Texture2D m_squareContainerShadow;
        private Texture2D m_screenDimmer;
        private Texture2D m_trophy;
        private Texture2D m_credits;
        private Texture2D m_instructions;

        private SpriteFont m_calligraphicFont;

        private float m_obelisksScale = 0;
        private float m_obelisksIncrement = 0.01f;

        private float m_bannerYPosition = -300;
        private float m_shiftMultiplier = 0.05f;

        #endregion

        #region Properties

        // Puzzle complete banner animation final location
        private int BannerYTarget
        {
            get
            {
                int val = this.MainGame.WindowHeight / 2 - 300;
                return val;
            }
        }
        private TileGame MainGame
        {
            get { return this.m_mainGame; }
        }
        private GameState ActiveGameState
        {
            get { return this.m_mainGame.ActiveGameState; }
            set { this.m_mainGame.ActiveGameState = value; }
        }

        public int PuzzleContainerSize { get; private set; }
        #endregion

        #region Constructor
        public InterfaceRenderer(TileGame mainGame, SpriteFont font)
        {
            this.m_mainGame = mainGame;
            this.m_calligraphicFont = font;
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// Loads up all the required UI textures 
        /// </summary>
        public void LoadTextures()
        {
            this.m_squareContainer = this.MainGame.Content.Load<Texture2D>("textures/interface/squareContainer");
            this.m_squareContainerShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/squareContainerShadow");
            
            this.m_sandy = this.MainGame.Content.Load<Texture2D>("textures/interface/sandy");
            this.m_sandyShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/sandyShadow");
            
            this.m_obelisks = this.MainGame.Content.Load<Texture2D>("textures/interface/obelisks");
            this.m_obelisksShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/obelisksShadow");

            this.m_sandyBanner = this.MainGame.Content.Load<Texture2D>("textures/interface/sandyBanner");
            this.m_sandyBannerShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/sandyBannerShadow");

            this.m_screenDimmer = this.MainGame.Content.Load<Texture2D>("textures/interface/screenDimmer");

            this.m_trophy = this.MainGame.Content.Load<Texture2D>("textures/interface/trophy");

            this.m_credits = this.MainGame.Content.Load<Texture2D>("textures/interface/credits");
            this.m_instructions = this.MainGame.Content.Load<Texture2D>("textures/interface/puzzleInstructions");
        }

        /// <summary>
        /// Called as part of the game draw cycle to display relevant UI elements
        /// </summary>
        public void DrawInterface(SpriteBatch spriteBatch)
        {
            if (this.ActiveGameState == GameState.MainTitleScreen || this.ActiveGameState == GameState.AnimatedTitleScreen)
            {
                spriteBatch.Draw(this.m_obelisksShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET, this.MainGame.WindowCenter.Y + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X * this.m_obelisksScale), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y * this.m_obelisksScale)),
                    null, Color.White, 0, new Vector2(this.m_obelisksShadow.Width / 2, this.m_obelisksShadow.Height / 2), SpriteEffects.None, 1); 
                
                spriteBatch.Draw(this.m_obelisks, 
                    new Rectangle(this.MainGame.WindowCenter.X, this.MainGame.WindowCenter.Y, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X * this.m_obelisksScale), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y * this.m_obelisksScale)), 
                    null, Color.White, 0, new Vector2(this.m_obelisks.Width / 2, this.m_obelisks.Height / 2), SpriteEffects.None, 1);

                if (this.ActiveGameState == GameState.MainTitleScreen)
                {
                    string text = "Amonkhet Tile Puzzles";
                    spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2), this.MainGame.WindowHeight / 3),  Color.White, 0, new Vector2((float)(this.m_calligraphicFont.MeasureString(text).X * 0.5), (float)(this.m_calligraphicFont.MeasureString(text).Y * 0.5)), 1.75f, SpriteEffects.None, 1.0f);
                }
            }
            
            if ((this.ActiveGameState == GameState.Credits || this.ActiveGameState == GameState.Instructions) || this.ActiveGameState == GameState.OptionsScreen)
            {
                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET, this.MainGame.WindowCenter.Y + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandyShadow.Width / 2, this.m_sandyShadow.Height / 2), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.MainGame.WindowCenter.X, this.MainGame.WindowCenter.Y, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X ), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandy.Width / 2, this.m_sandy.Height / 2), SpriteEffects.None, 1);
            }
            
            if (this.ActiveGameState == GameState.PuzzleActive)
            {
                // HACK if the window becomes too narrow, then the size of the puzzle container will shrink
                // by doing this it will remain fully visible at all times
                int shorterSide = (this.MainGame.WindowWidth * 2 / 3 < this.MainGame.WindowHeight) ? this.MainGame.WindowWidth * 3 / 5 : this.MainGame.WindowHeight;
                int containerSize = 3 * shorterSide / 4;
                this.PuzzleContainerSize = containerSize;

                // Puzzle container
                spriteBatch.Draw(this.m_squareContainerShadow, new Rectangle(this.MainGame.WindowHeight / 8 + SHADOW_OFFSET, this.MainGame.WindowHeight / 8 + SHADOW_OFFSET, containerSize, containerSize), Color.White);
                spriteBatch.Draw(this.m_squareContainer, new Rectangle(this.MainGame.WindowHeight / 8, this.MainGame.WindowHeight / 8, containerSize, containerSize), Color.White);
                if (this.MainGame.ActiveTileManager.m_completeDelayFlag)
                    spriteBatch.Draw(this.MainGame.ActiveTileManager.m_puzzleImage, new Rectangle(this.MainGame.WindowHeight / 8 + Tile.TILE_CONTAINER_PADDING, this.MainGame.WindowHeight / 8 + Tile.TILE_CONTAINER_PADDING, containerSize - Tile.TILE_CONTAINER_PADDING * 2, containerSize - Tile.TILE_CONTAINER_PADDING * 2), Color.White);

                // buttons box
                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET + this.MainGame.WindowHeight / 8, this.MainGame.WindowCenter.Y + this.MainGame.WindowHeight / 16 + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight / 4)), (int)(this.MainGame.WindowHeight * 5 / 16)),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.MainGame.WindowCenter.X + this.MainGame.WindowHeight / 8, this.MainGame.WindowCenter.Y + this.MainGame.WindowHeight / 16, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight / 4)), (int)(this.MainGame.WindowHeight * 5 / 16)),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                // score box
                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.m_mainGame.WindowCenter.X + 15 + this.m_mainGame.WindowHeight * 7 / 16, this.m_mainGame.WindowHeight / 8 + 15 + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight * 9 / 16) - 10), this.m_mainGame.WindowHeight * 5 / 16),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.m_mainGame.WindowCenter.X + 10 + this.m_mainGame.WindowHeight * 7 / 16, this.m_mainGame.WindowHeight / 8 + 15, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight * 9 / 16) - 10), this.m_mainGame.WindowHeight * 5 / 16),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                // Score box text
                spriteBatch.DrawString(this.m_calligraphicFont, $"{this.MainGame.CurrentGridSize} x {this.MainGame.CurrentGridSize}", new Vector2(this.MainGame.WindowCenter.X + 105 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 25), Color.White);
                spriteBatch.DrawString(this.m_calligraphicFont, $"Moves: {this.MainGame.ActiveTileManager.MoveCount}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 85), Color.White);

                if (this.MainGame.ActiveTileManager.HighscoreSet)
                    spriteBatch.DrawString(this.m_calligraphicFont, $"Best Moves: {this.MainGame.ActiveHighscoreTracker.GetBestMoves(this.MainGame.ActiveTileManager.GridSize, this.MainGame.ActiveTileManager.PuzzleImageIndex)}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 125), Color.White);

                if (this.MainGame.ShowTimer)
                {
                    int secondsPassed = this.MainGame.ActiveTileManager.TotalSecondsElapsed;
                    string displaySeconds = (secondsPassed % 60 > 9) ? $"{secondsPassed % 60}" : $"0{secondsPassed % 60}";
                    spriteBatch.DrawString(this.m_calligraphicFont, $"Time: {secondsPassed / 60}:{displaySeconds}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 185), Color.White);
                    if (this.MainGame.ActiveTileManager.HighscoreSet)
                    {
                        int lowestSecondsPassed = this.MainGame.ActiveHighscoreTracker.GetBestTime(this.MainGame.ActiveTileManager.GridSize, this.MainGame.ActiveTileManager.PuzzleImageIndex);
                        string displayLowestSeconds = (lowestSecondsPassed % 60 > 9) ? $"{lowestSecondsPassed % 60}" : $"0{lowestSecondsPassed % 60}";
                        spriteBatch.DrawString(this.m_calligraphicFont, $"Best Time: {lowestSecondsPassed / 60}:{displayLowestSeconds}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 225), Color.White);
                    }
                }
            }
            
            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                string text = "Options";
                spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_calligraphicFont.MeasureString(text).X * 0.5)), 125), Color.White);
                text = "Grid Size:";
                int yPos = (int)(ButtonManager.OPTION_BUTTON_START_Y * this.MainGame.GetWindowScaleFactor().Y);
                spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_calligraphicFont.MeasureString(text).X) - 50), yPos), Color.White);
                text = "Show tile numbers:";
                yPos += (int)(ButtonManager.OPTION_BUTTON_SEPARATION_Y * this.MainGame.GetWindowScaleFactor().Y);
                spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_calligraphicFont.MeasureString(text).X) - 50), yPos), Color.White);
                text = "Show timer:";
                yPos += (int)(ButtonManager.OPTION_BUTTON_SEPARATION_Y * this.MainGame.GetWindowScaleFactor().Y);

                spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_calligraphicFont.MeasureString(text).X) - 50), yPos), Color.White);

            }
            
            if (this.ActiveGameState == GameState.PuzzleSelect)
            {
                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET, this.MainGame.WindowCenter.Y + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandyShadow.Width / 2, this.m_sandyShadow.Height / 2), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.MainGame.WindowCenter.X, this.MainGame.WindowCenter.Y, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandy.Width / 2, this.m_sandy.Height / 2), SpriteEffects.None, 1);

                string text = "Puzzle Select";
                spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_calligraphicFont.MeasureString(text).X * 0.5)), 150), Color.White);

                // Highscore display upon puzzle image button hover
                foreach (Button puzzleImageButton in this.MainGame.ActiveInputManager.ActiveButtonManager.ImageSelectButtons)
                {
                    if (puzzleImageButton.IsHover)
                    {
                        int puzzleImageIndex = this.MainGame.ActiveInputManager.ActiveButtonManager.ImageSelectButtons.IndexOf(puzzleImageButton);
                        int currentGridSize = this.MainGame.CurrentGridSize;

                        bool hasScoreSet = (this.MainGame.ActiveHighscoreTracker.GetRelevantEntries(currentGridSize, puzzleImageIndex).Count > 0);
                        string scoreToDisplay;
                        string timeToDisplay;

                        if (hasScoreSet) 
                        {
                            scoreToDisplay = $"Best Moves:\n {this.MainGame.ActiveHighscoreTracker.GetBestMoves(currentGridSize, puzzleImageIndex)}";
                            int lowestSecondsPassed = this.MainGame.ActiveHighscoreTracker.GetBestTime(currentGridSize, puzzleImageIndex);
                            string displayLowestSeconds = (lowestSecondsPassed % 60 > 9) ? $"{lowestSecondsPassed % 60}" : $"0{lowestSecondsPassed % 60}";
                            timeToDisplay = $"Best Time:\n {lowestSecondsPassed / 60}:{displayLowestSeconds}";
                        }
                        else 
                        {
                            scoreToDisplay = "Best Moves:\n --";
                            timeToDisplay = "Best Time:\n --";
                        }

                        spriteBatch.DrawString(this.m_calligraphicFont, scoreToDisplay, new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 6 / 16, this.MainGame.WindowHeight / 8 + 275), Color.White);

                        spriteBatch.DrawString(this.m_calligraphicFont, timeToDisplay , new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 6 / 16, this.MainGame.WindowHeight / 8 + 375), Color.White);
                    }
                }
            }
            
            if (this.ActiveGameState == GameState.PuzzleComplete)
            {
                spriteBatch.Draw(this.m_screenDimmer, new Rectangle(0, 0, this.MainGame.WindowWidth, this.MainGame.WindowHeight), Color.White);
       
                spriteBatch.Draw(this.m_sandyBannerShadow, new Rectangle(0, (int)this.m_bannerYPosition + SHADOW_OFFSET, this.MainGame.WindowWidth, 200), Color.White);
                spriteBatch.Draw(this.m_sandyBanner, new Rectangle(0, (int)this.m_bannerYPosition, this.MainGame.WindowWidth, 200), Color.White);

                // Only show text once the complete banner has finished moving into position
                if (Math.Abs(this.m_bannerYPosition - this.BannerYTarget) < 2)
                {
                    int puzzleImageIndex = this.MainGame.PuzzleTextures.IndexOf(this.MainGame.ActiveTileManager.m_puzzleImage);
                    int currentGridSize = this.MainGame.CurrentGridSize;
                    string text = "Puzzle Complete!";
                    spriteBatch.DrawString(this.m_calligraphicFont, text, new Vector2((this.MainGame.WindowWidth / 2), this.BannerYTarget + 100), Color.White, 0, new Vector2((float)(this.m_calligraphicFont.MeasureString(text).X * 0.5), (float)(this.m_calligraphicFont.MeasureString(text).Y * 0.5)), 1.75f, SpriteEffects.None, 1.0f);

                    // Award display for getting a highscore
                    if (this.MainGame.ActiveTileManager.MoveCount == this.MainGame.ActiveHighscoreTracker.GetBestMoves(currentGridSize, puzzleImageIndex))
                    {
                        spriteBatch.Draw(this.m_trophy, new Rectangle(335, this.BannerYTarget + 250, 100, 120), Color.White);
                        spriteBatch.DrawString(this.m_calligraphicFont, "New best move count!", new Vector2(445, this.BannerYTarget + 275), Color.White);
                    }
                    
                    if (this.MainGame.ActiveTileManager.TotalSecondsElapsed == this.MainGame.ActiveHighscoreTracker.GetBestTime(currentGridSize, puzzleImageIndex))
                    { 
                        spriteBatch.Draw(this.m_trophy, new Rectangle(this.MainGame.WindowWidth / 2 + 135, this.BannerYTarget + 250, 100, 120), Color.White);
                        spriteBatch.DrawString(this.m_calligraphicFont, "New best time!", new Vector2(this.MainGame.WindowWidth / 2 + 245, this.BannerYTarget + 275), Color.White);
                    }
                }
            }
            
            if (this.ActiveGameState == GameState.Credits || this.ActiveGameState == GameState.Instructions)
            {
                Vector2 origin = new Vector2(this.m_credits.Width / 2, this.m_credits.Height / 2);
                Vector2 position = new Vector2(this.MainGame.WindowWidth / 2, this.MainGame.WindowHeight / 2 - 50);
                Vector2 scaleMultiplier = this.MainGame.GetWindowScaleFactor();
                if (this.ActiveGameState == GameState.Credits)
                {
                    Vector2 scale = new Vector2(0.6f, 0.6f) * scaleMultiplier;
                    spriteBatch.Draw(this.m_credits, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 1.0f);
                }
                else
                {
                    Vector2 scale = new Vector2(0.5f, 0.5f) * scaleMultiplier;
                    spriteBatch.Draw(this.m_instructions, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 1.0f);
                }
            }
        }

        /// <summary>
        /// During the update sequence the animated UI element locations are ticked
        /// They are also reset if the state changes to one in which the element isn't visible
        /// </summary>
        public void UpdateIt(GameTime gameTime)
        {
            if ((int)(this.ActiveGameState) > 4)
            {
                this.m_obelisksScale = 0;
                this.m_obelisksIncrement = 0.01f;
            }

            if (this.ActiveGameState == GameState.AnimatedTitleScreen)
            {
                if (this.m_obelisksScale < 1)
                {
                    this.m_obelisksScale += (float)gameTime.ElapsedGameTime.TotalSeconds * this.m_obelisksIncrement;
                    this.m_obelisksIncrement += this.m_obelisksScale;
                }
                else
                {
                    this.ActiveGameState = GameState.MainTitleScreen;
                }
            }
            else if (this.ActiveGameState == GameState.PuzzleComplete)
            {
                var dy = this.BannerYTarget - this.m_bannerYPosition;
                this.m_bannerYPosition += dy * this.m_shiftMultiplier;
            } 
            else
            {
                this.m_bannerYPosition = -300;
            }
        }
        #endregion
    }
}
