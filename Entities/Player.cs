using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class Player : Component
{
    internal float velocity = 1;
    internal Texture2D currentSprite;
    internal Vector2 currentPosition = new(0, 0);
    internal Direction currentDirection = Direction.DOWN;
    internal List<Texture2D> upIdle, leftIdle, rightIdle, upWalk, leftWalk, rightWalk, currentAnimation;

    public Player() { }

    internal void CheckForMovement()
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

        if (pressedKeys.Count() > 0)
        {
            if (pressedKeys.Contains(Keys.W)) { currentPosition.Y -= velocity; currentDirection = Direction.UP; currentAnimation = upWalk; }
            if (pressedKeys.Contains(Keys.S)) { currentPosition.Y += velocity; currentDirection = Direction.DOWN; currentAnimation = leftWalk; }
            if (pressedKeys.Contains(Keys.A)) { currentPosition.X -= velocity; currentDirection = Direction.LEFT; currentAnimation = leftWalk; }
            if (pressedKeys.Contains(Keys.D)) { currentPosition.X += velocity; currentDirection = Direction.RIGHT; currentAnimation = rightWalk; }
        }
        else
        {
            if (currentDirection == Direction.LEFT)
            {
                currentAnimation = leftIdle;
            }
            if (currentDirection == Direction.RIGHT)
            {
                currentAnimation = rightIdle;
            }
            if (currentDirection == Direction.UP)
            {
                currentAnimation = upIdle;
            }
            if (currentDirection == Direction.DOWN)
            {
                currentAnimation = leftIdle;
            }
        }
    }

    internal void LoadSprites()
    {
        upIdle = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Idle_02_Back_L-Sheet"));
        leftIdle = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Idle_01_Front_L-Sheet"));
        rightIdle = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Idle_01_Front_R-Sheet"));

        upWalk = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Walk_02_Back_L-Sheet"));
        leftWalk = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Walk_02_Front_L-Sheet"));
        rightWalk = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Walk_02_Front_R-Sheet"));

        currentAnimation = leftIdle;
        currentSprite = leftIdle[0];
    }

    public override void Update() 
    {
        CheckForMovement();
    }

    public override void Draw()
    {
        if (Globals.renderedFrames % 15 == 0)
        {
            currentSprite = currentAnimation[currentAnimation.IndexOf(currentSprite) == currentAnimation.Count - 1 ? 0 : currentAnimation.IndexOf(currentSprite) + 1];
        }

        Globals.SpriteBatch.Draw(currentSprite, currentPosition, Color.White);
    }
}
