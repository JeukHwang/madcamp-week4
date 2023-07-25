using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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

    public static GameMap MapToJson(GameObject[] gameObjects, int mapWidth, int mapHeight)
    {
        Debug.Log(gameObjects[0].ToString());
        Debug.Log(gameObjects[0].transform.position.ToString());
        Debug.Log(gameObjects[0].name.ToString());
        Debug.Log(gameObjects[0].tag.ToString());

        Tile[] tiles = new Tile[gameObjects.Length];

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];

            if (go == null) continue;

            switch (go.tag)
            {
                case "Player":
                    Tile tile = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        data = new TilePlayer()
                        {
                            color = go.transform.GetChild(1).GetComponent<Renderer>().material.color
                        }
                    };
                    tiles[i] = tile;
                    break;
/*
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
                        red = go.transform.GetComponent<TorchSwitchController>().red,
                        green = go.transform.GetComponent<TorchSwitchController>().green,
                        blue = go.transform.GetComponent<TorchSwitchController>().blue,
                    };
                    tiles[i] = tileSwitch;
                    break;

                case "Door":
                    TileDoor tileDoor = new()
                    {
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetComponent<Renderer>().material.color,
                        isTransparent = go.transform.GetComponent<DoorController>().isTransparent,
                        switches = getSwitchArray(gameObjects, go.transform.GetComponent<DoorController>().switchControllers)
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
                        color = go.transform.GetChild(1).GetComponent<Renderer>().material.color,
                        isTransparent = go.transform.GetComponent<WallController>().isTransparent,
                    };
                    tiles[i] = tileWall;
                    break;*/
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

    // 문과 연결된 스위치들의 json 속 index을 가져온다.
    private static int[] getSwitchArray(GameObject[] gameObjects, List<TorchSwitchController> switchControllers)
    {
        List<GameObject> objects = gameObjects.ToList();
        return switchControllers.ConvertAll<int>(gameObject => objects.FindIndex(switchController => switchController.gameObject)).ToArray();
    }
}


[Serializable]
public class GameMap
{
    public int width;
    public int height;
    public Tile[] tiles;
}

[Serializable]
public class Tile
{
    public Vector3 position;
    public float rotate = 0;
    public dynamic data;
}

[Serializable]
public class TilePlayer
{
    public string type = "player";
    public Color color;
}

[Serializable]
public class TileTorch
{
    public string type = "torch";
    public bool red = true;
    public bool green = true;
    public bool blue = true;
    public bool on = true;
}

[Serializable]
public class TileSwitch
{
    public string type = "switch";
    public bool red = true;
    public bool green = true;
    public bool blue = true;
}

[Serializable]
public class TileDoor
{
    public string type = "door";
    public Color color;
    public bool isTransparent = false;
    public int[] switches;
}

[Serializable]
public class TileExit
{
    public string type = "exit";
}

[Serializable]
public class TileWall
{
    public string type = "wall";
    public Color color;
    public bool isTransparent = false;
}


