using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* TOGGLE BUTTON CLASS
     * Last modified by Joe Bevis 12/01/2022
     ****************************************/

    /// <summary>
    /// The togglebutton inherits from the regular button class
    /// The draw and update methods are overridden to enable toggle functionality
    /// The togglebutton has one event handler - OnToggle; easily distinguishable from OnClick
    /// </summary>
    public class ToggleButton : Button
    {
        public event EventHandler OnToggle;

        #region Variables
        private bool m_toggledState;

        #endregion

        #region Properties
        public bool ToggledState
        {
            get { return this.m_toggledState; }
            set { this.m_toggledState = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor is empty - the base (button) constructor can be used to full effect
        /// </summary>
        public ToggleButton(Vector2 position, int width, int height, GameState[] visibleStates, string buttonText = "Undefined") 
            : base(position, width, height, visibleStates, buttonText)
        {

        }
        #endregion

        #region Class Methods
        /// <summary>
        /// The update method differs very slightly from base button update method 
        /// The togglestate value is changed to keep track of the toggle
        /// </summary>
        public override void UpdateIt(GameTime gameTime, MouseState currentMouseState, Vector2 windowScaleFactor)
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
            }
            if (this.m_hasBeenClicked)
            {
                if (currentMouseState.LeftButton == ButtonState.Released)
                {
                    
                    this.m_hasBeenClicked = false;
                    this.ToggledState = !this.ToggledState;
                    OnToggle?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Depending on the toggle state, the button appears light or dark to indicate it
        /// </summary>
        public override void DrawIt(SpriteBatch spriteBatch, Texture2D normalTexture, Texture2D hoverTexture, Texture2D clickedTexture, SpriteFont font)
        {

            if (this.m_isHover)
                spriteBatch.Draw(hoverTexture, this.ButtonBounds, Color.White);
            else if (this.ToggledState)
                spriteBatch.Draw(clickedTexture, this.ButtonBounds, Color.White);
            else
                spriteBatch.Draw(normalTexture, this.ButtonBounds, Color.White);

            var x = (this.ButtonBounds.X + (this.ButtonBounds.Width / 2)) - (font.MeasureString(this.ButtonText).X / 2);
            var y = (this.ButtonBounds.Y + (this.ButtonBounds.Height / 2)) - (font.MeasureString(this.ButtonText).Y / 2);

            spriteBatch.DrawString(font, this.ButtonText, new Vector2(x, y), this.m_textColour);
        }

        /// <summary>
        /// A different sound effect should play when the button is untoggled vs toggled
        /// </summary>
        public override bool CheckIfClicked(MouseState currentMouseState)
        {
            if (this.ButtonBounds.Contains(currentMouseState.Position))
            {
                this.m_hasBeenClicked = true;
                if (!AudioStore.IsMuted)
                {
                    if (!this.ToggledState)
                        AudioStore.m_clickOnSFX.Play();
                    else
                        AudioStore.m_clickOffSFX.Play();
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
