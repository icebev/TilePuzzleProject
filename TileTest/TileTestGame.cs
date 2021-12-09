using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TileTestGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D m_testSprite; 
        Texture2D m_backgroundTexture;
        private Texture2D m_tileShadowTexture;
        SoundEffect m_tileSlideSfx;
        public SpriteFont m_bahnschriftFont;

        TileManager m_tileManager;

        
        private KeyboardState m_currentKeyboardState;
        private KeyboardState m_previousKeyboardState;

        private MouseState m_currentMouseState;
        private MouseState m_previousMouseState;
        


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
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.m_testSprite = this.Content.Load<Texture2D>("textures/puzzles/tinybones");
            this.m_backgroundTexture = this.Content.Load<Texture2D>("textures/ancientrock");
            this.m_tileShadowTexture = this.Content.Load<Texture2D>("textures/tileshadow");
            this.m_tileSlideSfx = this.Content.Load<SoundEffect>("audio/slide");
            this.m_bahnschriftFont = this.Content.Load<SpriteFont>("fonts/bahnschrift");

            this.m_tileManager = new TileManager(3, this.m_testSprite, this.m_tileSlideSfx, this.m_bahnschriftFont, this.m_tileShadowTexture);
            this.m_tileManager.GenerateTiles();
            Point emptyTilePosition = this.m_tileManager.FindBlankTile();
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
            if (this.m_currentKeyboardState.IsKeyDown(Keys.Up) && !this.m_previousKeyboardState.IsKeyDown(Keys.Up))
                this.m_tileManager.SwapTile(this.m_tileManager.m_tilesArray[2, 1]);

            this.m_tileManager.CheckForTileClick(this.m_currentMouseState, this.m_previousMouseState);

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
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(this.m_backgroundTexture, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            //this.spriteBatch.Draw(this.m_testSprite, new Rectangle(0, 0, 500, 500), Color.White);
            this.m_tileManager.DrawTiles(this.spriteBatch);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
