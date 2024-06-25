using Microsoft.Xna.Framework.Graphics;

public class ButtonClickEventArgs : System.EventArgs
{
    public Texture2D Texture { get; }

    public ButtonClickEventArgs(Texture2D texture)
    {
        Texture = texture;
    }
}
