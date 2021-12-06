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

        public bool IsCurrentlySwappable { get; set; }
        public Point CurrentGridPosition { get; set; }
        public Point CorrectGridPosition { get; set; }
        public int GridSize { get; set; }

        Texture2D m_puzzleImage;
        public int PuzzleImageDimensions
        {
            get 
            {
                if (this.m_puzzleImage.Width <= this.m_puzzleImage.Height - IMAGE_TOLERANCE || this.m_puzzleImage.Width >= this.m_puzzleImage.Height + IMAGE_TOLERANCE)
                    Debug.WriteLine("Source image does not have square dimensions.");

                return this.m_puzzleImage.Width;                    
            }
        }

        public float SourceScaleFactor => this.PuzzleImageDimensions / (TILE_DIMENSION * this.GridSize);

        public Tile(Point correctPosition, Texture2D puzzleImage, int gridSize)
        {
            this.CorrectGridPosition = correctPosition;
            this.CurrentGridPosition = correctPosition;
            this.m_puzzleImage = puzzleImage;
            this.GridSize = gridSize;
        }

        public void DrawIt(SpriteBatch spriteBatch)
        {
            Rectangle destinationRectangle = new Rectangle(
                (int)(this.CurrentGridPosition.X * (TILE_DIMENSION + TILE_PADDING)), 
                (int)(this.CurrentGridPosition.Y * (TILE_DIMENSION + TILE_PADDING)), 
                TILE_DIMENSION, 
                TILE_DIMENSION);

            Rectangle sourceRectangle = new Rectangle(
                (int)(this.SourceScaleFactor * this.CorrectGridPosition.X * (TILE_DIMENSION + TILE_PADDING)), 
                (int)(this.SourceScaleFactor * this.CorrectGridPosition.Y * (TILE_DIMENSION + TILE_PADDING)), 
                (int)(TILE_DIMENSION * this.SourceScaleFactor), 
                (int)(TILE_DIMENSION * this.SourceScaleFactor));

            spriteBatch.Draw(this.m_puzzleImage, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
