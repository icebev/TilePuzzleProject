using Microsoft.Xna.Framework;

namespace TileTest
{
    public class Button
    {
        private string m_buttonText;
        private Vector2 m_position;
        private bool m_isVisible = false;

     
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

    }
}