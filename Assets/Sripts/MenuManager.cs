using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button buttonStart;
    public Button buttonEnd;

    public string sceneToChangeInto;

    // Start is called before the first frame update
    void Start()
    {
        buttonStart.onClick.AddListener(TaskOnClick01);
        buttonEnd.onClick.AddListener(TaskOnClick02);
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
