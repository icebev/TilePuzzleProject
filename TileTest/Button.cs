using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TileTest
{
    public class Button
    {
        protected string m_buttonText;
        public string m_buttonName;
        protected Vector2 m_position;
        protected bool m_isVisible = true;
        protected bool m_isHover = false;
        protected Color m_textColour = Color.White;
        protected bool m_hasBeenClicked;
        protected Vector2 m_windowScaleFactor;

        public GameState[] m_visibleStates;

        public event EventHandler OnClick;



        public Vector2 ButtonPosition
        {
            get { return this.m_position; }
            private set { this.m_position = value; }
        }

        public bool IsVisible
        {
            get { return this.m_isVisible; }
            set { this.m_isVisible = value; }
        }

        public string ButtonText 
        {
            get { return this.m_buttonText; }
            set { this.m_buttonText = value; }
        }

        public int ButtonWidth { get; private set; }
        public int ButtonHeight { get; private set; }

        public Rectangle ButtonBounds
        {
            get
            {
                Rectangle boundingRectangle = new Rectangle(new Point((int)(this.ButtonPosition.X * this.m_windowScaleFactor.X), (int)(this.ButtonPosition.Y *this.m_windowScaleFactor.Y)), new Point((int)(this.ButtonWidth * this.m_windowScaleFactor.X), (int)(this.ButtonHeight * this.m_windowScaleFactor.Y)));

                return boundingRectangle;
            }
        }

        public Button(Vector2 position, int width, int height, GameState[] visibleStates, string buttonText = "Undefined")
        {
            this.m_windowScaleFactor = new Vector2(1, 1);
            this.ButtonPosition = position;
            this.ButtonWidth = width;
            this.ButtonHeight = height;
            this.ButtonText = buttonText;

            this.m_visibleStates = visibleStates;
        }
        public virtual void UpdateIt(GameTime gameTime, MouseState currentMouseState, Vector2 windowScaleFactor)
        {
            this.m_windowScaleFactor = windowScaleFactor;
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
        public virtual void DrawIt(SpriteBatch spriteBatch, Texture2D normalTexture, Texture2D hoverTexture, Texture2D clickedTexture, SpriteFont font)
        {
            //float xTarget = (this.ButtonPosition.X * windowScaleFactor.X);
            //float yTarget = (this.ButtonPosition.Y * windowScaleFactor.Y);
            //float widthTarget = this.ButtonWidth * windowScaleFactor.X;
            //float heightTarget = this.ButtonHeight * windowScaleFactor.Y;
            //Rectangle destinationRectangle = new Rectangle((int)(xTarget), (int)(yTarget), (int)(widthTarget), (int)(heightTarget));

            if(this.m_hasBeenClicked)
                spriteBatch.Draw(clickedTexture, this.ButtonBounds, Color.White);
            else if (!this.m_isHover)
                spriteBatch.Draw(normalTexture, this.ButtonBounds, Color.White);
            else
                spriteBatch.Draw(hoverTexture, this.ButtonBounds, Color.White);


            var x = (this.ButtonBounds.X + (this.ButtonBounds.Width / 2)) - (font.MeasureString(this.ButtonText).X / 2);
            var y = (this.ButtonBounds.Y + (this.ButtonBounds.Height / 2)) - (font.MeasureString(this.ButtonText).Y / 2);
            
            spriteBatch.DrawString(font, this.ButtonText, new Vector2(x, y), this.m_textColour);
        }

        public virtual void DrawIt(SpriteBatch spriteBatch, Texture2D imageTexture)
        {
            if (this.m_hasBeenClicked)
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.Black);
            else if (!this.m_isHover)
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.Gray);
            else
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.White);
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