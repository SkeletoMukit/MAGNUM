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

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
        slider.value = PlayerPrefs.GetFloat("Sens");
        if (slider.value == 0) { slider.value = 0.5F; }
        PlayerController.mouseSens = slider.value * 10F;

        buttonStart.onClick.AddListener(TaskOnClick01);
        buttonEnd.onClick.AddListener(TaskOnClick02);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void TaskOnClick01()
    {
        SceneManager.LoadScene(sceneToChangeInto);
    }

    void TaskOnClick02()
    {
        Application.Quit();
    }

    void ValueChangeCheck()
    {
        PlayerController.mouseSens = slider.value * 10F;
        PlayerPrefs.SetFloat("Sens", PlayerController.mouseSens);
    }
}
