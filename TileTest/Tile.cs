using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class Tile : IGridMember
    {

        private const int TILE_DIMENSION = 150;
        private const int GRID_START_X = 200;
        private const int GRID_START_Y = 200;
        public const int TILE_PADDING = 5;
        public const int IMAGE_TOLERANCE = 5;
        public const int ANIMATION_TOLERANCE = 2;
        public const float ANIMATION_BASE_SPEED = 65f;
        private const float DELTA_MULTIPLIER = 2.5f;

        private SpriteFont m_hintFont;
        public bool IsCurrentlySwappable { get; set; }

        public bool IsInAnimation { get; set; }
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int GridSize { get; set; }

        private readonly Texture2D m_puzzleImage;
        private readonly Texture2D m_tileShadowTexture;

        private Point m_tileAnimatedDrawPosition;
        public Point TileFinalDrawPosition
        {
            get
            {
                Point drawTarget = new Point(GRID_START_X + (int)(this.CurrentGridPosition.X * (TILE_DIMENSION + TILE_PADDING)),
                    GRID_START_Y + (int)(this.CurrentGridPosition.Y * (TILE_DIMENSION + TILE_PADDING)));

                return drawTarget;
            }
        }
          
        public Rectangle TileBounds => new Rectangle(this.TileFinalDrawPosition, new Point(TILE_DIMENSION, TILE_DIMENSION));
        public int PuzzleImageDimensions
        {
            get 
            {
                if (this.m_puzzleImage.Width <= this.m_puzzleImage.Height - IMAGE_TOLERANCE || this.m_puzzleImage.Width >= this.m_puzzleImage.Height + IMAGE_TOLERANCE)
                    Debug.WriteLine("Source image does not have square dimensions.");

                return this.m_puzzleImage.Width;                    
            }
        }

        public float SourceScaleFactor
        {
            get
            {
                return (float)this.PuzzleImageDimensions / (float)((TILE_DIMENSION + TILE_PADDING) * this.GridSize);
            }
        }

        public Tile(Point correctPosition, Texture2D puzzleImage, int gridSize, SpriteFont hintFont, Texture2D tileshadow)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.m_puzzleImage = puzzleImage;
            this.m_tileShadowTexture = tileshadow;
            this.GridSize = gridSize;
            this.m_hintFont = hintFont;

            this.m_tileAnimatedDrawPosition = this.TileFinalDrawPosition;
        }

        public void DrawIt(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(
                this.m_tileAnimatedDrawPosition.X, 
                this.m_tileAnimatedDrawPosition.Y, 
                TILE_DIMENSION, 
                TILE_DIMENSION);

            Rectangle sourceRectangle = new Rectangle(
                (int)(this.CorrectGridPosition.X * (TILE_DIMENSION * this.SourceScaleFactor + TILE_PADDING)), 
                (int)(this.CorrectGridPosition.Y * (TILE_DIMENSION * this.SourceScaleFactor + TILE_PADDING)), 
                (int)((TILE_DIMENSION) * this.SourceScaleFactor), 
                (int)((TILE_DIMENSION) * this.SourceScaleFactor));

            Rectangle shadowDestinationRectangle = new Rectangle(
                this.m_tileAnimatedDrawPosition.X + 3,
                this.m_tileAnimatedDrawPosition.Y + 3,
                TILE_DIMENSION + 1,
                TILE_DIMENSION + 1);

            spriteBatch.Draw(this.m_tileShadowTexture, shadowDestinationRectangle, null, Color.White);

            spriteBatch.Draw(this.m_puzzleImage, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.DrawString(this.m_hintFont, $"{this.CorrectGridPosition.X}, {this.CorrectGridPosition.Y}", new Vector2(this.m_tileAnimatedDrawPosition.X + 50, this.m_tileAnimatedDrawPosition.Y + 50), Color.White);
        }

        public void UpdateIt(GameTime gameTime)
        {
            int deltaX = this.m_tileAnimatedDrawPosition.X - this.TileFinalDrawPosition.X;
            int deltaY = this.m_tileAnimatedDrawPosition.Y - this.TileFinalDrawPosition.Y;

            // needs refactoring
            if (!(Math.Abs(deltaX) <= ANIMATION_TOLERANCE && Math.Abs(deltaY) <= ANIMATION_TOLERANCE))
            {
                int ANIMATION_SPEED_BOOST_X = (int)(Math.Abs(deltaX) * DELTA_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds);
                int ANIMATION_SPEED_BOOST_Y = (int)(Math.Abs(deltaY) * DELTA_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds);

                if (deltaX < -ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.X += (int)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.X += ANIMATION_SPEED_BOOST_X + 1;
                }
                else if (deltaX > ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.X -= (int)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.X -= ANIMATION_SPEED_BOOST_X + 1;
                }

                if (deltaY < -ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.Y += (int)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.Y += ANIMATION_SPEED_BOOST_Y + 1;
                }
                else if (deltaY > ANIMATION_TOLERANCE)
                {
                    this.m_tileAnimatedDrawPosition.Y -= (int)(ANIMATION_BASE_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
                    this.m_tileAnimatedDrawPosition.Y -= ANIMATION_SPEED_BOOST_Y + 1;
                }

            }
            else
            {
                this.m_tileAnimatedDrawPosition.X = this.TileFinalDrawPosition.X;
                this.m_tileAnimatedDrawPosition.Y = this.TileFinalDrawPosition.Y;
            }
        }
    }
}
