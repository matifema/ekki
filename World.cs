using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection.Metadata;

public class World
{
    public Player _player;
	public Map _worldMap;
	public World()
	{
        // init player and first sprite to down
		_player = new();
		
		// init map
        _worldMap = new();
    }
    public void LoadAssets()
	{
        _player.LoadSprites();
		_worldMap.LoadSprites();
		_worldMap.LoadMap("Spawn");
    }

	public void WorldUpdate()
	{
		_player.CheckForMovement();
	}
}
