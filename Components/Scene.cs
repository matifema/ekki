using System.Collections.Generic;
using System.Linq;

public class Scene : Component
{
    public Player Player;
	public Map Map;
	public Scene(List<Map> maps)
	{
		// todo modify
		Map = maps[0];
		Player = new();
    }
	public Scene(Map map)
	{
		Map = map;
		Player = new();
    }

    public void LoadAssets()
	{
        Player.LoadSprites();
    }

    public override void Draw()
    {
		Map.Draw();
		Player.Draw();
	}

    public override void Update()
    {
        Player.Update();
    }
}