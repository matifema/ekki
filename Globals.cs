using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class Globals
{
	public static World World;
	public static SpriteFont gameFont {  get; set; }
	public static bool isIngame {  get; set; }
	public static ContentManager Content { get; set; }
	public static SpriteBatch SpriteBatch { get; set; }
	public static Point WindowSize { get; set; }
    public static GraphicsDeviceManager graphics {  get; set; }
}
