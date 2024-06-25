using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class WorldEditor: Component
{
	private ScrollableMenu menu;
	private FollowMouseTexture fllwMouse;
    private KeyboardState _previousKeypress;

    public WorldEditor(List<Texture2D> myTiles) 
	{
		Point mousePosition = Globals.GetMousePosition();
		menu = new ScrollableMenu(mousePosition.X, mousePosition.Y, 162, 162, myTiles);

		fllwMouse = new FollowMouseTexture();
	}

	public override void Draw()
	{
		menu.Draw();
		
		if(menu.selectedTile != null)
		{
			fllwMouse.SetTexture(menu.selectedTile);
			fllwMouse.Draw();	
		}
	}

	public override void Update()
	{
		// keystroke checks
		if (Keyboard.GetState().IsKeyDown(Keys.F2) && Keyboard.GetState() != _previousKeypress)
		{
			Console.WriteLine("Saving map...");
			Globals.CurrentScene.Map.SaveMap();
			Console.WriteLine("Map saved!");
		}
		if (Keyboard.GetState().IsKeyDown(Keys.F3) && Keyboard.GetState() != _previousKeypress)
		{
			Console.WriteLine("Reloading map...");
			Globals.CurrentScene.Map.LoadMap("Spawn");
			Console.WriteLine("Map reloaded!");
		}

		// updates
		menu.Update();
		fllwMouse.Update();

		// if the user has selected a texture from editor and left clicks on screen
		// we add that texture to the map in the position where they clicked
		// todo still a bit shitty
		if(fllwMouse.IsActive && Globals.HasNotClickedHere(menu.container, true))
		{
			Point position = Globals.GetMousePosition();

			// calculate tile's position from mouse
			int xTile = position.X - position.X % 16;
			int yTile = position.Y - position.Y % 16;

			Texture2D TileTexture = fllwMouse.texture;

			// actually add tile to 
			Globals.CurrentScene.Map.AddTile(xTile, yTile, TileTexture);
		}

		// if the user right clicks on texture delete that tile
		if(Globals.HasNotClickedHere(menu.container, false))
		{
			Point position = Globals.GetMousePosition();

			// calculate tile's position from mouse
			int xTile = position.X - position.X % 16;
			int yTile = position.Y - position.Y % 16;

			// actually remove tile 
			Globals.CurrentScene.Map.RemoveTile(xTile, yTile);
		}
		_previousKeypress = Keyboard.GetState();

	}
}