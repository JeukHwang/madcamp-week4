using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButtonController : MonoBehaviour
{
    public GameObject levelButton;
    public int levelNumber;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        for (int index = 1; index <= levelNumber; index++)
        {
            GameObject button = Instantiate(levelButton);
            button.transform.parent = transform;
            Text text = button.GetComponentInChildren<Text>();
            text.text = index.ToString();
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Stage");
            });
        }
    }

    void loadLevel()
    {

    }
}
