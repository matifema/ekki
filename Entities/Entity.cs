using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public abstract class Entity : Component
{
    internal float velocity = 1;
    internal Texture2D currentSprite;
    internal Vector2 currentPosition = new(0, 0);
    internal Direction currentDirection = Direction.DOWN;


    public abstract void LoadSprites();

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}