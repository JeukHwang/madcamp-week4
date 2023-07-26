using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SizeInputField : MonoBehaviour
{
    public GameObject Plane;

    private void Start()
    {
        // ó���� �ԷµǾ� �ִ� ������ �����Ѵ�.
        float initWidth = float.Parse(gameObject.transform.GetChild(0).GetComponent<TMP_InputField>().text);
        float initHeight = float.Parse(gameObject.transform.GetChild(1).GetComponent<TMP_InputField>().text);

        Plane.transform.position = new Vector3(initWidth - 1, Plane.transform.position.y, initHeight - 1);
        Plane.transform.localScale = new Vector3(initWidth / 5f, Plane.transform.localScale.y, initHeight / 5f);
        Plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(initWidth / 8f, initHeight / 8f);
        Camera.main.transform.position = new Vector3(initWidth - 5, Camera.main.transform.position.y, initHeight - 1);
    }

    public void OnEndEditValue(GameObject inputFieldObject)
    {
        int newValue = 1;
        string inputFieldName = inputFieldObject.name;

        if (inputFieldObject.GetComponent<TMP_InputField>().text != "")
        {
            newValue = (int)float.Parse(inputFieldObject.GetComponent<TMP_InputField>().text);
        }

        // �ּ� ���� 1, �ִ� ���� 100
        if (newValue < 0) newValue = 1;
        else if (newValue > 100) newValue = 100;

        // �ٲ� ���� input field�� �ݿ��Ѵ�.
        inputFieldObject.GetComponent<TMP_InputField>().text = newValue.ToString();


        // (1) ���ο� �Է°��� �°� ���� ũ�� �� ��ġ, ī�޶� ��ġ�� �����Ѵ�.
        float newPosition = newValue - 1;
        float newscale = newValue / 5f;
        float newTiling = newValue / 8f;

        Vector3 prevPosition = Plane.transform.position;
        Vector3 prevScale = Plane.transform.localScale;
        Vector2 prevTiling = Plane.GetComponent<Renderer>().material.mainTextureScale;
        Vector3 prevCamPos = Camera.main.transform.position;

        switch (inputFieldName)
        {
            case "WidthInputField":
                Plane.transform.position = new Vector3(newPosition, prevPosition.y, prevPosition.z);
                Plane.transform.localScale = new Vector3(newscale, prevScale.y, prevScale.z);
                Plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(newTiling, prevTiling.y);
                Camera.main.transform.position = new Vector3(newPosition - 4, prevCamPos.y, prevCamPos.z);
                break;

            case "HeightInputField":
                Plane.transform.position = new Vector3(prevPosition.x, prevPosition.y, newPosition);
                Plane.transform.localScale = new Vector3(prevScale.x, prevScale.y, newscale);
                Plane.GetComponent<Renderer>().material.mainTextureScale = new Vector2(prevTiling.x, newTiling);
                Camera.main.transform.position = new Vector3(prevCamPos.x, prevCamPos.y, newPosition);
                break;
        }

        // (2) ���� ���� ũ�Ⱑ �پ��ٸ�, �� ũ�Ⱑ �۾����鼭 �� ������ �з����� �� ������Ʈ���� �����Ѵ�.
        int prevWidth = MapEditor.mapWidth;
        int prevHeight = MapEditor.mapHeight;

        switch (inputFieldName)
        {
            case "WidthInputField":
                if (newValue < prevWidth)
                {
                    for (int j = 0; j < prevHeight; j++)
                    {
                        for (int i = newValue; i < prevWidth; i++)
                        {
                            int index = i + prevWidth * j;
                            print(index);
                            Destroy(MapEditor.gameObjects[index]); // �� �󿡼� ����
                            MapEditor.gameObjects[index] = null; // ����Ʈ �󿡼� ����
                        }
                    }
                    MapEditor.mapWidth = newValue;
                }
                break;

            case "HeightInputField":
                if (newValue < prevHeight)
                {
                    for (int j = newValue; j < prevHeight; j++)
                    {
                        for (int i = 0; i < prevWidth; i++)
                        {
                            int index = i + prevWidth * j;
                            print(index);
                            Destroy(MapEditor.gameObjects[index]);
                            MapEditor.gameObjects[index] = null;
                        }
                    }
                    MapEditor.mapHeight = newValue;
                } 
                break;
        }

        // (3) gameObjects�� ������ ���ο� Length�� �°� �������Ѵ�.
        GameObject[] newGameObjects;

        switch (inputFieldName)
        {
            case "WidthInputField":
                newGameObjects = new GameObject[newValue * prevHeight];
                for (int j = 0; j < prevHeight; j++)
                {
                    Array.Copy(MapEditor.gameObjects, prevWidth * j, newGameObjects, newValue * j, newValue);
                }
                MapEditor.gameObjects = newGameObjects;
                break;

            case "HeightInputField":
                newGameObjects = new GameObject[prevWidth * newValue];
                Array.Copy(MapEditor.gameObjects, 0, newGameObjects, 0, newGameObjects.Length);
                MapEditor.gameObjects = newGameObjects;
                break;
        }
    }
}
