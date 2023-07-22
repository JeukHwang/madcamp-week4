using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 플레이어가 아이템을 먹으면, 아이템을 비활성화하고, 플레이어의 빛의 세기를 2배로 만든다.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            other.transform.GetChild(0).GetComponent<Light>().range *= 2;
        }
    }
}
