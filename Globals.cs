using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class Globals
{
	public static int renderedFrames = 0;
	public static World World;
	public static SpriteFont gameFont {  get; set; }
	public static bool isIngame {  get; set; }
	public static ContentManager Content { get; set; }
	public static SpriteBatch SpriteBatch { get; set; }
	public static Point WindowSize { get; set; }
    public static GraphicsDeviceManager graphics {  get; set; }

    public static List<Texture2D> CutSpriteSheet(Texture2D texture)
    {
        List<Texture2D> textures = new List<Texture2D>();

        int textureWidth = texture.Width;
        int textureHeight = texture.Height;

        int rows = textureHeight / 16;
        int columns = textureWidth / 16;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Create a new texture for each 16x16 block
                Texture2D subTexture = new Texture2D(graphics.GraphicsDevice, 16, 16);

                // Get the data from the original texture
                Color[] data = new Color[16 * 16];
                texture.GetData(0, new Rectangle(col * 16, row * 16, 16, 16), data, 0, 16 * 16);

                // Set the data to the new texture
                subTexture.SetData(data);

                // Add the new texture to the list
                textures.Add(subTexture);
            }
        }

        return textures;
    }
}
