﻿using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class Map : Component
{

    // per ogni posizione (x,y) nel dizionario ho una lista di texture
    // ognuna di queste è un livello del tile.
    // nel file ogni tile è divisa da ",", 
    // ogni livello è diviso da "/"

    public Dictionary<Vector2, List<Texture2D>> map = new();
    public int Level;
    public string MapName;
    
    public Map(string Name) 
    {
        MapName = Name;
        LoadMap(Name);
    }

    public void LoadMap(string MapName)
    {
        // la mappa ho deciso (per ora) che è di tipo csv
        // ogni riga nel file è una riga di tile nel gioco
        // ogni elemento delimitato da "," è un indice che punta alla posizione
        //   della texture in Globals.LoadedSprites
        map = new();
        using (StreamReader reader = new StreamReader("Maps/" + MapName + ".txt"))
        {
            int x = 0;
            int y = 0;
            string line;

            // leggo riga da file
            while ((line = reader.ReadLine()) != null)
            {
                var row = line.Split(',').ToList();

                // itero su riga letta
                foreach (string tile in row)
                {
                    // update posizione
                    var posizione = new Vector2(x, y);
                    
                    // empty tile
                    if (tile == "")
                    {
                        x += 16;
                        continue;
                    }
                    
                    // logica gestione livelli
                    if (tile.Contains('/'))
                    {
                        // legge indice da file e lo scrive in dict (x,y),texture 
                        map[posizione] = new List<Texture2D>();
                        var levels = tile.Split('/').ToList();

                        // itero su ogni livello del tile
                        foreach (string level in levels)
                        {
                            map[posizione].Add(Globals.LoadedSprites[int.Parse(level)]);
                        }
                    }
                    else
                    {
                        // legge indice da file e lo scrive in dict (x,y),texture
                        map[posizione] = new List<Texture2D> { Globals.LoadedSprites[int.Parse(tile)] }; 
                    }
                    x += 16;
                }
                x = 0;
                y += 16;
            }
        }
    }

    public void SaveMap()
    {
        using (StreamWriter writer = new StreamWriter("Maps/" + MapName + ".txt"))
        {
            // Get the boundaries of the map
            int maxX = 0;
            int maxY = 0;

            foreach (var key in map.Keys)
            {
                if (key.X > maxX) maxX = (int)key.X;
                if (key.Y > maxY) maxY = (int)key.Y;
            }

            // Write the map to the CSV file
            for (int y = 0; y <= maxY; y+=16)
            {
                for (int x = 0; x <= maxX; x+=16)
                {
                    Vector2 currentPosition = new Vector2(x, y);

                    if (map.TryGetValue(currentPosition, out List<Texture2D> textures))
                    {
                        // Convert the texture list to a string of indices
                        List<int> textureIndices = textures.ConvertAll(texture => Globals.LoadedSprites.IndexOf(texture));
                        string textureIndicesString = string.Join("/", textureIndices);
                        writer.Write(textureIndicesString);
                    }

                    if (x < maxX)
                    {
                        writer.Write(",");
                    }
                }
                writer.WriteLine();
            }
        }
    }

    public void AddTile(int x, int y, Texture2D texture)
    {
        if (map.TryAdd(new Vector2(x, y), new(){texture}))
        {
            // new tile addedd
        }
        else
        {
            // new level added
            map[new Vector2(x, y)].Add(texture);
        }
    }

    public void RemoveTile(int x, int y)
    {
        map.Remove(new Vector2(x, y));
    }

    public override void Update() { }

    public override void Draw()
    {
        // draw loop
        foreach (Vector2 position in map.Keys)
        {
            foreach (Texture2D level in map[position])
            {
                Globals.SpriteBatch.Draw(level, position, Color.White);
            }
        }
    }

}
