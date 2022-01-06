using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class InterfaceRenderer
    {
        private const float CENTRAL_INTERFACE_PROPORTION_X = 0.8f;
        private const float CENTRAL_INTERFACE_PROPORTION_Y = 0.8f;
        private const int SHADOW_OFFSET = 5;
        private TileTestGame m_mainGame;

        private Texture2D m_obelisks;
        private Texture2D m_sandy;
        private Texture2D m_squareContainer;

        private Texture2D m_obelisksShadow;
        private Texture2D m_sandyShadow;
        private Texture2D m_squareContainerShadow;

        private SpriteFont m_bahnschriftFont;

        private float m_obelisksScale = 0;
        private float m_obelisksIncrement = 0.01f;

        private TitleScreenState m_titleState = TitleScreenState.Animated;

        private TileTestGame MainGame
        {
            get { return this.m_mainGame; }
        }
        private GameState ActiveGameState
        {
            get { return this.m_mainGame.ActiveGameState; }
            set { this.m_mainGame.ActiveGameState = value; }
        }

        public TitleScreenState TitleState { get => this.m_titleState; set => this.m_titleState = value; }

        public InterfaceRenderer(TileTestGame mainGame, SpriteFont font)
        {
            this.m_mainGame = mainGame;
            this.m_bahnschriftFont = font;
        }

        public void LoadTextures()
        {
            this.m_squareContainer = this.MainGame.Content.Load<Texture2D>("textures/interface/squareContainer");
            this.m_squareContainerShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/squareContainerShadow");
            
            this.m_sandy = this.MainGame.Content.Load<Texture2D>("textures/interface/sandy");
            this.m_sandyShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/sandyShadow");
            
            this.m_obelisks = this.MainGame.Content.Load<Texture2D>("textures/interface/obelisks");
            this.m_obelisksShadow = this.MainGame.Content.Load<Texture2D>("textures/shadows/obelisksShadow");

        }

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
                    spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X * 0.5)), this.MainGame.WindowHeight / 3), Color.White);
                }
            }
            else if ((this.ActiveGameState == GameState.Credits || this.ActiveGameState == GameState.Instructions) || this.ActiveGameState == GameState.OptionsScreen)
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
                int shorterSide = (this.MainGame.WindowWidth * 2 / 3 < this.MainGame.WindowHeight) ? this.MainGame.WindowWidth * 3 / 5 : this.MainGame.WindowHeight;
                int containerSize = 3 * shorterSide / 4;
                spriteBatch.Draw(this.m_squareContainerShadow, new Rectangle(this.MainGame.WindowHeight / 8 + SHADOW_OFFSET, this.MainGame.WindowHeight / 8 + SHADOW_OFFSET, containerSize, containerSize), Color.White);
                spriteBatch.Draw(this.m_squareContainer, new Rectangle(this.MainGame.WindowHeight / 8, this.MainGame.WindowHeight / 8, containerSize, containerSize), Color.White);

                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET + this.MainGame.WindowHeight / 8, this.MainGame.WindowCenter.Y + this.MainGame.WindowHeight / 16 + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight / 4)), (int)(this.MainGame.WindowHeight * 5 / 16)),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.MainGame.WindowCenter.X + this.MainGame.WindowHeight / 8, this.MainGame.WindowCenter.Y + this.MainGame.WindowHeight / 16, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight / 4)), (int)(this.MainGame.WindowHeight * 5 / 16)),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                //score box
                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.m_mainGame.WindowCenter.X + 15 + this.m_mainGame.WindowHeight * 7 / 16, this.m_mainGame.WindowHeight / 8 + 15 + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight * 9 / 16) - 10), this.m_mainGame.WindowHeight * 5 / 16),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.m_mainGame.WindowCenter.X + 10 + this.m_mainGame.WindowHeight * 7 / 16, this.m_mainGame.WindowHeight / 8 + 15, (int)(this.MainGame.WindowWidth / 2 - (this.MainGame.WindowHeight * 9 / 16) - 10), this.m_mainGame.WindowHeight * 5 / 16),
                    null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                // text
                spriteBatch.DrawString(this.m_bahnschriftFont, $"{this.MainGame.CurrentGridSize} x {this.MainGame.CurrentGridSize}", new Vector2(this.MainGame.WindowCenter.X + 105 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 25), Color.White);
                spriteBatch.DrawString(this.m_bahnschriftFont, $"Moves: {this.MainGame.ActiveTileManager.MoveCount}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 85), Color.White);

                if (this.MainGame.ActiveTileManager.HighscoreSet)
                    spriteBatch.DrawString(this.m_bahnschriftFont, $"Best Moves: {this.MainGame.ActiveHighscoreTracker.GetBestMoves(this.MainGame.ActiveTileManager.GridSize, this.MainGame.ActiveTileManager.PuzzleImageIndex)}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 125), Color.White);

                if (this.MainGame.ShowTimer)
                {
                    int secondsPassed = this.MainGame.ActiveTileManager.TotalSecondsElapsed;
                    string displaySeconds = (secondsPassed % 60 > 9) ? $"{secondsPassed % 60}" : $"0{secondsPassed % 60}";
                    spriteBatch.DrawString(this.m_bahnschriftFont, $"Time: {secondsPassed / 60}:{displaySeconds}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 185), Color.White);
                    if (this.MainGame.ActiveTileManager.HighscoreSet)
                    {
                        int lowestSecondsPassed = this.MainGame.ActiveHighscoreTracker.GetBestTime(this.MainGame.ActiveTileManager.GridSize, this.MainGame.ActiveTileManager.PuzzleImageIndex);
                        string displayLowestSeconds = (lowestSecondsPassed % 60 > 9) ? $"{lowestSecondsPassed % 60}" : $"0{lowestSecondsPassed % 60}";
                        spriteBatch.DrawString(this.m_bahnschriftFont, $"Best Time: {lowestSecondsPassed / 60}:{displayLowestSeconds}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 7 / 16, this.MainGame.WindowHeight / 8 + 225), Color.White);
                    }
                }
            }

            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                string text = "Options";
                spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X * 0.5)), 125), Color.White);
                text = "Grid Size:";
                spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X) - 50), 225), Color.White);
                text = "Show tile numbers:";
                spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X) - 50), 375), Color.White);
                text = "Show timer:";
                spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X) - 50), 525), Color.White);

            }
            // new Rectangle(this.m_mainGame.WindowCenter.X + this.m_mainGame.WindowHeight / 8, this.m_mainGame.WindowHeight / 8 + 15, this.m_mainGame.WindowHeight * 5 / 16, this.m_mainGame.WindowHeight * 5 / 16)

            if (this.ActiveGameState == GameState.PuzzleSelect)
            {
                //spriteBatch.Draw(this.m_sandyShadow, new Rectangle(150 - 15, 150 - 15, 450 + 60, 450 + 60), Color.White);
                //spriteBatch.Draw(this.m_sandy, new Rectangle(150 - 20, 150 - 20, 450 + 60, 450 + 60), Color.White);

                spriteBatch.Draw(this.m_sandyShadow,
                    new Rectangle(this.MainGame.WindowCenter.X + SHADOW_OFFSET, this.MainGame.WindowCenter.Y + SHADOW_OFFSET, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandyShadow.Width / 2, this.m_sandyShadow.Height / 2), SpriteEffects.None, 1);

                spriteBatch.Draw(this.m_sandy,
                    new Rectangle(this.MainGame.WindowCenter.X, this.MainGame.WindowCenter.Y, (int)(this.MainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X), (int)(this.MainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y)),
                    null, Color.White, 0, new Vector2(this.m_sandy.Width / 2, this.m_sandy.Height / 2), SpriteEffects.None, 1);

                string text = "Puzzle Select";
                spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.MainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X * 0.5)), 150), Color.White);
                
                foreach (Button puzzleImageButton in this.MainGame.ActiveInputManager.ActiveButtonManager.m_imageSelectButtons)
                {
                    if (puzzleImageButton.IsHover)
                    {
                        int puzzleImageIndex = this.MainGame.ActiveInputManager.ActiveButtonManager.m_imageSelectButtons.IndexOf(puzzleImageButton);
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

                        spriteBatch.DrawString(this.m_bahnschriftFont, $"{this.MainGame.CurrentGridSize} x {this.MainGame.CurrentGridSize}", new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 6 / 16, this.MainGame.WindowHeight / 8 + 165), Color.White);


                        spriteBatch.DrawString(this.m_bahnschriftFont, scoreToDisplay, new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 6 / 16, this.MainGame.WindowHeight / 8 + 275), Color.White);

                        
                        spriteBatch.DrawString(this.m_bahnschriftFont, timeToDisplay , new Vector2(this.MainGame.WindowCenter.X + 20 + this.MainGame.WindowHeight * 6 / 16, this.MainGame.WindowHeight / 8 + 375), Color.White);

                    }
                }
            }
        }

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


        }
    }
}
