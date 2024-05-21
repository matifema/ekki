using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Map
{
    public Dictionary<Vector2,Texture2D> map = new();
    
    private List<Texture2D> grass = new();
    private List<Texture2D> water = new();


    public Map() { }

    public void LoadSprites()
    {
        grass = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("GrassHills"));
        Console.WriteLine(grass.Count);
    }

    public void LoadMap(string MapName)
    {
        // la mappa ho deciso (per ora) che è di tipo csv
        // ogni riga nel file è una riga di tile nel gioco
        // il primo char è il tipo di terreno, il resto è l'indice del terreno nella lista del tipo

        Dictionary<Vector2,Texture2D> loadedMap = new();
        List<List<string>> rows = new();

        using (StreamReader reader = new StreamReader("Maps/" + MapName + ".txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                rows.Add(line.Split(',').ToList());
            }
        }

        int x = 0;
        int y = 0;
        // itero sulle righe
        foreach (List<string> row in rows)
        {
            // itero sulle singole tile
            foreach (string tile in row)
            {
                char terrainType = tile[0];
                int terrainIndex = int.Parse(tile[1..]);

                // TODO: Generalizza questo in un dict in Globals o qualcosa del genere.
                if (terrainType == 'g')
                {
                    loadedMap[new Vector2(x, y)] = grass[terrainIndex];
                }

                x += 16;
            }
            x = 0;
            y += 16;
        }

        map = loadedMap;
    }

    internal void Draw()
    {
        foreach (Vector2 position in map.Keys)
        {
            Globals.SpriteBatch.Draw(map[position], position, Color.White);
        }
        //Globals.SpriteBatch.Draw(grass[0], Vector2.Zero, Color.White);
    }


}
