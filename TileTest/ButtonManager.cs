using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class ButtonManager
    {
        private const int IMAGE_GRID_COLUMNS = 3;
        private TileTestGame m_mainGame;
        public List<Button> m_buttonRoster;
        public List<Button> m_imageSelectButtons;
        private List<ToggleButton> m_difficultyToggleButtons;
        private Texture2D m_normalTexture;
        private Texture2D m_hoverTexture;
        private Texture2D m_clickTexture;

        private SpriteFont m_bahnschriftFont;
        //private Button testImageButton;

        private TileTestGame MainGame
        {
            get { return this.m_mainGame; }
        }

        public ButtonManager(TileTestGame mainGame)
        {
            this.m_mainGame = mainGame;
            this.m_bahnschriftFont = this.MainGame.m_bahnschriftFont;
            this.m_normalTexture = this.MainGame.Content.Load<Texture2D>("textures/button/normal");
            this.m_hoverTexture = this.MainGame.Content.Load<Texture2D>("textures/button/hover");
            this.m_clickTexture = this.MainGame.Content.Load<Texture2D>("textures/button/click");
            this.m_imageSelectButtons = new List<Button>();
            this.m_difficultyToggleButtons = new List<ToggleButton>();


            for (int i = 0; i < this.MainGame.PuzzleTextures.Count(); i++)
            {
                int gridX = i % IMAGE_GRID_COLUMNS;
                int gridY = (int)Math.Floor((float)(i / IMAGE_GRID_COLUMNS));
                var newImageButton = new Button(new Vector2(400 + 250 * gridX, 250 + 175 * gridY), 150, 150, new GameState[] { GameState.PuzzleSelect });
                newImageButton.OnClick += this.ImageButton_OnClick;
                this.m_imageSelectButtons.Add(newImageButton);
            }

            for (int i = 0; i < 3; i++)
            {
                string text = $"{i + 3} x {i + 3}";
                var newDifficiltyButton = new ToggleButton(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 + (200 * i), 300), 200, 100, new GameState[] { GameState.OptionsScreen }, text);
                newDifficiltyButton.OnToggle += this.DifficultyButton_OnToggle;
                this.m_difficultyToggleButtons.Add(newDifficiltyButton);
            }

            var titleScreenButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 - 200, 400), 400, 100, new GameState[1] { GameState.MainTitleScreen }, "Start Game");
            titleScreenButton.OnClick += this.GoToPuzzleSelect_OnClick;
            
            var creditsButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 3 - 75, 650), 300, 70, new GameState[1] { GameState.MainTitleScreen }, "Credits");
            creditsButton.OnClick += this.CreditsButton_OnClick;

            var instructionsButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH * 2 / 3 - 225, 650), 300, 70, new GameState[1] { GameState.MainTitleScreen }, "Instructions");
            instructionsButton.OnClick += this.InstructionsButton_OnClick; ;

            var backButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 - 150, 650), 300, 70, new GameState[3] { GameState.Credits, GameState.Instructions, GameState.OptionsScreen }, "Back");
            backButton.OnClick += this.BackButton_OnClick;

            var fullScreenButton = new ToggleButton(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH - 600, 15), 250, 55, new GameState[2] { GameState.MainTitleScreen, GameState.PuzzleActive }, "Fullscreen");
            fullScreenButton.OnToggle += this.FullScreenButton_OnToggle;            
            
            var muteButton = new ToggleButton(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH - 300, 15), 250, 55, new GameState[2] { GameState.MainTitleScreen, GameState.PuzzleActive }, "Mute");
            muteButton.OnToggle += this.MuteButton_OnToggle;
            
            var exitButton = new Button(new Vector2(100, 15), 250, 55, new GameState[3] { GameState.MainTitleScreen, GameState.PuzzleSelect, GameState.PuzzleActive }, "Exit");
            exitButton.OnClick += this.ExitButton_OnClick;

            var randomPuzzleButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 + 300, 625), 200, 100, new GameState[1] { GameState.PuzzleSelect }, "Random");
            randomPuzzleButton.OnClick += this.RandomPuzzleButton_OnClick;

            var shuffleButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 + 412 - 200, 525), 400, 70, new GameState[1] { GameState.PuzzleActive }, "Restart / Shuffle");
            shuffleButton.OnClick += this.ShuffleButton_OnClick;

            var puzzleSelectButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 + 412 - 150, 615), 300, 70, new GameState[1] { GameState.PuzzleActive }, "Puzzle Select");
            puzzleSelectButton.OnClick += GoToPuzzleSelect_OnClick;

            var optionsButton = new Button(new Vector2(TileTestGame.WINDOW_STARTING_WIDTH / 2 + 412 - 150, 705), 300, 70, new GameState[1] { GameState.PuzzleActive }, "Options");
            optionsButton.OnClick += this.OptionsButton_OnClick;

            this.m_buttonRoster = new List<Button>()
            {
                titleScreenButton,
                fullScreenButton,
                muteButton,
                exitButton,
                randomPuzzleButton,
                creditsButton,
                instructionsButton,
                backButton,
                shuffleButton,
                optionsButton,
                puzzleSelectButton
            };

            //this.testImageButton = new Button(new Vector2(300, 500), 100, 100, "testimage");
            //this.testImageButton.OnClick += this.TitleScreenButton_OnClick;

        }

        private void DifficultyButton_OnToggle(object sender, EventArgs e)
        {
            ToggleButton senderButton = (ToggleButton)sender;
            int i = this.m_difficultyToggleButtons.IndexOf(senderButton);
            switch (i)
            {
                case 0:
                    this.MainGame.CurrentGridSize = 3;
                    this.MainGame.SetupTileGrid(this.MainGame.ActiveTileManager.m_puzzleImage);
                    break;
                case 1:
                    this.MainGame.CurrentGridSize = 4;
                    this.MainGame.SetupTileGrid(this.MainGame.ActiveTileManager.m_puzzleImage);

                    break;
                case 2:
                    this.MainGame.CurrentGridSize = 5;
                    this.MainGame.SetupTileGrid(this.MainGame.ActiveTileManager.m_puzzleImage);
                    break;     
            }
            for (int b = 0; b < this.m_difficultyToggleButtons.Count; b++)
            {

                this.m_difficultyToggleButtons[b].ToggledState = (b == i);
            }
        }

        private void OptionsButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveGameState = GameState.OptionsScreen;
        }

        private void ShuffleButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveTileManager.JumbleTiles();
        }

        private void ExitButton_OnClick(object sender, EventArgs e)
        {
            if (this.MainGame.ActiveGameState == GameState.MainTitleScreen)
                this.MainGame.Exit();
            else
                this.MainGame.ActiveGameState = GameState.AnimatedTitleScreen;
        }

        private void InstructionsButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveGameState = GameState.Instructions;
        }

        private void CreditsButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveGameState = GameState.Credits;
        }

        private void BackButton_OnClick(object sender, EventArgs e)
        {
            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
                this.MainGame.ActiveGameState = GameState.PuzzleActive;
            else
                this.MainGame.ActiveGameState = GameState.MainTitleScreen;
        }

        private void RandomPuzzleButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.SetupTileGrid(this.MainGame.RandomPuzzleTexture);
            this.MainGame.ActiveGameState = GameState.PuzzleActive; 
        }

        private void ImageButton_OnClick(object sender, EventArgs e)
        {
            int i = this.m_imageSelectButtons.IndexOf((Button)sender);
            this.MainGame.SetupTileGrid(this.MainGame.PuzzleTextures[i]);
            this.MainGame.ActiveGameState = GameState.PuzzleActive;
        }

        private void MuteButton_OnToggle(object sender, EventArgs e)
        {
            ToggleButton senderButton = (ToggleButton)sender;
            if (senderButton.ToggledState)
            {
                senderButton.ButtonText = "Unmute";
                Debug.WriteLine("Toggled on!");
            }
            else
            {
                senderButton.ButtonText = "Mute";
                Debug.WriteLine("Toggled off!");
            }
        }

        private void FullScreenButton_OnToggle (object sender, EventArgs e)
        {
            
            this.MainGame.m_graphics.IsFullScreen = !this.MainGame.m_graphics.IsFullScreen;
            this.MainGame.m_graphics.ApplyChanges();

            ToggleButton senderButton = (ToggleButton)sender;
            if (senderButton.ToggledState)
                senderButton.ButtonText = "Windowed";
            else
                senderButton.ButtonText = "Fullscreen";
        }

        private void GoToPuzzleSelect_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveGameState = GameState.PuzzleSelect;
            
        }

        public void UpdateButtons(GameTime gameTime, MouseState currentMouseState)
        {
            //this.m_buttonRoster[0].IsVisible = false;
            //if (this.MainGame.m_interfaceRenderer.TitleState == TitleScreenState.Main)
            //    this.m_buttonRoster[0].IsVisible = true;

            foreach (Button button in this.m_buttonRoster)
            {
                if (button.IsVisible)
                    button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
            }
            
        
            foreach (Button button in this.m_imageSelectButtons)
            {
                button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
            }
                //this.testImageButton.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
           
            foreach (Button button in this.m_difficultyToggleButtons)
            {
                button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
            }
        }
        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (Button button in this.m_buttonRoster)
            {
                if (button.m_visibleStates.Contains(this.MainGame.ActiveGameState))
                {
                    button.IsVisible = true;
                    button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_bahnschriftFont);
                }
                else
                {
                    button.IsVisible = false;
                }
            }

            if (this.MainGame.ActiveGameState == GameState.PuzzleSelect)
            {
                for (int i = 0; i < this.m_imageSelectButtons.Count(); i++)
                {
                    
                    this.m_imageSelectButtons[i].DrawIt(spriteBatch, this.MainGame.PuzzleTextures[i]);
                }
            }

            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                foreach (ToggleButton button in this.m_difficultyToggleButtons)
                    button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_bahnschriftFont);

            }

            //this.testImageButton.DrawIt(spriteBatch, this.MainGame.PuzzleTextures[0]);
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

            if (this.MainGame.ActiveGameState == GameState.PuzzleSelect)
            {
                foreach (Button button in this.m_imageSelectButtons)
                {
                    button.CheckIfClicked(currentMouseState);
                }
            }

            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                foreach (ToggleButton button in this.m_difficultyToggleButtons)
                    button.CheckIfClicked(currentMouseState);

            }
            //this.testImageButton.CheckIfClicked(currentMouseState);
        }
    }

}
