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

                

            var titleScreenButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 - 200, 500), 400, 100,"start", "Start Game");
            titleScreenButton.OnClick += this.TitleScreenButton_OnClick;

            var fullScreenButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH - 600, 25), 250, 75, "fullscreen", "Fullscreen");
            fullScreenButton.OnClick += this.FullScreenButton_OnClick;            
            
            var muteButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH - 300, 25), 250, 75, "mute", "Mute");
            muteButton.OnClick += this.MuteButton_OnClick;

            this.m_buttonRoster = new List<Button>()
            {
                titleScreenButton,
                fullScreenButton,
                muteButton
            };

            this.testImageButton = new Button(new Vector2(300, 500), 100, 100, "testimage");
            this.testImageButton.OnClick += this.TitleScreenButton_OnClick;

        }

        private void MuteButton_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FullScreenButton_OnClick(object sender, EventArgs e)
        {
            this.m_mainGame.m_graphics.IsFullScreen = !this.m_mainGame.m_graphics.IsFullScreen;
            this.m_mainGame.m_graphics.ApplyChanges();
        }

        private void TitleScreenButton_OnClick(object sender, EventArgs e)
        {
            this.m_mainGame.ActiveGameState = GameState.PuzzleSelect;
            
        }

        public void UpdateButtons(GameTime gameTime, MouseState currentMouseState)
        {
            this.m_buttonRoster[0].IsVisible = false;
            if (this.m_mainGame.m_interfaceRenderer.TitleState == TitleScreenState.Main)
                this.m_buttonRoster[0].IsVisible = true;

            foreach (Button button in this.m_buttonRoster)
            {
                button.UpdateIt(gameTime, currentMouseState, this.m_mainGame.GetWindowScaleFactor());
            }
            this.testImageButton.UpdateIt(gameTime, currentMouseState, this.m_mainGame.GetWindowScaleFactor());

        }
        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                if (button.IsVisible)
                {
                    button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_bahnschriftFont);
                }
            }
            this.testImageButton.DrawIt(spriteBatch, this.m_mainGame.PuzzleTextures[0]);
        }

        public void CheckIfButtonsClicked(MouseState currentMouseState)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                if (button.IsVisible)
                {
                    button.CheckIfClicked(currentMouseState);
                }
            }
            this.testImageButton.CheckIfClicked(currentMouseState);
        }
    }

}
