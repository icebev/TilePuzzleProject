using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* TILE CLASS
     * Last modified by Joe Bevis 13/01/2022
     ****************************************/

    /// <summary>
    /// Each visible tile in the tile puzzle grid will be a tile object
    /// Tiles have a correct position stored in addition to current their current position 
    /// so that they can be checked for puzzle completion
    /// </summary>
    public class Tile : IGridMember
    {
        #region Variables

        public const int TILE_CONTAINER_PADDING = 20;
        public const int TILE_PADDING = 5;
        public const int IMAGE_TOLERANCE = 5;
        public const int ANIMATION_TOLERANCE = 2;
        public const float ANIMATION_BASE_SPEED = 50f;
        private const float DELTA_MULTIPLIER = 3.5f;

        private const int SHADOW_OFFSET_X = 3;
        private const int SHADOW_OFFSET_Y = 1;

        private SpriteFont m_hintFont;
        private TileGame m_mainGame;
        private readonly Texture2D m_puzzleImage;
        private readonly Texture2D m_tileShadowTexture;
        
        private int m_windowHeight;

        // Used for the number hints
        public int m_positionValue;

        private Vector2 m_tileAnimatedDrawPosition;

        #endregion

        #region Properties
        public TileGame MainGame { get => this.m_mainGame; }
        public int GridStartX
        {
            get
            {
                int xPaddingPixels = this.m_windowHeight / 8 + TILE_CONTAINER_PADDING;
                return xPaddingPixels;
            }
        }
        public bool IsCurrentlySwappable { get; set; }
        public int TileDimension
        {
            get 
            {
                // The adjusted size of the UI container is calculated in the interface renderer draw 
                int containerSize = this.MainGame.ActiveInterfaceRenderer.PuzzleContainerSize;
                int adjustedContainerSize = containerSize - (this.GridSize * TILE_PADDING) - (2 * TILE_CONTAINER_PADDING);
                int tileDimensionPixels = adjustedContainerSize / this.GridSize;
                return tileDimensionPixels;
            }
        }
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int GridSize { get; set; }

        /* The final draw position is based on the grid location the tile has been set to
         * The player only sees the tile in this position after the animation is completed
         * the lag behind the current draw position and final draw position enables the animation
         ******************************************************************************************/
        public Vector2 TileFinalDrawPosition
        {
            get
            {
                Vector2 drawTarget = new Vector2(this.GridStartX + (this.CurrentGridPosition.X * (this.TileDimension + TILE_PADDING)),
                    this.GridStartX+ (this.CurrentGridPosition.Y * (this.TileDimension + TILE_PADDING)));

                return drawTarget;
            }
        }

        /// <summary>
        /// Each tile has bounds based on the final target position
        /// Used for drawing and detecting tile clicks
        /// </summary>
        public Rectangle TileBounds
        {
            get
            {
                Rectangle boundingRectangle = new Rectangle(new Point((int)this.TileFinalDrawPosition.X, (int)this.TileFinalDrawPosition.Y), 
                    new Point(this.TileDimension, this.TileDimension));

                return boundingRectangle;
            }
        }

        // Getter that checks the source image for a square
        public int PuzzleImageDimensions
        {
            get 
            {
                if (this.m_puzzleImage.Width <= this.m_puzzleImage.Height - IMAGE_TOLERANCE || this.m_puzzleImage.Width >= this.m_puzzleImage.Height + IMAGE_TOLERANCE)
                    Debug.WriteLine("Source image does not have square dimensions.");

                return this.m_puzzleImage.Width;                    
            }
        }

        /// <summary>
        /// The texture dimensions divided by the draw grid dimensions gets a scale factor to be used with
        /// the source rectangles in the draw step to ensure the entire image is drawn
        /// </summary>
        public float SourceScaleFactor
        {
            get
            {
                return (float)this.PuzzleImageDimensions / (float)((this.TileDimension + TILE_PADDING) * this.GridSize);
            }
        }
        #endregion

        #region Constructor
        public Tile(Point correctPosition, int value, Texture2D puzzleImage, int gridSize, SpriteFont hintFont, Texture2D tileshadow, TileGame mainGame)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.m_positionValue = value;
            this.m_puzzleImage = puzzleImage;
            this.m_tileShadowTexture = tileshadow;
            this.GridSize = gridSize;
            this.m_hintFont = hintFont;
            this.m_mainGame = mainGame;
                
            this.m_tileAnimatedDrawPosition = this.TileFinalDrawPosition;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Called during each draw cycle as part of the game loop
        /// A destination and source rectangle is calculated for the tile texture and shadow
        /// </summary>
        public void DrawIt(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(
                (int)this.m_tileAnimatedDrawPosition.X, 
                (int)this.m_tileAnimatedDrawPosition.Y, 
                this.TileDimension, 
                this.TileDimension);

            Rectangle sourceRectangle = new Rectangle(
                (int)(this.CorrectGridPosition.X * (this.TileDimension * this.SourceScaleFactor + TILE_PADDING)), 
                (int)(this.CorrectGridPosition.Y * (this.TileDimension * this.SourceScaleFactor + TILE_PADDING)), 
                (int)((this.TileDimension) * this.SourceScaleFactor), 
                (int)((this.TileDimension) * this.SourceScaleFactor));

            Rectangle shadowDestinationRectangle = new Rectangle(
                (int)this.m_tileAnimatedDrawPosition.X + SHADOW_OFFSET_X,
                (int)this.m_tileAnimatedDrawPosition.Y + SHADOW_OFFSET_X,
                this.TileDimension + SHADOW_OFFSET_Y,
                this.TileDimension + SHADOW_OFFSET_Y);

            spriteBatch.Draw(this.m_tileShadowTexture, shadowDestinationRectangle, null, Color.White);
            spriteBatch.Draw(this.m_puzzleImage, destinationRectangle, sourceRectangle, Color.White);

            // The tile value assigned during tile generation is drawn in the center of the tile
            // It acts as a hint and will be toggled on and off in the options
            if (this.MainGame.ShowTileNumbers)
                spriteBatch.DrawString(this.m_hintFont, $"{this.m_positionValue}", 
                    new Vector2(this.m_tileAnimatedDrawPosition.X + this.TileDimension / 2 - this.m_hintFont.MeasureString($"{this.m_positionValue}").X / 2,
                    this.m_tileAnimatedDrawPosition.Y + this.TileDimension / 2 - this.m_hintFont.MeasureString($"{this.m_positionValue}").Y / 2), 
                    Color.White);
        }

        /// <summary>
        /// The tile is updated as part of the update loop of the game.
        /// The windowHeight is updated in case the tiles need to change size with the window.
        /// The difference between the animated draw position and final draw position decreases:
        /// this gives the tiles a sliding animated effect.
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateIt(GameTime gameTime)
        {
            this.m_windowHeight = this.MainGame.WindowHeight;
            
            float deltaX = this.m_tileAnimatedDrawPosition.X - this.TileFinalDrawPosition.X;

            if (!(Math.Abs(deltaX) <= ANIMATION_TOLERANCE))
            {
                /* The animation speed boost enables the speed of the animated movement
                 * to depend on the distance to the target.
                 * As the tile moves closer to the target position it slows down
                 * because the deltaX decreases.
                 *********************************************************************************/
                
                float ANIMATION_SPEED_BOOST_X = (float)(Math.Abs(deltaX) * DELTA_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds);

                if (deltaX < -ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.X += (float)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.X += ANIMATION_SPEED_BOOST_X;
                }
                else if (deltaX > ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.X -= (float)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.X -= ANIMATION_SPEED_BOOST_X;
                }
            }
            else if (this.m_tileAnimatedDrawPosition.X != this.TileFinalDrawPosition.X)
            {
                this.m_tileAnimatedDrawPosition.X = this.TileFinalDrawPosition.X;
            }
           
            float deltaY = this.m_tileAnimatedDrawPosition.Y - this.TileFinalDrawPosition.Y;

            if (!(Math.Abs(deltaY) <= ANIMATION_TOLERANCE))
            {
                float ANIMATION_SPEED_BOOST_Y = (float)(Math.Abs(deltaY) * DELTA_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds);

                if (deltaY < -ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.Y += (float)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.Y += ANIMATION_SPEED_BOOST_Y;
                }
                else if (deltaY > ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.Y -= (float)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.Y -= ANIMATION_SPEED_BOOST_Y;
                }
            }
            else if (this.m_tileAnimatedDrawPosition.Y != this.TileFinalDrawPosition.Y)
            {
                this.m_tileAnimatedDrawPosition.Y = this.TileFinalDrawPosition.Y;
            }
        }

        #endregion
    }
}
