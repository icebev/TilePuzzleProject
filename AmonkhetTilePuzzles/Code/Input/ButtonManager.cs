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
    /* BUTTON MANAGER CLASS
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/
    /// <summary>
    /// Holds the button and togglebutton instances in a manageable way
    /// Defines the functionality of each button through event subscriptions
    /// An instance of the button manager is created within the input manager
    /// </summary>
    public class ButtonManager
    {
        #region Variables
        // Keeping these public allows the label text to be aligned correctly in the interface renderer
        public const int OPTION_BUTTON_START_Y = 200;
        public const int OPTION_BUTTON_SEPARATION_Y = 150;

        private const int IMAGE_GRID_COLUMNS = 3;
        private const int PUZZLE_SELECT_BUTTONS_START_X = 400;
        private const int PUZZLE_SELECT_BUTTONS_SEPARATION_X = 250;
        private const int PUZZLE_SELECT_BUTTONS_START_Y = 250;
        private const int PUZZLE_SELECT_BUTTONS_SEPARATION_Y = 175;
        private const int PUZZLE_SELECT_BUTTONS_SIZE = 150;
        private const int STANDARD_BUTTON_HEIGHT = 100;
        private const int SMALLER_BUTTON_HEIGHT = 70;
        private const int BORDER_BUTTON_HEIGHT = 55;
        private const int BORDER_BUTTON_WIDTH = 250;

        private TileGame m_mainGame;
        
        private List<Button> m_buttonRoster;
        private List<Button> m_imageSelectButtons;
        private List<ToggleButton> m_difficultyToggleButtons;
        
        private Texture2D m_normalTexture;
        private Texture2D m_hoverTexture;
        private Texture2D m_clickTexture;
        private SpriteFont m_calligraphicFont;
        private Button m_gridSizeButton;

        #endregion

        #region Properties
        private TileGame MainGame
        {
            get { return this.m_mainGame; }
        }
        public List<Button> ButtonRoster { get => this.m_buttonRoster; }
        public List<Button> ImageSelectButtons { get => this.m_imageSelectButtons;}
        #endregion

        #region Constructor
        public ButtonManager(TileGame mainGame)
        {
            this.m_mainGame = mainGame;
            this.m_calligraphicFont = this.MainGame.m_calligraphicFont;
            this.m_normalTexture = this.MainGame.Content.Load<Texture2D>("textures/button/normal");
            this.m_hoverTexture = this.MainGame.Content.Load<Texture2D>("textures/button/hover");
            this.m_clickTexture = this.MainGame.Content.Load<Texture2D>("textures/button/click");
            
            this.m_imageSelectButtons = new List<Button>();
            this.m_difficultyToggleButtons = new List<ToggleButton>();

            #region Button Instantiation

            // For loop used to create a grid of puzzle image buttons for the puzzle select screen.
            // It can work with any number of puzzles in the PuzzleTextures list.
            for (int i = 0; i < this.MainGame.PuzzleTextures.Count(); i++)
            {
                int gridX = i % IMAGE_GRID_COLUMNS;
                int gridY = (int)Math.Floor((float)(i / IMAGE_GRID_COLUMNS));
                var newImageButton = new Button(new Vector2(PUZZLE_SELECT_BUTTONS_START_X + PUZZLE_SELECT_BUTTONS_SEPARATION_X * gridX, PUZZLE_SELECT_BUTTONS_START_Y + PUZZLE_SELECT_BUTTONS_SEPARATION_Y * gridY), 
                    PUZZLE_SELECT_BUTTONS_SIZE, PUZZLE_SELECT_BUTTONS_SIZE, new GameState[] { GameState.PuzzleSelect });
                newImageButton.OnClick += this.ImageButton_OnClick;
                this.m_imageSelectButtons.Add(newImageButton);
            }

            // For loop used to create the grid size toggle buttons to avoid repeated code
            for (int i = 3; i < 6; i++)
            {
                string text = $"{i} x {i}";
                var newDifficiltyButton = new ToggleButton(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + (175 * (i - 3)), OPTION_BUTTON_START_Y), 175, STANDARD_BUTTON_HEIGHT, new GameState[] { GameState.OptionsScreen }, text);
                newDifficiltyButton.OnToggle += this.DifficultyButton_OnToggle;
                if ((i) == this.MainGame.CurrentGridSize)
                    newDifficiltyButton.ToggledState = true;
                this.m_difficultyToggleButtons.Add(newDifficiltyButton);
                
            }

            /* All other buttons will be placed in a button roster list once instantiated
             * this enables easy management (updating and drawing) as they are similar is many ways due to the class design
             * HACK Hard coded integer values without a variable assigned represent a pixel dimension or location 
             * each pixel value will be scaled from the orginal frame of reference
             * 1600 x 900 are the original window dimensions:
             * the integer values have been calculated with this in mind 
             * Once each new button instance has been created, the corresponding function is added to the event
             ****************************************************************************************************************/

            var toggleHintsButton = new ToggleButton(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 163, OPTION_BUTTON_START_Y + OPTION_BUTTON_SEPARATION_Y), 200, STANDARD_BUTTON_HEIGHT, new GameState[] { GameState.OptionsScreen }, "Off");
            toggleHintsButton.OnToggle += this.ToggleHintsButton_OnToggle;
            
            var toggleTimerButton = new ToggleButton(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 163, OPTION_BUTTON_START_Y + OPTION_BUTTON_SEPARATION_Y * 2), 200, STANDARD_BUTTON_HEIGHT, new GameState[] { GameState.OptionsScreen }, "Off");
            toggleTimerButton.OnToggle += this.ToggleTimerButton_OnToggle;

            var titleScreenButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 - 200, 400), 400, STANDARD_BUTTON_HEIGHT, new GameState[1] { GameState.MainTitleScreen }, "Start Game");
            titleScreenButton.OnClick += this.GoToPuzzleSelect_OnClick;
            
            var creditsButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 3 - 75, 650), 300, SMALLER_BUTTON_HEIGHT, new GameState[1] { GameState.MainTitleScreen }, "Credits");
            creditsButton.OnClick += this.CreditsButton_OnClick;

            var instructionsButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH * 2 / 3 - 225, 650), 300, SMALLER_BUTTON_HEIGHT, new GameState[1] { GameState.MainTitleScreen }, "Instructions");
            instructionsButton.OnClick += this.InstructionsButton_OnClick; ;

            var backButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 - 150, 650), 300, SMALLER_BUTTON_HEIGHT, new GameState[4] { GameState.Credits, GameState.Instructions, GameState.OptionsScreen, GameState.PuzzleComplete }, "Back");
            backButton.OnClick += this.BackButton_OnClick;

            var fullScreenButton = new ToggleButton(new Vector2(TileGame.WINDOW_STARTING_WIDTH - 600, 15), BORDER_BUTTON_WIDTH, BORDER_BUTTON_HEIGHT, new GameState[2] { GameState.MainTitleScreen, GameState.PuzzleActive }, "Fullscreen");
            fullScreenButton.ToggledState = true;
            fullScreenButton.OnToggle += this.FullScreenButton_OnToggle;            
            
            var muteButton = new ToggleButton(new Vector2(TileGame.WINDOW_STARTING_WIDTH - 300, 15), BORDER_BUTTON_WIDTH, BORDER_BUTTON_HEIGHT, new GameState[2] { GameState.MainTitleScreen, GameState.PuzzleActive }, "Mute");
            muteButton.OnToggle += this.MuteButton_OnToggle;
            
            var resetScoresButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH - 300, TileGame.WINDOW_STARTING_HEIGHT - 70), BORDER_BUTTON_WIDTH, BORDER_BUTTON_HEIGHT, new GameState[1] { GameState.OptionsScreen }, "Reset Scores");
            resetScoresButton.OnClick += this.ResetScoresButton_OnClick;
            
            var exitButton = new Button(new Vector2(100, 15), BORDER_BUTTON_WIDTH, BORDER_BUTTON_HEIGHT, new GameState[3] { GameState.MainTitleScreen, GameState.PuzzleSelect, GameState.PuzzleActive }, "Exit");
            exitButton.OnClick += this.ExitButton_OnClick;

            var randomPuzzleButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 350, 625), 200, STANDARD_BUTTON_HEIGHT, new GameState[1] { GameState.PuzzleSelect }, "Random");
            randomPuzzleButton.OnClick += this.RandomPuzzleButton_OnClick;

            var gridSizeButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 350, 200), 200, STANDARD_BUTTON_HEIGHT, new GameState[1] { GameState.PuzzleSelect }, "Grid: 3 x 3");
            gridSizeButton.OnClick += this.GridSizeButton_OnClick;
            this.m_gridSizeButton = gridSizeButton;

            var shuffleButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 412 - 200, 525), 400, SMALLER_BUTTON_HEIGHT, new GameState[1] { GameState.PuzzleActive }, "Restart / Shuffle");
            shuffleButton.OnClick += this.ShuffleButton_OnClick;

            var puzzleSelectButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 412 - 150, 615), 300, SMALLER_BUTTON_HEIGHT, new GameState[1] { GameState.PuzzleActive }, "Puzzle Select");
            puzzleSelectButton.OnClick += this.GoToPuzzleSelect_OnClick;

            var optionsButton = new Button(new Vector2(TileGame.WINDOW_STARTING_WIDTH / 2 + 412 - 150, 705), 300, SMALLER_BUTTON_HEIGHT, new GameState[1] { GameState.PuzzleActive }, "Options");
            optionsButton.OnClick += this.OptionsButton_OnClick;

            this.m_buttonRoster = new List<Button>()
            {
                titleScreenButton,
                fullScreenButton,
                muteButton,
                exitButton,
                resetScoresButton,
                randomPuzzleButton,
                gridSizeButton,
                creditsButton,
                instructionsButton,
                backButton,
                shuffleButton,
                optionsButton,
                puzzleSelectButton,
                toggleHintsButton,
                toggleTimerButton
            };
            #endregion
        }

        #endregion

        #region Button Funtions
        private void ToggleTimerButton_OnToggle(object sender, EventArgs e)
        {
            ToggleButton senderButton = (ToggleButton)sender;
            this.MainGame.ShowTimer = senderButton.ToggledState;
            senderButton.ButtonText = senderButton.ToggledState ? "On" : "Off";
        }
        private void ToggleHintsButton_OnToggle(object sender, EventArgs e)
        {
            ToggleButton senderButton = (ToggleButton)sender;
            this.MainGame.ShowTileNumbers = senderButton.ToggledState;
            senderButton.ButtonText = senderButton.ToggledState ? "On" : "Off";
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
        }
        private void ResetScoresButton_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveHighscoreTracker.ResetScores();
        }
        private void GridSizeButton_OnClick(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            switch (this.MainGame.CurrentGridSize)
            {
                case 5:
                    this.MainGame.CurrentGridSize = 3;
                    break;
                default:
                    this.MainGame.CurrentGridSize++;
                    break;
            };
            senderButton.ButtonText = $"Grid: {this.MainGame.CurrentGridSize} x {this.MainGame.CurrentGridSize}";
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
            else if (this.MainGame.ActiveGameState == GameState.PuzzleComplete)
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
            AudioStore.IsMuted = senderButton.ToggledState;

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
            ToggleButton senderButton = (ToggleButton)sender;
            if (senderButton.ToggledState)
            {
                this.MainGame.m_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                this.MainGame.m_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                this.MainGame.m_graphics.PreferredBackBufferWidth = 1600;
                this.MainGame.m_graphics.PreferredBackBufferHeight = 900;
            }
            this.MainGame.m_graphics.IsFullScreen = senderButton.ToggledState;
            this.MainGame.m_graphics.ApplyChanges();
        }
        private void GoToPuzzleSelect_OnClick(object sender, EventArgs e)
        {
            this.MainGame.ActiveGameState = GameState.PuzzleSelect;
            
        }
        #endregion

        #region Class Methods

        /// <summary>
        /// UpdateButtons method is called as part of the game update loop by the input manager
        /// Lists of buttons including the main roster are iterated through for code efficiency
        /// </summary>
        public void UpdateButtons(GameTime gameTime, MouseState currentMouseState)
        {
            foreach (Button button in this.ButtonRoster)
            {
                if (button.m_visibleStates.Contains(this.MainGame.ActiveGameState))
                {
                    button.IsVisible = true;
                    button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
                }
                else
                {
                    button.IsVisible = false;
                }
            }
                    
            foreach (Button button in this.ImageSelectButtons)
            {
                button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
                if (this.MainGame.ActiveGameState == GameState.PuzzleSelect)
                    button.IsVisible = true;
            }
            
            foreach (Button button in this.m_difficultyToggleButtons)
            {
                button.UpdateIt(gameTime, currentMouseState, this.MainGame.GetWindowScaleFactor());
                if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
                    button.IsVisible = true;
            }
            // The grid button visible on the puzzle select screen needs to be updated separately
            // as the gridsize can also be changed from the options menu
            this.m_gridSizeButton.ButtonText = $"Grid: {this.MainGame.CurrentGridSize} x {this.MainGame.CurrentGridSize}";

        }
        /// <summary>
        /// Similar to UpdateButtons, called as part of the draw cycle by the input manager
        /// Main roster buttons are only drawn if visible as expected
        /// The other buttons int the other lists are dependant on a single game state directly 
        /// so IsVisible doesn't need to be checked
        /// </summary>
        public void DrawButtons(SpriteBatch spriteBatch)
        {
            foreach (Button button in this.ButtonRoster)
            {
                if (button.IsVisible)
                {
                    button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_calligraphicFont);
                }
            }
            // TODO Is it safe to use IsVisible instead of checking the game state here? Check if you have time
            if (this.MainGame.ActiveGameState == GameState.PuzzleSelect)
            {
                for (int i = 0; i < this.ImageSelectButtons.Count(); i++)
                {
                    this.ImageSelectButtons[i].DrawIt(spriteBatch, this.MainGame.PuzzleTextures[i]);
                }
            }

            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                for (int b = 0; b < this.m_difficultyToggleButtons.Count; b++)
                {
                    this.m_difficultyToggleButtons[b].ToggledState = (b == this.MainGame.CurrentGridSize - 3);
                }
                foreach (ToggleButton button in this.m_difficultyToggleButtons)
                {
                    button.DrawIt(spriteBatch, this.m_normalTexture, this.m_hoverTexture, this.m_clickTexture, this.m_calligraphicFont);
                }
            }
        }
        /// <summary>
        /// Called upon successful mouse down within the input manager
        /// Iterates through all visible buttons to check if they have been clicked
        /// If the button is not visible then this ensures the event will not be invoked accidentally
        /// </summary>
        /// <param name="currentMouseState">The current state of the mouse within the game window</param>
        public void CheckIfButtonsClicked(MouseState currentMouseState)
        {
            foreach (Button button in this.ButtonRoster)
            {
                if (button.IsVisible)
                {
                    button.CheckIfClicked(currentMouseState);
                }
            }

            if (this.MainGame.ActiveGameState == GameState.PuzzleSelect)
            {
                foreach (Button button in this.ImageSelectButtons)
                {
                    button.CheckIfClicked(currentMouseState);
                }
            }

            if (this.MainGame.ActiveGameState == GameState.OptionsScreen)
            {
                foreach (ToggleButton button in this.m_difficultyToggleButtons)
                {
                    button.CheckIfClicked(currentMouseState);
                }
            }
        }
        #endregion
    }

}
