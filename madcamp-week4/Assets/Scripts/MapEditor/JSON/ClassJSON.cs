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
    public static GameMap MapToJson(GameObject[] gameObjects, int mapWidth, int mapHeight)
    {
        Tile[] tiles = new Tile[gameObjects.Length];

        print(gameObjects[1]);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            GameObject go = gameObjects[i];

            if (go == null)
            {
                tiles[i] = null;
                continue;
            }

            switch (go.tag)
            {
                case "Player":
                    tiles[i] = new()
                    {
                        type = "Player",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetChild(2).GetComponent<Renderer>().material.color
                    };
                    break;

                case "Torch":
                    tiles[i] = new()
                    {
                        type = "Torch",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        red = go.transform.GetComponent<TorchController>().red,
                        green = go.transform.GetComponent<TorchController>().green,
                        blue = go.transform.GetComponent<TorchController>().blue,
                        on = go.transform.GetComponent<TorchController>().on
                    };
                    break;

                case "Switch":
                    tiles[i] = new()
                    {
                        type = "Switch",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        red = go.transform.GetComponent<TorchSwitchController>().red,
                        green = go.transform.GetComponent<TorchSwitchController>().green,
                        blue = go.transform.GetComponent<TorchSwitchController>().blue,
                    };
                    break;

                case "Door":
                    tiles[i] = new()
                    {
                        type = "Door",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetComponent<Renderer>().material.color,
                        isTransparent = go.transform.GetComponent<DoorController>().isTransparent,
                        switches = ControllerToArray(gameObjects, go.transform.GetComponent<DoorController>().switchControllers)
                    };
                    break;

                case "Exit":
                    tiles[i] = new()
                    {
                        type = "Exit",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y
                    };
                    break;

                case "Wall":
                    tiles[i] = new()
                    {
                        type = "Wall",
                        position = go.transform.position,
                        rotate = go.transform.rotation.y,
                        color = go.transform.GetComponent<Renderer>().material.color,
                        isTransparent = go.transform.GetComponent<WallController>().isTransparent,
                    };
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

    // 문과 연결된 스위치들의 json 속 index을 가져온다.
    private static int[] ControllerToArray(GameObject[] gameObjects, List<TorchSwitchController> switchControllers)
    {
        List<GameObject> objects = gameObjects.ToList();
        return switchControllers.ConvertAll<int>(gameObject => objects.FindIndex(switchController => switchController.gameObject)).ToArray();
    }

    public static GameObject[] JsonToMap(
        string json,
        GameObject PlaneFactory,
        GameObject PlayerFactory,
        GameObject TorchFactory,
        GameObject SwitchFactory,
        GameObject DoorFactory,
        GameObject ExitFactory,
        GameObject WallFactory)
    {
        GameMap gameMap = JsonUtility.FromJson<GameMap>(json);

        // (1) Plane 생성 후 크기와 위치 조정
        Vector3 planePosition = new Vector3(gameMap.width - 1, 0, gameMap.height - 1);
        Vector3 planeScale = new Vector3(gameMap.width / 5f, 1, gameMap.height / 5f);
        Vector2 planeTiling = new Vector2(gameMap.width / 8f, gameMap.height / 8f);

        GameObject newPlane = Instantiate(PlaneFactory, planePosition, Quaternion.identity);
        newPlane.transform.localScale = planeScale;
        newPlane.GetComponent<Renderer>().material.mainTextureScale = planeTiling;


        // (2) json에서 GameObject[]로 변환하기
        GameObject[] gameObjects = new GameObject[gameMap.tiles.Length];

        for (int i = 0; i < gameMap.tiles.Length; i++)
        {
            Tile tile = gameMap.tiles[i];
            Vector3 position = tile.position;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, tile.rotate, 0));

            GameObject go = null;

            switch (tile.type)
            {
                case "Player":
                    go = Instantiate(PlayerFactory, position, rotation);
                    go.transform.GetChild(2).GetComponent<Renderer>().material.color = tile.color;
                    break;

                case "Torch":
                    go = Instantiate(TorchFactory, position, rotation);
                    go.GetComponent<TorchController>().red = tile.red;
                    go.GetComponent<TorchController>().green = tile.green;
                    go.GetComponent<TorchController>().blue = tile.blue;
                    go.GetComponent<TorchController>().on = tile.on;
                    break;

                case "Switch":
                    go = Instantiate(SwitchFactory, position, rotation);
                    go.GetComponent<TorchSwitchController>().red = tile.red;
                    go.GetComponent<TorchSwitchController>().green = tile.green;
                    go.GetComponent<TorchSwitchController>().blue = tile.blue;
                    break;

                case "Door":
                    go = Instantiate(DoorFactory, position, rotation);
                    go.GetComponent<Renderer>().material.color = tile.color;
                    go.GetComponent<DoorController>().isTransparent = tile.isTransparent;
                    go.GetComponent<DoorController>().switchControllers = ArrayToController(tile.switches, gameObjects);
                    break;

                case "Exit":
                    go = Instantiate(ExitFactory, position, rotation);
                    break;

                case "Wall":
                    go = Instantiate(WallFactory, position, rotation);
                    go.GetComponent<Renderer>().material.color = tile.color;
                    go.GetComponent<DoorController>().isTransparent = tile.isTransparent;
                    break;
            }
            gameObjects[i] = go;
        }
        return gameObjects;
    }

    // switch의 index들을 controller로 변환한다.
    private static List<TorchSwitchController> ArrayToController(int[] switches, GameObject[] gameObjects)
    {
        List<TorchSwitchController> switchList = new List<TorchSwitchController>();

        foreach (int index in switches)
        {
            switchList.Add(gameObjects[index].GetComponent<TorchSwitchController>());
        }

        return switchList;
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
    public string type;
    public Vector3 position;
    public float rotate;

    // Player, Door, Wall
    public Color color;

    // Torch, Switch
    public bool red;
    public bool green;
    public bool blue;

    // Torch
    public bool on;

    // Door, Wall
    public bool isTransparent;

    // Door
    public int[] switches;

}
