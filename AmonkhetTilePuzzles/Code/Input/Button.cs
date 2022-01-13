using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AmonkhetTilePuzzles
{
    /* BUTTON CLASS
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/

    /// <summary>
    /// Each button visible in game is an instance of this class
    /// The standard button has one EventHandler (OnClick) 
    /// It will be invoked when the button is released after clicking on it
    /// </summary>
    public class Button
    {
        public event EventHandler OnClick;

        #region Variables

        protected string m_buttonText;
        protected Vector2 m_position;
        protected Vector2 m_windowScaleFactor;

        public GameState[] m_visibleStates;
        protected bool m_isVisible = true;
        protected bool m_isHover = false;
        protected bool m_hasBeenClicked;
        
        protected Color m_textColour = Color.White;

        #endregion

        #region Properties
        public Vector2 ButtonPosition
        {
            get { return this.m_position; }
            private set { this.m_position = value; }
        }
        public bool IsHover 
        { 
            get { return this.m_isHover; }
            private set { this.m_isHover = value; }
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

        // Auto properties are used for button width and height as these are passed straight into the buttonbounds rectangle 
        public int ButtonWidth { get; private set; }
        public int ButtonHeight { get; private set; }

        /// <summary>
        /// The bounds of the button are represented by this rectangle for the purposes of mouse detection
        /// </summary>
        public Rectangle ButtonBounds
        {
            get
            {
                Rectangle boundingRectangle = new Rectangle(
                    new Point((int)(this.ButtonPosition.X * this.m_windowScaleFactor.X), (int)(this.ButtonPosition.Y * this.m_windowScaleFactor.Y)), 
                    new Point((int)(this.ButtonWidth * this.m_windowScaleFactor.X), (int)(this.ButtonHeight * this.m_windowScaleFactor.Y)));

                return boundingRectangle;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// The position, width and height are all relative to the original game window dimensions
        /// These are (1600, 900) - for a window of 1920 in X the windowScaleFactor will become 1920/1600
        /// For example a button at (800, 450) will have it's top left corner in the center of the screen
        /// </summary>
        public Button(Vector2 position, int width, int height, GameState[] visibleStates, string buttonText = "Undefined")
        {
            this.m_windowScaleFactor = new Vector2(1, 1);
            this.ButtonPosition = position;
            this.ButtonWidth = width;
            this.ButtonHeight = height;
            this.ButtonText = buttonText;
            // Visible states are those in the GameState enum for which this instance is visible
            this.m_visibleStates = visibleStates;
        }

        #endregion

        #region Class Methods
        /// <summary>
        /// Called as part of the game update cycle 
        /// The button should check wether the mouse is hovering over it in each update cycle
        /// </summary>
        public virtual void UpdateIt(GameTime gameTime, MouseState currentMouseState, Vector2 windowScaleFactor)
        {
            this.m_windowScaleFactor = windowScaleFactor;
            if (this.IsVisible)
            {
                if (this.ButtonBounds.Contains(currentMouseState.Position))
                {
                    this.m_textColour = Color.Black;
                    this.IsHover = true;
                }
                else
                {
                    this.m_textColour = Color.White;
                    this.IsHover = false;
                }

                /* The onclick event is only invoked after the left mouse button has been released
                 * This is checked once per update cycle, 
                 * setting the hasBeenClicked flag back to false ensures the event only occurs once
                 *************************************************************************************/
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

        /// <summary>
        /// Main draw method takes arguments of the specific textures and fonts to draw for that button
        /// This enables flexibility for different button designs and fonts if desired
        /// </summary>
        public virtual void DrawIt(SpriteBatch spriteBatch, Texture2D normalTexture, Texture2D hoverTexture, Texture2D clickedTexture, SpriteFont font)
        {
            /* As the ButtonBounds rectangle property already factors in the current window scale
             * it can be used as the destination rectangle for the button texture draw
             **************************************************************************************/

            if(this.m_hasBeenClicked)
                spriteBatch.Draw(clickedTexture, this.ButtonBounds, Color.White);
            else if (!this.IsHover)
                spriteBatch.Draw(normalTexture, this.ButtonBounds, Color.White);
            else
                spriteBatch.Draw(hoverTexture, this.ButtonBounds, Color.White);

            // Determining the text position so that it is directly central to the button when drawn
            var x = (this.ButtonBounds.X + (this.ButtonBounds.Width / 2)) - (font.MeasureString(this.ButtonText).X / 2);
            var y = (this.ButtonBounds.Y + (this.ButtonBounds.Height / 2)) - (font.MeasureString(this.ButtonText).Y / 2);
            
            spriteBatch.DrawString(font, this.ButtonText, new Vector2(x, y), this.m_textColour);
        }

        /// <summary>
        /// Second draw method with only 2 arguments
        /// used for buttons that only consist of a single image
        /// such as the puzzle selection grid
        /// </summary>
        public virtual void DrawIt(SpriteBatch spriteBatch, Texture2D imageTexture)
        {
            if (this.m_hasBeenClicked)
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.Black);
            else if (!this.IsHover)
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.Gray);
            else
                spriteBatch.Draw(imageTexture, this.ButtonBounds, Color.White);
        }

        /// <summary>
        /// Called by the input manager using the button manager method when a click is detected
        /// Always called externally and not within the button class itself
        /// If the button was clicked the button will know 
        /// </summary>
        /// <returns> Whether or not the button was clicked </returns>
        public virtual bool CheckIfClicked(MouseState currentMouseState)
        {
            if (this.ButtonBounds.Contains(currentMouseState.Position))
            {
                this.m_hasBeenClicked = true;
                if (!AudioStore.IsMuted)
                {
                    AudioStore.m_clickOnSFX.Play();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        #endregion
    }
}