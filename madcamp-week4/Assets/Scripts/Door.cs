using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // ���� �������� ���� ��ġ�̴�.
    float closedX; 
    float closedZ;
    Vector3 closedPos;

    // ���� �������� ���� ��ġ�̴�.
    public float openedX = 0; 
    public float openedZ = 0;
    Vector3 openedPos;

    float positionY;

    bool isOpened = false; // ���� ������ �����ִ� �����̴�.
    bool isClosed = true; // ���� ������ �����ִ� �����̴�.
    public bool shouldOpen; // ���� ����� �Ѵ�. (true�� �����, false�� �ݾƾ� �Ѵ�)

    public float moveSpeed = 0.05f; // ���� ������ ������ �ӵ��̴�.
    public float threshold = 0.05f; // ���� ���� ������ ���Ȱų� ���������� �Ǵ��ϴ� �����̴�.

    // Start is called before the first frame update
    void Start()
    {
        closedX = transform.position.x;
        positionY = transform.position.y;
        closedZ = transform.position.z;

        closedPos = new Vector3(closedX, positionY, closedZ);
        openedPos = new Vector3(openedX, positionY, openedZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldOpen)
        {
            isClosed = false;
        }
        else
        {
            isOpened = false;
        }

        if(shouldOpen && !isOpened) // ����� �ϴµ�, ���� ���� �ʴ�.
        {
            // ������ �������� moveSpeed�� �ӵ��� �̵��Ѵ�.
            transform.position += new Vector3(openedX - transform.position.x, 0, openedZ - transform.position.z).normalized * moveSpeed;

            // ���� ������ ���ȴٰ� �� �� �ִ�.
            if ((transform.position - openedPos).magnitude < threshold)
            {
                transform.position = openedPos;
                isOpened = true;
            }
        }
        else if (!shouldOpen && !isClosed) // �ݾƾ� �ϴµ�, ���� ���� �ʴ�.
        {
            // ������ �������� moveSpeed�� �ӵ��� �̵��Ѵ�.
            transform.position += new Vector3(closedX - transform.position.x, 0, closedZ - transform.position.z).normalized * moveSpeed;

            // ���� ������ �����ٰ� �� �� �ִ�.
            if ((transform.position - closedPos).magnitude < threshold)
            {
                transform.position = closedPos;
                isClosed = true;
            }
        }
        
    }
}
