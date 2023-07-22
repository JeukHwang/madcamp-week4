using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveX; // �÷��̾ �̵��� X��ǥ ũ��
    float moveY; // �÷��̾ �̵��� Y��ǥ ũ��

    // ���� �÷��̾�2�̸�, �����¿츦 WASD�� �ٲ۴�.
    public bool player2 = false;

    KeyCode userUp = KeyCode.UpArrow;
    KeyCode userDown = KeyCode.DownArrow;
    KeyCode userLeft = KeyCode.LeftArrow;
    KeyCode userRight = KeyCode.RightArrow;

    KeyCode direction; // �ش� �������� �� ���̴�

    public float moveSpeed = 2f; // �÷��̾� �̵� �ӵ�

    float mouseX;
    float mouseY;

    public float mouseSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        if (player2)
        {
            userUp = KeyCode.W;
            userDown = KeyCode.S;
            userRight = KeyCode.D;
            userLeft = KeyCode.A;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
        mouseY += Input.GetAxis("Mouse Y") * mouseSpeed;
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);
        transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);


        if (Input.GetKey(userRight))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(userLeft))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(userUp))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(userDown))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        */




        
        // ���������� �Է��� Ű���� ������ �������� ȸ���Ѵ�.
        // ���� ���������� �Է��� ���������� Ű���� �Է��� ���� ���, �ٸ� Ű���� �Է��� �ִ��� �̵����� �ʴ´�.
        if (Input.GetKeyDown(userRight))
        {
            direction = userRight;
        }
        else if (Input.GetKeyDown(userLeft))
        {
            direction = userLeft;
        }
        else if (Input.GetKeyDown(userUp))
        {
            direction = userUp;
        }
        else if (Input.GetKeyDown(userDown))
        {
            direction = userDown;
        }

        if (!player2)
        {
            switch (direction)
            {
                case KeyCode.RightArrow:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                case KeyCode.LeftArrow:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
                case KeyCode.UpArrow:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case KeyCode.DownArrow:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case KeyCode.D:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                case KeyCode.A:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
                case KeyCode.W:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case KeyCode.S:
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
            }
        }
        

        // ���� �ٶ󺸰� �ִ� ������ �̵��Ѵ�.
        if (Input.GetKey(direction))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        

        //////////////////////////////////////////////////////////////

        /*
        moveX = 0;
        moveY = 0;

        // ���������� �Է��� Ű���� ������ �������� �̵��Ѵ�.
        // ���� �ش� ���������� Ű���� �Է��� ���� ���, �ٸ� Ű���� �Է��� �־ �̵����� �ʴ´�.
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = KeyCode.RightArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = KeyCode.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = KeyCode.DownArrow;
        }

        if (Input.GetKey(direction))
        {
            switch (direction)
            {
                case KeyCode.RightArrow:
                    moveX += moveSpeed;
                    break;
                case KeyCode.LeftArrow:
                    moveX -= moveSpeed;
                    break;
                case KeyCode.UpArrow:
                    moveY += moveSpeed;
                    break;  
                case KeyCode.DownArrow:
                    moveY -= moveSpeed;
                    break;
            }
            transform.Translate(new Vector3(moveX, moveY));
        }
    }*/
    }
}
