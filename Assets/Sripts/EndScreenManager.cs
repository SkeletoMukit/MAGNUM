using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    public Button buttonReset;
    public Button buttonEnd;

    public string sceneToChangeInto;

    public TextMeshProUGUI textTimePlayed;

    // Start is called before the first frame update
    void Start()
    {
        buttonReset.onClick.AddListener(TaskOnClick01);
        buttonEnd.onClick.AddListener(TaskOnClick02);

        textTimePlayed.text = "Time played:\n" + TimePlayed.timePlayed.ToString() + " sec";
        TimePlayed.timePlayed = 0F;
    }

    void TaskOnClick01()
    {
        SceneManager.LoadScene(sceneToChangeInto);
    }

    void TaskOnClick02()
    {
        Application.Quit();
    }
}
