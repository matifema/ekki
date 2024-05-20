using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public Dictionary<string, Texture2D> playerSprites = new();
    internal Texture2D currentSprite = Globals.Content.Load<Texture2D>("down");
    internal Vector2 currentPosition = new(0,0);
    internal float velocity = 1;

    public Player() 
    {
        LoadSprites(Globals.Content);
    }

    internal void CheckForMovement()
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
        
        if (pressedKeys.Contains(Keys.W))  { currentSprite = playerSprites["up"]; currentPosition.Y -= velocity; }
        if (pressedKeys.Contains(Keys.S))  { currentSprite = playerSprites["down"]; currentPosition.Y += velocity; }
        if (pressedKeys.Contains(Keys.A))  { currentSprite = playerSprites["left"]; currentPosition.X -= velocity; }
        if (pressedKeys.Contains(Keys.D))  { currentSprite = playerSprites["right"]; currentPosition.X += velocity; }
        
    }

    internal void LoadSprites(ContentManager Content)
    {
        playerSprites.Add("up",Content.Load<Texture2D>("up"));
        playerSprites.Add("down", Content.Load<Texture2D>("down"));
        playerSprites.Add("left", Content.Load<Texture2D>("left"));
        playerSprites.Add("right",Content.Load<Texture2D>("right"));
    }

    internal void Draw()
    {
        Globals.SpriteBatch.Draw(currentSprite, currentPosition, Color.White);
    }
}
