using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class ScrollableMenu 
{
    // render container
    private Texture2D containerTexture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1);
    public Rectangle container;
    private List<Button> displayedButtons = new();
    private List<Texture2D> renderTiles;
    private int posX, posY, height, width;
    private int scrollValue;
    public Texture2D selectedTile = null;

    public ScrollableMenu (int positionX, int positionY, int hgt, int wdth, List<Texture2D> tiles)
    {
        containerTexture.SetData(new[] {Color.White});

        renderTiles = tiles;
        posX = positionX;
        posY = positionY;
        height = hgt;
        width = wdth;

        container = new Rectangle(positionX, positionY, wdth, hgt);
    }
    
    internal void Draw()
    {
        Globals.SpriteBatch.Draw(containerTexture, container, Color.Black);

        // reset displayedbuttons

        displayedButtons = new();

        // renderizziamo le singole tile
        int x = 1, y = 1;
        foreach(Texture2D texture in renderTiles)
        {
            if ( x + texture.Width >= width)
            {
                x = 1;
                y += texture.Height; 
            }
            if ( y + texture.Height < height )
            {
                Button tileButton = new(texture, Globals.gameFont)
                {
                    Position = new( x + posX,  y + posY)
                };

                tileButton.Click += TileSelection; // evento al click
                displayedButtons.Add(tileButton); // aggiungo ai displayed per update

                tileButton.Draw();

                x += texture.Width;
            }
        }
    }

    internal void Update()
    {
        int rowElements = (int)width / 16;

        foreach (Button button in displayedButtons)
        {
            button.Update();
        }

        // scroll rendering logic
        if (scrollValue > Mouse.GetState().ScrollWheelValue)
        {
            // scrolled down

            var firstRow = renderTiles.Take(rowElements).ToList(); // takes the first row
            renderTiles = renderTiles.Skip(rowElements).Concat(firstRow).ToList(); // moves the first row to the end
        }
        else if (scrollValue < Mouse.GetState().ScrollWheelValue)
        {
            // scrolled up

            var lastRow = renderTiles.Skip(renderTiles.Count() - rowElements).Take(rowElements).ToList(); // takes the last row
            renderTiles = lastRow.Concat(renderTiles.Take(renderTiles.Count() - rowElements)).ToList(); // moves the last row to the start
        }
        scrollValue = Mouse.GetState().ScrollWheelValue;
    }

    private void TileSelection(Object sender, ButtonClickEventArgs e)
    {
        if(selectedTile != e.Texture)
        {
            selectedTile = e.Texture;
        }
        else
        {
            selectedTile = null;
        }
    }
}