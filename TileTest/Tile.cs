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
        public const int TILE_PADDING = 10;
        public const int IMAGE_TOLERANCE = 5;
        public const int ANIMATION_TOLERANCE = 2;
        public const float ANIMATION_SPEED = 300f;


        public bool IsCurrentlySwappable { get; set; }

        public bool IsInAnimation { get; set; }
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int GridSize { get; set; }

        private readonly Texture2D m_puzzleImage;

        private Point m_tileAnimatedDrawPosition;
        public Point TileFinalDrawPosition
        {
            get
            {
                Point drawTarget = new Point((int)(this.CurrentGridPosition.X * (TILE_DIMENSION + TILE_PADDING)),
                    (int)(this.CurrentGridPosition.Y * (TILE_DIMENSION + TILE_PADDING)));

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

        public Tile(Point correctPosition, Texture2D puzzleImage, int gridSize)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.m_puzzleImage = puzzleImage;
            this.GridSize = gridSize;

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

            spriteBatch.Draw(this.m_puzzleImage, destinationRectangle, sourceRectangle, Color.White);
        }

        public void UpdateIt(GameTime gameTime)
        {
            if (this.m_tileAnimatedDrawPosition.X < this.TileFinalDrawPosition.X)
                this.m_tileAnimatedDrawPosition.X += (int)(ANIMATION_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
            else if (this.m_tileAnimatedDrawPosition.X > this.TileFinalDrawPosition.X)
                this.m_tileAnimatedDrawPosition.X -= (int)(ANIMATION_SPEED * gameTime.ElapsedGameTime.TotalSeconds);

             if (this.m_tileAnimatedDrawPosition.Y < this.TileFinalDrawPosition.Y)
                this.m_tileAnimatedDrawPosition.Y += (int)(ANIMATION_SPEED * gameTime.ElapsedGameTime.TotalSeconds);
            else if (this.m_tileAnimatedDrawPosition.Y > this.TileFinalDrawPosition.Y)
                this.m_tileAnimatedDrawPosition.Y -= (int)(ANIMATION_SPEED * gameTime.ElapsedGameTime.TotalSeconds);


            if (Math.Abs(this.m_tileAnimatedDrawPosition.X - this.TileFinalDrawPosition.X) <= ANIMATION_TOLERANCE && Math.Abs(this.m_tileAnimatedDrawPosition.Y - this.TileFinalDrawPosition.Y) <= ANIMATION_TOLERANCE)
            {
                this.m_tileAnimatedDrawPosition = this.TileFinalDrawPosition;
            }
        }
    }
}
