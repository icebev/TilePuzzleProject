using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTest
{
    public class ToggleButton : Button
    {
        private bool m_toggledState;

        public event EventHandler OnToggle;

        public bool ToggledState
        {
            get { return this.m_toggledState; }
            set { this.m_toggledState = value; }
        }
        public ToggleButton(Vector2 position, int width, int height, GameState[] visibleStates, string buttonText = "Undefined") : base(position, width, height, visibleStates, buttonText)
        {

        }

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

        public override bool CheckIfClicked(MouseState currentMouseState)
        {
            if (this.ButtonBounds.Contains(currentMouseState.Position))
            {
                this.m_hasBeenClicked = true;
                if (!AudioStore.m_isMuted)
                {
                    if (!this.ToggledState)
                        AudioStore.m_clickOnSFX.Play();
                    else
                        AudioStore.m_clickOffSFX.Play();
                }
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
