using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClassJSON : MonoBehaviour
{
    public static GameMap[] gameMaps;

    // Start is called before the first frame update
    void Start()
    {
        gameMaps = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameMap MapToJson(GameObject[] gameObjects, int mapWidth, int mapHeight)
    {
        Tile[] tiles = new Tile[gameObjects.Length];

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];

            switch (go.tag)
            {
                case "Player":
                    TilePlayer tilePlayer = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetChild(1).GetComponent<Renderer>().material.color
                    };
                    tiles[i] = tilePlayer;
                    break;

                case "Torch":
                    TileTorch tileTorch = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        red = go.transform.GetComponent<TorchController>().red,
                        green = go.transform.GetComponent<TorchController>().green,
                        blue = go.transform.GetComponent<TorchController>().blue,
                        on = go.transform.GetComponent<TorchController>().on
                    };
                    tiles[i] = tileTorch;
                    break;

                case "Switch":
                    TileSwitch tileSwitch= new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        red = go.transform.GetComponent<SwitchController>().red,
                        green = go.transform.GetComponent<SwitchController>().green,
                        blue = go.transform.GetComponent<SwitchController>().blue,
                    };
                    tiles[i] = tileSwitch;
                    break;

                case "Door":
                    TileDoor tileDoor = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetComponent<DoorController>().material.color,
                        isTransparent = go.transform.GetComponent<DoorController>().isTransparent,
                        openOrientation = go.transform.GetComponent<DoorController>().openOrientation,
                        open = go.transform.GetComponent<DoorController>().open,
                        switches = go.transform.GetComponent<DoorController>().switches
                    };
                    tiles[i] = tileDoor;
                    break;

                case "Exit":
                    TileExit tileExit = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y
                    };
                    tiles[i] = tileExit;
                    break;

                case "Wall":
                    TileWall tileWall = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetChild(1).GetComponent<WallController>().material.color,
                        isTransparent = go.transform.GetComponent<WallController>().isTransparent,
                    };
                    tiles[i] = tileWall;
                    break;
            }
        }
        GameMap gameMap = new()
        {
            width = mapWidth,
            height = mapHeight,
            tiles = tiles
        };
        return gameMap;
    }
}

[Serializable]
public class GameMap
{
    public int width;
    public int height;
    public Tile[] tiles;
}

public class Tile
{
    public Vector3 position;
    public float rotate = 0;
}

public class TilePlayer : Tile
{
    public string type = "player";
    public Color color;
}

public class TileTorch : Tile
{
    public string type = "torch";
    public bool red = true;
    public bool green = true;
    public bool blue = true;
    public bool on = true;
}

public class TileSwitch : Tile
{
    public string type = "switch";
    public bool red = true;
    public bool green = true;
    public bool blue = true;
}

public class TileDoor : Tile
{
    public string type = "door";
    public Color color;
    public bool isTransparent = false;
    public string openOrientation; // : 'up' | 'down' | 'left' | 'right';
    public bool open = false;
    public int[] switches;
}

public class TileExit : Tile
{
    public string type = "exit";
}

public class TileWall : Tile
{
    public string type = "wall";
    public Color color;
    public bool isTransparent = false;
}


