using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // 문이 닫혀있을 때의 위치이다.
    float closedX; 
    float closedZ;
    Vector3 closedPos;

    // 문이 열려있을 때의 위치이다.
    public float openedX = 0; 
    public float openedZ = 0;
    Vector3 openedPos;

    float positionY;

    bool isOpened = false; // 현재 완전히 열려있는 상태이다.
    bool isClosed = true; // 현재 완전히 닫혀있는 상태이다.
    public bool shouldOpen; // 문을 열어야 한다. (true면 열어야, false면 닫아야 한다)

    public float moveSpeed = 0.05f; // 문이 열리고 닫히는 속도이다.
    public float threshold = 0.05f; // 문이 현재 완전히 열렸거나 닫혔는지를 판단하는 기준이다.

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

        if(shouldOpen && !isOpened) // 열어야 하는데, 열려 있지 않다.
        {
            // 열리는 방향으로 moveSpeed의 속도로 이동한다.
            transform.position += new Vector3(openedX - transform.position.x, 0, openedZ - transform.position.z).normalized * moveSpeed;

            // 문이 완전히 열렸다고 볼 수 있다.
            if ((transform.position - openedPos).magnitude < threshold)
            {
                transform.position = openedPos;
                isOpened = true;
            }
        }
        else if (!shouldOpen && !isClosed) // 닫아야 하는데, 닫혀 있지 않다.
        {
            // 닫히는 방향으로 moveSpeed의 속도로 이동한다.
            transform.position += new Vector3(closedX - transform.position.x, 0, closedZ - transform.position.z).normalized * moveSpeed;

            // 문이 완전히 닫혔다고 볼 수 있다.
            if ((transform.position - closedPos).magnitude < threshold)
            {
                transform.position = closedPos;
                isClosed = true;
            }
        }
        
    }
}
