using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class FollowMouseTexture : Component
{
    public Texture2D texture;
    private Rectangle drawRectangle;
    private Point msPosition;
    public bool IsActive = false;

    public FollowMouseTexture() 
    {
        msPosition = Globals.GetMousePosition();
        drawRectangle = new Rectangle(msPosition.X, msPosition.Y, 0, 0);
    }


    public override void Draw()
    {
        Globals.SpriteBatch.Draw(texture, drawRectangle, Color.White);
    }

    public override void Update()
    {
        msPosition = Globals.GetMousePosition();
        drawRectangle.X = msPosition.X;
        drawRectangle.Y = msPosition.Y;

        IsActive = texture != null;
    }

    public void SetTexture(Texture2D texture2D)
    {
        texture = texture2D;
        drawRectangle.Width = texture.Width;
        drawRectangle.Height = texture.Height;
    }
}