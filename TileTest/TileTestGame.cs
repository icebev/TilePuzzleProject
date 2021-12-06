using Microsoft.Xna.Framework;
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

        TileManager m_tileManager;

        private KeyboardState m_previousKeyboardState;

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
            this.m_testSprite = this.Content.Load<Texture2D>("tinybones");

            this.m_tileManager = new TileManager(3, this.m_testSprite);
            this.m_tileManager.GenerateTiles();
            Point emptyTilePosition = this.m_tileManager.FindBlankTile();
            this.m_tileManager.DetermineSwappableTiles();


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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !this.m_previousKeyboardState.IsKeyDown(Keys.Up))
                this.m_tileManager.SwapTile(this.m_tileManager.m_tilesArray[2, 1]);

            this.m_previousKeyboardState = Keyboard.GetState();
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
            //this.spriteBatch.Draw(this.m_testSprite, new Rectangle(0, 0, 500, 500), Color.White);
            this.m_tileManager.DrawTiles(this.spriteBatch);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
