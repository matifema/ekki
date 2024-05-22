﻿using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class Map : Component
{
  public Dictionary<Vector2,Texture2D> map = new();

  public List<Texture2D> grass = new(), water = new(), props = new();

  public Map() { }

  public void LoadSprites()
  {
      grass = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("GrassHills"));
      water = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Lake_Tileset"));
      props = Globals.CutSpriteSheet(Globals.Content.Load<Texture2D>("Props"));
  }

  public void LoadMap(string MapName)
  {
      // la mappa ho deciso (per ora) che è di tipo csv
      // ogni riga nel file è una riga di tile nel gioco
      // il primo char è il tipo di terreno, il resto è l'indice del terreno nella lista del tipo

      Dictionary<Vector2,Texture2D> loadedMap = new();

      using (StreamReader reader = new StreamReader("Maps/" + MapName + ".txt"))
      {
          int x = 0;
          int y = 0;
          string line;

          // leggo riga da file
          while ((line = reader.ReadLine()) != null)
          {
              List<string> row = line.Split(',').ToList();

              // itero su riga letta
              foreach (string tile in row)
              {
                  char terrainType = tile[0];
                  int terrainIndex = int.Parse(tile[1..]);

                  // TODO: Generalizza questo in un dict in Globals o qualcosa del genere.
                  if (terrainType == 'g')
                  {
                      loadedMap[new Vector2(x, y)] = grass[terrainIndex];
                  }
                  if (terrainType == 'w')
                  {
                      loadedMap[new Vector2(x, y)] = water[terrainIndex];
                  }
                  if (terrainType == 'p')
                  {
                      loadedMap[new Vector2(x, y)] = props[terrainIndex];
                  }

                  x += 16;
              }
              x = 0;
              y += 16;

          }
          map = loadedMap;
      }

  }

  public override void Update() { }

  public override void Draw()
  {
      foreach (Vector2 position in map.Keys)
      {
          Globals.SpriteBatch.Draw(map[position], position, Color.White);
      }
      //Globals.SpriteBatch.Draw(grass[0], Vector2.Zero, Color.White);
  }

}
