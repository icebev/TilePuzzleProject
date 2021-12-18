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
        private const float CENTRAL_INTERFACE_PROPORTION_X = 0.7f;
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
            this.m_squareContainer = this.m_mainGame.Content.Load<Texture2D>("textures/interface/squareContainer");
            this.m_squareContainerShadow = this.m_mainGame.Content.Load<Texture2D>("textures/shadows/squareContainerShadow");
            
            this.m_sandy = this.m_mainGame.Content.Load<Texture2D>("textures/interface/sandy");
            this.m_sandyShadow = this.m_mainGame.Content.Load<Texture2D>("textures/shadows/sandyShadow");
            
            this.m_obelisks = this.m_mainGame.Content.Load<Texture2D>("textures/interface/obelisks");
            this.m_obelisksShadow = this.m_mainGame.Content.Load<Texture2D>("textures/shadows/obelisksShadow");

        }

        public void DrawInterface(SpriteBatch spriteBatch)
        {
            if (this.ActiveGameState == GameState.TitleScreen)
            {
                spriteBatch.Draw(this.m_obelisksShadow,
                    new Rectangle(this.m_mainGame.WindowCenter.X + SHADOW_OFFSET, this.m_mainGame.WindowCenter.Y + SHADOW_OFFSET, (int)(this.m_mainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X * this.m_obelisksScale), (int)(this.m_mainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y * this.m_obelisksScale)),
                    null, Color.White, 0, new Vector2(this.m_obelisksShadow.Width / 2, this.m_obelisksShadow.Height / 2), SpriteEffects.None, 1); 
                
                spriteBatch.Draw(this.m_obelisks, 
                    new Rectangle(this.m_mainGame.WindowCenter.X, this.m_mainGame.WindowCenter.Y, (int)(this.m_mainGame.WindowWidth * CENTRAL_INTERFACE_PROPORTION_X * this.m_obelisksScale), (int)(this.m_mainGame.WindowHeight * CENTRAL_INTERFACE_PROPORTION_Y * this.m_obelisksScale)), 
                    null, Color.White, 0, new Vector2(this.m_obelisks.Width / 2, this.m_obelisks.Height / 2), SpriteEffects.None, 1);

                if (this.m_obelisksScale >= 1)
                {
                    string text = "Amonkhet Tile Puzzles";
                    spriteBatch.DrawString(this.m_bahnschriftFont, text, new Vector2((this.m_mainGame.WindowWidth / 2 - (int)(this.m_bahnschriftFont.MeasureString(text).X * 0.5)), this.m_mainGame.WindowHeight / 3), Color.White);
                }
            }


            if (this.ActiveGameState == GameState.PuzzleActive)
            {
                spriteBatch.Draw(this.m_squareContainerShadow, new Rectangle(150 - 15, 150 - 15, 450 + 60, 450 + 60), Color.White);
                spriteBatch.Draw(this.m_squareContainer, new Rectangle(150 - 20, 150 - 20, 450 + 60, 450 + 60), Color.White);
            }

            if (this.ActiveGameState == GameState.PuzzleSelect)
            {
                spriteBatch.Draw(this.m_sandyShadow, new Rectangle(150 - 15, 150 - 15, 450 + 60, 450 + 60), Color.White);
                spriteBatch.Draw(this.m_sandy, new Rectangle(150 - 20, 150 - 20, 450 + 60, 450 + 60), Color.White);

            }
        }

        public void UpdateIt(GameTime gameTime)
        {
            if (this.ActiveGameState != GameState.TitleScreen)
            {
                this.m_obelisksScale = 0;
                this.m_obelisksIncrement = 0.01f;
            }

            if (this.TitleState == TitleScreenState.Animated)
            {

                if (this.m_obelisksScale < 1)
                {
                    this.m_obelisksScale += (float)gameTime.ElapsedGameTime.TotalSeconds * this.m_obelisksIncrement;
                    this.m_obelisksIncrement += this.m_obelisksScale;
                }
                else
                {
                    this.TitleState = TitleScreenState.Main;
                }
            }


        }
    }
}
