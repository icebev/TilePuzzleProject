using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class ButtonManager
    {
        private TileTestGame m_mainGame;
        public List<Button> m_buttonRoster;

        private Texture2D m_normalTexture;
        private Texture2D m_hoverTexture;
        private Texture2D m_clickTexture;

        private SpriteFont m_bahnschriftFont;
        private Button testImageButton;

        public ButtonManager(TileTestGame mainGame)
        {
            this.m_mainGame = mainGame;
            this.m_bahnschriftFont = this.m_mainGame.m_bahnschriftFont;
            this.m_normalTexture = this.m_mainGame.Content.Load<Texture2D>("textures/button/normal");
            this.m_hoverTexture = this.m_mainGame.Content.Load<Texture2D>("textures/button/hover");
            this.m_clickTexture = this.m_mainGame.Content.Load<Texture2D>("textures/button/click");

            this.m_buttonRoster = new List<Button>();

            var titleScreenButton = new Button(new Vector2(700, 500), 400, 100, "Start Game");
            titleScreenButton.OnClick += this.TitleScreenButton_OnClick;
            this.m_buttonRoster.Add(titleScreenButton);

            this.testImageButton = new Button(new Vector2(300, 500), 100, 100);
            this.testImageButton.OnClick += this.TitleScreenButton_OnClick;

        }

        private void TitleScreenButton_OnClick(object sender, EventArgs e)
        {
            this.m_mainGame.ActiveGameState = GameState.PuzzleSelect;
        }

        public void UpdateButtons(GameTime gameTime, MouseState currentMouseState)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                button.UpdateIt(gameTime, currentMouseState);
            }
            this.testImageButton.UpdateIt(gameTime, currentMouseState);

        }
        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_bahnschriftFont);
            }
            this.testImageButton.DrawIt(spriteBatch, this.m_mainGame.PuzzleTextures[0]);
        }

        public void CheckIfButtonsClicked(MouseState currentMouseState)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                button.CheckIfClicked(currentMouseState);
            }
            this.testImageButton.CheckIfClicked(currentMouseState);
        }
    }

}
