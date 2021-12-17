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
        private TileTestGame m_mainGame;

        private Texture2D m_obelisks;
        private Texture2D m_sandy;
        private Texture2D m_squareContainer;

        private Texture2D m_obelisksShadow;
        private Texture2D m_sandyShadow;
        private Texture2D m_squareContainerShadow;

        private SpriteFont m_bahnschriftFont;

        private float m_obelisksScale = 0;
        private float m_obelisksScaleMultiplier = 0.01f;

        

        private GameState ActiveGameState
        {
            get { return this.m_mainGame.ActiveGameState; }
            set { this.m_mainGame.ActiveGameState = value; }
        }

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
                spriteBatch.Draw(this.m_obelisksShadow, new Rectangle(150 - 5, 150 - 10, (int)(this.m_obelisks.Width * this.m_obelisksScale), (int)(this.m_obelisks.Height * this.m_obelisksScale)), Color.White);
                spriteBatch.Draw(this.m_obelisks, new Rectangle(150 - 15, 150 - 15, (int)(this.m_obelisks.Width* this.m_obelisksScale), (int)(this.m_obelisks.Height * this.m_obelisksScale)), Color.White);
                if (this.m_obelisksScale >= 1)
                    spriteBatch.DrawString(this.m_bahnschriftFont, "Amonkhet Tile Puzzles", new Vector2(400, 300), Color.White);
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
                this.m_obelisksScaleMultiplier = 0.01f;
            }
            

            if (this.m_obelisksScale < 1)
            {
                this.m_obelisksScale += (float)gameTime.ElapsedGameTime.TotalSeconds * this.m_obelisksScaleMultiplier;
                this.m_obelisksScaleMultiplier += this.m_obelisksScale;
            }


        }
    }
}
