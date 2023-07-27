using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButtonController : MonoBehaviour
{
    public string[] stageNames= { "Stage0", "Stage1" };
    public GameObject levelButton;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < stageNames.Length; i++)
        {
            GameObject button = Instantiate(levelButton);
            button.transform.parent = transform;
            button.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Text text = button.GetComponentInChildren<Text>();
            text.text = (i+1).ToString();
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene(stageNames[index]);
            });
        }
    }
}
