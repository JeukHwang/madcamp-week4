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

    // �÷��̾ �������� ������, �������� ��Ȱ��ȭ�ϰ�, �÷��̾��� ���� ���⸦ 2��� �����.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            other.transform.GetChild(0).GetComponent<Light>().range *= 2;
        }
    }
}
