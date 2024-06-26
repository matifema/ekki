using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Scene : Component
{
	public Map Map;
	public List<Component> components = new();

	public Scene(Map map)
	{
		Map = map;
    }

	public void MoveEntity(Entity entity, Vector2 position)
	{
		entity.currentPosition = position;
	}

	public void SpawnEntity(Entity entity, Vector2 position)
	{
		entity.currentPosition = position;
		entity.LoadSprites();
		components.Add(entity);
	}

    public override void Draw()
    {
		Map.Draw();
		
		foreach(Component comp in components)
		{
			comp.Draw();
		}
	}

    public override void Update()
    {
        foreach(Component comp in components)
		{
			comp.Update();
		}
    }
}