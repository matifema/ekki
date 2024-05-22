using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public class WorldEditor
{
	protected List<Texture2D> tiles;

	public WorldEditor(List<Texture2D> myTiles) 
	{
		tiles = myTiles;
	}

	internal void Draw()
	{
		// diminuisce se scroll down, aumenta se scroll up
		int scrollValue = Mouse.GetState().ScrollWheelValue;

		// render container
		Texture2D containerTexture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1);
		containerTexture.SetData(new[] {Color.White});
		Rectangle container = new Rectangle(Globals.mousePosition.X/4, Globals.mousePosition.Y/4, 171, 102);
		Globals.SpriteBatch.Draw(containerTexture, container, Color.Black);

		// renderizziamo le singole tile
		int x = 1, y = 0;
		foreach(Texture2D texture in tiles)
		{
			if ( x >= 171 )
			{
				x = 1;
				y += 17; 
			}
			if ( y < 102 )
			{
				Globals.SpriteBatch.Draw(texture, new Rectangle(container.X + x, container.Y + y, 16, 16), Color.White);
				x += 17;
			}
		}
	}
}