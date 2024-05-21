using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

enum Direction {
    UP,
    DOWN,
    LEFT,
    RIGHT
}


public class Player
{
    internal List<Texture2D> upIdle;
    internal List<Texture2D> leftIdle;
    internal List<Texture2D> rightIdle;
    internal List<Texture2D> upWalk;
    internal List<Texture2D> leftWalk;
    internal List<Texture2D> rightWalk;
    internal Vector2 currentPosition = new(0,0);
    internal Direction currentDirection = Direction.DOWN;
    internal Texture2D currentSprite;
    internal List<Texture2D> currentAnimation;
    internal float velocity = 1;

    public Player() { }

    internal void CheckForMovement()
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

        if (pressedKeys.Count() > 0)
        {
            if (pressedKeys.Contains(Keys.W))  { currentPosition.Y -= velocity; currentDirection = Direction.UP; currentAnimation = upWalk;}
            if (pressedKeys.Contains(Keys.S))  { currentPosition.Y += velocity; currentDirection = Direction.DOWN; currentAnimation = leftWalk;}
            if (pressedKeys.Contains(Keys.A))  { currentPosition.X -= velocity; currentDirection = Direction.LEFT; currentAnimation = leftWalk;}
            if (pressedKeys.Contains(Keys.D))  { currentPosition.X += velocity; currentDirection = Direction.RIGHT; currentAnimation = rightWalk;}
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

    internal void Draw(GameTime time)
    {
        if(Globals.renderedFrames % 15 == 0)
        {
            currentSprite = currentAnimation[currentAnimation.IndexOf(currentSprite) == currentAnimation.Count-1 ? 0 : currentAnimation.IndexOf(currentSprite)+1];
        }

        Globals.SpriteBatch.Draw(currentSprite, currentPosition, Color.White);
    }
}
