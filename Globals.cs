using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;

public static class Globals
{
    public static int renderedFrames = 0;
    public static GameTime time { get; set; }
    public static List<Texture2D> LoadedSprites = new();
    internal static Scene CurrentScene;
    private static MouseState _currentMouse, _previousMouse = Mouse.GetState();

    public static bool isIngame { get; set; }
    internal static bool isEditing { get; set; }
    public static Point WindowSize { get; set; }
    public static SpriteFont gameFont { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDeviceManager graphics { get; set; }

    public static Point GetMousePosition()
    {
        return new Point(Mouse.GetState().X / 4, Mouse.GetState().Y / 4);
    }

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

    public static void LoadSprites(string SpriteSheet)
    {
        LoadedSprites.AddRange(CutSpriteSheet(Content.Load<Texture2D>(SpriteSheet)));
    }

    public static bool HasClickedHere(Rectangle rectangle, bool isLeftClick)
    {
        var result = false;
        _currentMouse = Mouse.GetState();
        
        var currentBtn = isLeftClick ? _currentMouse.LeftButton : _currentMouse.RightButton; 
        var prevBtn = isLeftClick ? _previousMouse.LeftButton : _previousMouse.RightButton; 

        var mouseRectangle = new Rectangle(_currentMouse.X/4, _currentMouse.Y/4, 1, 1);

        if(mouseRectangle.Intersects(rectangle))
        {
            if(currentBtn == ButtonState.Released && prevBtn == ButtonState.Pressed)
            {
                result = true;
            }
        }
        _previousMouse = _currentMouse;

        return result;
    }

    // todo:: remove
    public static bool HasNotClickedHere(Rectangle rectangle, bool isLeftClick)
    {
        var result = false;
        _currentMouse = Mouse.GetState();

        var currentBtn = isLeftClick ? _currentMouse.LeftButton : _currentMouse.RightButton; 
        var prevBtn = isLeftClick ? _previousMouse.LeftButton : _previousMouse.RightButton; 

        var mouseRectangle = new Rectangle(_currentMouse.X/4, _currentMouse.Y/4, 1, 1);

        if(!mouseRectangle.Intersects(rectangle))
        {
            if(currentBtn == ButtonState.Released && prevBtn == ButtonState.Pressed)
            {
                result = true;
            }
        }
        _previousMouse = _currentMouse;

        return result;
    }

    public static bool HasClicked(bool isLeftClick)
    {
        var result = false;
        _currentMouse = Mouse.GetState();

        var currentBtn = isLeftClick ? _currentMouse.LeftButton : _currentMouse.RightButton; 
        var prevBtn = isLeftClick ? _previousMouse.LeftButton : _previousMouse.RightButton; 

        if(currentBtn == ButtonState.Released && prevBtn == ButtonState.Pressed)
        {
            result = true;
        }
        _previousMouse = _currentMouse;

        return result;
    }
}