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

        public const int TILE_CONTAINER_PADDING = 20;
        public const int TILE_PADDING = 5;
        public const int IMAGE_TOLERANCE = 5;
        public const int ANIMATION_TOLERANCE = 2;
        public const float ANIMATION_BASE_SPEED = 50f;
        private const float DELTA_MULTIPLIER = 3.5f;

        private SpriteFont m_hintFont;
        private TileTestGame m_maingame;
        private int m_windowHeight;

        private int GridStartX
        {
            get
            {
                int xPaddingPixels = this.m_windowHeight / 8 + TILE_CONTAINER_PADDING;
                return xPaddingPixels;

            }
        }

        private int GridStartY
        {   get
            {
                int yPaddingPixels =  this.m_windowHeight / 8 + TILE_CONTAINER_PADDING;
                return yPaddingPixels;
            }
        }
        public bool IsCurrentlySwappable { get; set; }

        public int TileDimension
        {
            get 
            {
                int containerSize = 3 * this.m_windowHeight / 4;
                int adjustedContainerSize = containerSize - (this.GridSize * TILE_PADDING) - (2 * TILE_CONTAINER_PADDING);
                int tileDimensionPixels = adjustedContainerSize / this.GridSize;
                return tileDimensionPixels;
            }
        }
        public bool IsInAnimation { get; set; }
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int m_positionValue;
        public int GridSize { get; set; }

        private readonly Texture2D m_puzzleImage;
        private readonly Texture2D m_tileShadowTexture;

        private Vector2 m_tileAnimatedDrawPosition;
        public Vector2 TileFinalDrawPosition
        {
            get
            {
                Vector2 drawTarget = new Vector2(this.GridStartX + (this.CurrentGridPosition.X * (this.TileDimension + TILE_PADDING)),
                    this.GridStartY + (this.CurrentGridPosition.Y * (this.TileDimension + TILE_PADDING)));

                return drawTarget;
            }
        }

        public Rectangle TileBounds
        {
            get
            {
                Rectangle boundingRectangle = new Rectangle(new Point((int)this.TileFinalDrawPosition.X, (int)this.TileFinalDrawPosition.Y), new Point(this.TileDimension, this.TileDimension));

                return boundingRectangle;
            }
        }
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
                return (float)this.PuzzleImageDimensions / (float)((this.TileDimension + TILE_PADDING) * this.GridSize);
            }
        }

        public Tile(Point correctPosition, int value, Texture2D puzzleImage, int gridSize, SpriteFont hintFont, Texture2D tileshadow, TileTestGame mainGame)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.m_positionValue = value;
            this.m_puzzleImage = puzzleImage;
            this.m_tileShadowTexture = tileshadow;
            this.GridSize = gridSize;
            this.m_hintFont = hintFont;
            this.m_maingame = mainGame;
                
            this.m_tileAnimatedDrawPosition = this.TileFinalDrawPosition;
        }

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
                (int)this.m_tileAnimatedDrawPosition.X + 3,
                (int)this.m_tileAnimatedDrawPosition.Y + 3,
                this.TileDimension + 1,
                this.TileDimension + 1);

            spriteBatch.Draw(this.m_tileShadowTexture, shadowDestinationRectangle, null, Color.White);
            spriteBatch.Draw(this.m_puzzleImage, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.DrawString(this.m_hintFont, $"{this.m_positionValue}", new Vector2(this.m_tileAnimatedDrawPosition.X + 50, this.m_tileAnimatedDrawPosition.Y + 50), Color.White);
        }

        public void UpdateIt(GameTime gameTime)
        {
            this.m_windowHeight = this.m_maingame.WindowHeight;
            float deltaX = this.m_tileAnimatedDrawPosition.X - this.TileFinalDrawPosition.X;

            if (!(Math.Abs(deltaX) <= ANIMATION_TOLERANCE))
            {
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
    }
}
