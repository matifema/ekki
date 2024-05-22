using System.Linq;

public class World
{
    public Player _player;
	public Map _worldMap;
    public WorldEditor _Editor;
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
        // TODO maybe modifica sta roba
        _Editor = new(_worldMap.grass.Concat(_worldMap.water).Concat(_worldMap.props).ToList());
    }

	public void WorldUpdate()
	{
		_player.CheckForMovement();
	}
}