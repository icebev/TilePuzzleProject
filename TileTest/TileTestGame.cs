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

        private GraphicsDeviceManager graphics;
        private SpriteBatch m_spriteBatch;

        // Content member variables
        private List<String> m_fileNames;
        private List<Texture2D> m_puzzleTextures;

        private Texture2D m_testSprite; 
        private Texture2D m_backgroundTexture;
        private Texture2D m_tileShadowTexture;
        private SoundEffect m_tileSlideSFX;
        private SpriteFont m_bahnschriftFont;

        private TileManager m_tileManager;

        private Random m_random;

        private KeyboardState m_currentKeyboardState;
        private KeyboardState m_previousKeyboardState;

        private MouseState m_currentMouseState;
        private MouseState m_previousMouseState;

        #endregion

        private int RandomTextureNumber
        {
            get { return this.m_random.Next(0, this.m_fileNames.Count); }
        }
        public TileTestGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
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
            this.graphics.PreferredBackBufferWidth = 1600;
            this.graphics.PreferredBackBufferHeight = 900;
            this.graphics.ApplyChanges();

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
            // File names string list contains all of the .png image names
            this.m_fileNames = new List<string> { "accursed", "tinybones", "building", "archfiend", "glorybringer", "kefnet", "mightyleap", "oketra", "sunset" };
            this.m_puzzleTextures = LoadPuzzleImages();
            this.m_random = new Random();

            // Content loading
            this.m_backgroundTexture = this.Content.Load<Texture2D>("textures/ancientrock");
            this.m_tileShadowTexture = this.Content.Load<Texture2D>("textures/tileshadow");
            this.m_tileSlideSFX = this.Content.Load<SoundEffect>("audio/slide");
            this.m_bahnschriftFont = this.Content.Load<SpriteFont>("fonts/bahnschrift");

            this.SetupTileGrid(3, this.m_puzzleTextures[this.RandomTextureNumber]);

        }
        public List<Texture2D> LoadPuzzleImages()
        {
            List<Texture2D> PuzzleImageList = new List<Texture2D>();

            foreach (String filename in this.m_fileNames)
            {
                Texture2D newTexture = this.Content.Load<Texture2D>("textures/puzzles/" + filename);
                PuzzleImageList.Add(newTexture);
            }
            return PuzzleImageList;
        }
        public void SetupTileGrid(int gridSize, Texture2D puzzleImage)
        {
            this.m_tileManager = new TileManager(gridSize, puzzleImage, this.m_tileSlideSFX, this.m_bahnschriftFont, this.m_tileShadowTexture);
            this.m_tileManager.GenerateTiles();
            this.m_tileManager.DetermineSwappableTiles();
            this.m_tileManager.JumbleTiles();
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
            this.m_currentKeyboardState = Keyboard.GetState();
            this.m_currentMouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || this.m_currentKeyboardState.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad2) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad2))
            {
                this.SetupTileGrid(2, this.m_puzzleTextures[this.RandomTextureNumber]);
            }

            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad3) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad3))
            {
                this.SetupTileGrid(3, this.m_puzzleTextures[this.RandomTextureNumber]);
            }

            if (this.m_currentKeyboardState.IsKeyDown(Keys.NumPad4) && !this.m_previousKeyboardState.IsKeyDown(Keys.NumPad4))
            {
                this.SetupTileGrid(4, this.m_puzzleTextures[this.RandomTextureNumber]);
            }

            this.m_tileManager.CheckForTileClick(this.m_currentMouseState, this.m_previousMouseState);
            this.m_tileManager.CheckForArrowKey(this.m_currentKeyboardState, this.m_previousKeyboardState);

            this.m_previousKeyboardState = Keyboard.GetState();
            this.m_previousMouseState = Mouse.GetState();

            this.m_tileManager.UpdateTiles(gameTime);
            this.m_tileManager.CheckPuzzleCompletion();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Purple);

            // TODO: Add your drawing code here
            this.m_spriteBatch.Begin();
            this.m_spriteBatch.Draw(this.m_backgroundTexture, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            this.m_tileManager.DrawScore(this.m_spriteBatch);
            this.m_tileManager.DrawTiles(this.m_spriteBatch);
            this.m_tileManager.DrawReferenceImage(this.m_spriteBatch);
            this.m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
