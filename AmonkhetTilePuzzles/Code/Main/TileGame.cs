using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AmonkhetTilePuzzles
{
    /* TILE GAME CLASS
     * Last modified by Joe Bevis 18/01/2022
     ****************************************/

    /// <summary>
    /// The main TileGame class - one instance of this will be created when the program begins.
    /// </summary>
    public class TileGame : Game
    {
        #region Variables
        public const int WINDOW_STARTING_WIDTH = 1600;
        public const int WINDOW_STARTING_HEIGHT = 900;

        public GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        // Content member variables
        private List<String> m_puzzleFilenames;
        private List<Texture2D> m_puzzleTextures;
        private Texture2D m_titleBackgroundTexture;
        private Texture2D m_puzzleBackgroundTexture;
        private Texture2D m_tileShadowTexture;
        public SpriteFont m_calligraphicFont;

        private GameState m_gameState;
        private TileManager m_tileManager;
        private InputManager m_inputManager;
        private InterfaceRenderer m_interfaceRenderer;
        private readonly Random m_random;

        // Input device states
        private KeyboardState m_currentKeyboardState;
        private KeyboardState m_previousKeyboardState;
        private MouseState m_currentMouseState;
        private MouseState m_previousMouseState;

        private int m_tileGridSize = 3;
        private bool m_showNumbers;
        private bool m_showTimer;
        private HighscoreTracker m_highscoreTracker;

        #endregion

        #region Properties

        // Using getters and setters ensures that private member variables aren't changed accidentally and restricts access accordingly
        private SpriteBatch MainSpriteBatch 
        {
            get { return this.m_spriteBatch; }
        }
        public int CurrentGridSize
        {
            get { return this.m_tileGridSize; }
            set { this.m_tileGridSize = value; }
        }
        public bool ShowTileNumbers
        {
            get { return this.m_showNumbers; }
            set { this.m_showNumbers = value; }
        }
        public bool ShowTimer
        {
            get { return this.m_showTimer; }
            set { this.m_showTimer = value; }
        }
        public TileManager ActiveTileManager
        {
            get { return this.m_tileManager; }

            private set { this.m_tileManager = value; }
        }
        public InputManager ActiveInputManager
        {
            get { return this.m_inputManager; }
        } 
        public InterfaceRenderer ActiveInterfaceRenderer
        {
            get { return this.m_interfaceRenderer; }
        }
        public GameState ActiveGameState
        {
            get { return this.m_gameState; }
            set { this.m_gameState = value; }
        }
        /// <summary>
        /// Helper property RandomPuzzleTexture selects at random one texture in the puzzleTextures list using the randomizer
        /// Used by the random puzzle select button
        /// </summary>
        public Texture2D RandomPuzzleTexture
        {
            get 
            {
                int randomInt = this.m_random.Next(0, this.m_puzzleFilenames.Count);
                return this.m_puzzleTextures[randomInt]; 
            }
        }
        public List<Texture2D> PuzzleTextures { get => this.m_puzzleTextures; }
        public int WindowWidth
        {
            get 
            {
                int pixelWidth = this.Window.ClientBounds.Width;
                return pixelWidth; 
            }
        }
        public int WindowHeight
        {
            get
            {
                int pixelHeight = this.Window.ClientBounds.Height;
                return pixelHeight;
            }
        }
        public Point WindowCenter
        {
            get
            {
                Point centerPoint = new Point((int)(this.WindowWidth * 0.5), (int)(this.WindowHeight * 0.5));
                return centerPoint;
            }
        }
        public HighscoreTracker ActiveHighscoreTracker { get => this.m_highscoreTracker; }

        #endregion

        #region Constructor
        // Constructor for the TileGame class
        public TileGame()
        {
            this.m_graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.m_random = new Random();

            // Begins the game with the welcome animation
            this.ActiveGameState = GameState.AnimatedTitleScreen;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Sets up the desired window resolution - fullscreen upon game start
            this.m_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.m_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; 
            // Allow user to resize the window
            this.Window.AllowUserResizing = true;
            this.m_graphics.ApplyChanges();

            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.m_spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // CONTENT LOADING

            // Initialize the list of string Filenames - list contains all of the .png image names
            // These can then be loaded without many lines of repeated code by iterating through the list
            this.m_puzzleFilenames = new List<string> { "accursed", "tinybones", "building", "archfiend", "glorybringer", "kefnet", "mightyleap", "oketra", "sunset" };
            this.m_puzzleTextures = this.LoadTextureList(this.m_puzzleFilenames);

            this.m_titleBackgroundTexture = this.Content.Load<Texture2D>("textures/backgrounds/titleBackground");
            this.m_puzzleBackgroundTexture = this.Content.Load<Texture2D>("textures/backgrounds/puzzleBackground");
            this.m_tileShadowTexture = this.Content.Load<Texture2D>("textures/shadows/tileShadow");
            
            AudioStore.LoadAudio(this);
            MediaPlayer.Volume = 0;

            this.m_calligraphicFont = this.Content.Load<SpriteFont>("fonts/calligraphic");
            this.m_inputManager = new InputManager(this);
            this.m_interfaceRenderer = new InterfaceRenderer(this, this.m_calligraphicFont);
            this.m_interfaceRenderer.LoadTextures();

            this.m_highscoreTracker = HighscoreTracker.Load();
        }

        /// <summary>
        /// LoadTextureList function takes in a string list and returns a list of loaded Texture2Ds 
        /// to avoid writing many lines of similar code for loading each texture
        /// </summary>
        public List<Texture2D> LoadTextureList(List<string> filenames)
        {
            List<Texture2D> textureList = new List<Texture2D>();

            foreach (String filename in filenames)
            {
                Texture2D newTexture = this.Content.Load<Texture2D>("textures/puzzles/" + filename);
                textureList.Add(newTexture);
            }
            return textureList;
        }

        /// <summary>
        /// SetupTileGrid function creates a new TileManager object with a specified gridSize and puzzle image texture
        /// will be stored in the ActiveTileManager for reference
        /// </summary>
        public void SetupTileGrid(Texture2D puzzleImage, int gridSizeOverride = 0)
        {
            if (gridSizeOverride > 1)
            {
                this.CurrentGridSize = gridSizeOverride;
            }
            this.ActiveTileManager = new TileManager(this, this.CurrentGridSize, puzzleImage, this.m_calligraphicFont, this.m_tileShadowTexture);
            this.ActiveTileManager.GenerateTiles();
            this.ActiveTileManager.JumbleTiles();
        }

        /// <summary>
        /// Get function for the window scale used for scaling buttons as the window changes size from the original 
        /// </summary>
        /// <returns> Outputs a vector with window scale in X and Y </returns>
        public Vector2 GetWindowScaleFactor()
        {
            var scaleFactorX = (float)this.WindowWidth / (float)WINDOW_STARTING_WIDTH;
            var scaleFactorY = (float)this.WindowHeight / (float)WINDOW_STARTING_HEIGHT;
            return new Vector2(scaleFactorX, scaleFactorY);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update member variables with the current input states of the keyboard and mouse
            this.m_currentKeyboardState = Keyboard.GetState();
            this.m_currentMouseState = Mouse.GetState();
            if (this.ActiveGameState != GameState.AnimatedTitleScreen)
            {
                if (AudioStore.IsMuted)
                {
                    MediaPlayer.Volume = 0;
                }
                else
                {
                    // Will loop the music if it stops playing
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(AudioStore.m_nileJourneyMusic);
                    }

                    MediaPlayer.Volume = 0.2f;
                }
            }

            // Only attempt to update the TileManager if it exists
            if (this.ActiveTileManager != null)
            {
                this.ActiveTileManager.UpdateTiles(gameTime);
            }

            this.ActiveInputManager.ProcessControls(this.m_previousMouseState, this.m_currentMouseState, 
                this.m_previousKeyboardState, this.m_currentKeyboardState, gameTime);

            this.ActiveInterfaceRenderer.UpdateIt(gameTime);

            // Store the input states for reference in the next update method,
            // used to make sure that the keys or mouse cannot be held down to break the game
            this.m_previousKeyboardState = Keyboard.GetState();
            this.m_previousMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.SandyBrown);
            this.MainSpriteBatch.Begin();

            // Draw the correct background based off the state
            if ((int)this.ActiveGameState <= 4)
                this.MainSpriteBatch.Draw(this.m_titleBackgroundTexture, new Rectangle(0, 0, this.WindowWidth, this.WindowHeight), Color.White);
            else
                this.MainSpriteBatch.Draw(this.m_puzzleBackgroundTexture, new Rectangle(0, 0, this.WindowWidth, this.WindowHeight), Color.White);

            this.ActiveInterfaceRenderer.DrawInterface(this.MainSpriteBatch);

            if (this.ActiveGameState == GameState.PuzzleActive)
            { 
                this.ActiveTileManager.DrawTiles(this.MainSpriteBatch);
                this.ActiveTileManager.DrawReferenceImage(this.MainSpriteBatch);
            }

            // Draws buttons on top of most elements
            this.ActiveInputManager.DrawIt(this.MainSpriteBatch);

            if (this.ActiveGameState == GameState.PuzzleSelect)
                this.ActiveInterfaceRenderer.DrawCompletedTrophies(this.MainSpriteBatch);

            this.MainSpriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
