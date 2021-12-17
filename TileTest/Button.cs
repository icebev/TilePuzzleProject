using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TileTest
{
    public class Button
    {
        private string m_buttonText;
        private Vector2 m_position;
        private bool m_isVisible = true;
        private bool m_isHover = false;
        private Color m_textColour = Color.White;
        private bool m_hasBeenClicked;

        public event EventHandler OnClick;



        public Vector2 ButtonPosition
        {
            get { return this.m_position; }
            private set { this.m_position = value; }
        }

        public bool IsVisible
        {
            get { return this.m_isVisible; }
            private set { this.m_isVisible = value; }
        }

        public string ButtonText 
        {
            get { return this.m_buttonText; }
            private set { this.m_buttonText = value; }
        }

        public int ButtonWidth { get; private set; }
        public int ButtonHeight { get; private set; }

        public Rectangle ButtonBounds
        {
            get
            {
                Rectangle boundingRectangle = new Rectangle(new Point((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y), new Point(this.ButtonWidth, this.ButtonHeight));

                return boundingRectangle;
            }
        }

        public Button(Vector2 position, int width, int height, string buttonText = "Undefined")
        {
            this.ButtonPosition = position;
            this.ButtonWidth = width;
            this.ButtonHeight = height;
            this.ButtonText = buttonText;
        }
        public void UpdateIt(GameTime gameTime, MouseState currentMouseState)
        {
            if (this.IsVisible)
            {
                if (this.ButtonBounds.Contains(currentMouseState.Position))
                {
                    this.m_textColour = Color.Black;
                    this.m_isHover = true;
                }
                else
                {
                    this.m_textColour = Color.White;
                    this.m_isHover = false;
                }

                if (this.m_hasBeenClicked)
                {
                    if (currentMouseState.LeftButton == ButtonState.Released)
                    {
                        this.m_hasBeenClicked = false;
                        OnClick?.Invoke(this, new EventArgs());
                    }
                }
            }
            
        }
        public void DrawIt(SpriteBatch spriteBatch, Texture2D normalTexture, Texture2D hoverTexture, Texture2D clickedTexture, SpriteFont font)
        {
            var x = (this.ButtonBounds.X + (this.ButtonBounds.Width / 2)) - (font.MeasureString(this.ButtonText).X / 2);
            var y = (this.ButtonBounds.Y + (this.ButtonBounds.Height / 2)) - (font.MeasureString(this.ButtonText).Y / 2);
            
            if(this.m_hasBeenClicked)
                spriteBatch.Draw(clickedTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.White);
            else if (!this.m_isHover)
                spriteBatch.Draw(normalTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.White);
            else
                spriteBatch.Draw(hoverTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.White);


            spriteBatch.DrawString(font, this.ButtonText, new Vector2(x, y), this.m_textColour);
        }

        public void DrawIt(SpriteBatch spriteBatch, Texture2D imageTexture)
        {
            if (this.m_hasBeenClicked)
                spriteBatch.Draw(imageTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.Black);
            else if (!this.m_isHover)
                spriteBatch.Draw(imageTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.Gray);
            else
                spriteBatch.Draw(imageTexture, new Rectangle((int)this.ButtonPosition.X, (int)this.ButtonPosition.Y, this.ButtonWidth, this.ButtonHeight), Color.White);
        }



        public bool CheckIfClicked(MouseState currentMouseState)
        {
            if (this.ButtonBounds.Contains(currentMouseState.Position))
            {
                this.m_hasBeenClicked = true;
                //OnClick?.Invoke(this, new EventArgs());
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}