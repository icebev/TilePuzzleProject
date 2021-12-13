using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TileTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TileTestGame : Game
    {
        #region Member Variables

        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        // Content member variables
        private List<String> m_puzzleFilenames;
        private List<Texture2D> m_puzzleTextures;

        private Texture2D m_backgroundTexture;
        private Texture2D m_squareUIElement;
        private Texture2D m_tileShadowTexture;
        private SoundEffect m_tileSlideSFX;
        private SpriteFont m_bahnschriftFont;

        private TileManager m_tileManager;

        private readonly Random m_random;

        private KeyboardState m_currentKeyboardState;
        private KeyboardState m_previousKeyboardState;

        private MouseState m_currentMouseState;
        private MouseState m_previousMouseState;

        #endregion

        #region Properties

        // Using getters and setters ensures that private member variables aren't changed accidentally and restricts access accordingly
        private SpriteBatch MainSpriteBatch 
        {
            get { return this.m_spriteBatch; }
        }
        public TileManager ActiveTileManager
        {
            get { return this.m_tileManager; }

            private set { this.m_tileManager = value; }
        }

        // Helper property RandomPuzzleTexture selects at random one texture in the puzzleTextures list using the randomizer, for shuffling the displayed puzzle image
        private Texture2D RandomPuzzleTexture
        {
            get 
            {
                int randomInt = this.m_random.Next(0, this.m_puzzleFilenames.Count);
                return this.m_puzzleTextures[randomInt]; 
            }
        }

        #endregion
        public TileTestGame()
        {
            this.m_graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.m_random = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //this.graphics.IsFullScreen = true;
            // Sets up the desired window resolution
            this.m_graphics.PreferredBackBufferWidth = 1600;
            this.m_graphics.PreferredBackBufferHeight = 900;
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

            // TODO: use this.Content to load your game content here

            // Initialize the list of string Filenames - list contains all of the .png image names
            this.m_puzzleFilenames = new List<string> { "accursed", "tinybones", "building", "archfiend", "glorybringer", "kefnet", "mightyleap", "oketra", "sunset" };
            this.m_puzzleTextures = this.LoadTextureList(this.m_puzzleFilenames);

            // Content loading
            this.m_backgroundTexture = this.Content.Load<Texture2D>("textures/background");
            this.m_squareUIElement = this.Content.Load<Texture2D>("textures/UI/squarecontainer");
            this.m_tileShadowTexture = this.Content.Load<Texture2D>("textures/tileshadow");
            this.m_tileSlideSFX = this.Content.Load<SoundEffect>("audio/slide");
            this.m_bahnschriftFont = this.Content.Load<SpriteFont>("fonts/bahnschrift");

            this.SetupTileGrid(3, this.RandomPuzzleTexture);

        }

        // LoadTextureList function takes in a string list and returns a list of loaded Texture2Ds to avoid writing many lines of similar code for loading each texture
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

        // SetupTileGrid function creates a new TileManager object with a specified gridSize and puzzle image texture and will be stored in the ActiveTileManager for reference
        public void SetupTileGrid(int gridSize, Texture2D puzzleImage)
        {
            this.ActiveTileManager = new TileManager(gridSize, puzzleImage, this.m_tileSlideSFX, this.m_bahnschriftFont, this.m_tileShadowTexture);
            this.ActiveTileManager.GenerateTiles();
            this.ActiveTileManager.JumbleTiles();
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || this.m_currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad2) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad2))
            {
                this.SetupTileGrid(2, this.RandomPuzzleTexture);
            }

            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad3) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad3))
            {
                this.SetupTileGrid(3, this.RandomPuzzleTexture);
            }

            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad4) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad4))
            {
                this.SetupTileGrid(4, this.RandomPuzzleTexture);
            }

            this.ActiveTileManager.CheckForTileClick(this.m_currentMouseState, this.m_previousMouseState);
            this.ActiveTileManager.CheckForArrowKey(this.m_currentKeyboardState, this.m_previousKeyboardState);
            this.ActiveTileManager.UpdateTiles(gameTime);
            this.ActiveTileManager.CheckPuzzleCompletion();

            // Store the input states for reference in the next update method, used to make sure that the keys or mouse cannot be held down to break the game
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

            // TODO: Add your drawing code here
            this.MainSpriteBatch.Begin();

            this.MainSpriteBatch.Draw(this.m_backgroundTexture, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            this.MainSpriteBatch.Draw(this.m_tileShadowTexture, new Rectangle(150 - 15, 150 - 15, 450 + 60, 450+ 60), Color.White);
            this.MainSpriteBatch.Draw(this.m_squareUIElement, new Rectangle(150 - 20, 150 - 20, 450 + 60, 450+ 60), Color.White);
            this.ActiveTileManager.DrawScore(this.MainSpriteBatch);
            this.ActiveTileManager.DrawTiles(this.MainSpriteBatch);
            this.ActiveTileManager.DrawReferenceImage(this.MainSpriteBatch);

            this.MainSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
