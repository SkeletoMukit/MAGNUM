using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public Button buttonResume;
    public Button buttonEnd;

    public Slider slider;

    public GameObject Level;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Sens");
        if (slider.value == 0) { slider.value = 0.5F; }
        PlayerController.mouseSens = slider.value * 10F;

        buttonResume.onClick.AddListener(TaskOnClick01);
        buttonEnd.onClick.AddListener(TaskOnClick02);
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    void TaskOnClick01()
    {
        Level.SetActive(true);
        Cursor.visible = false;
        PauseMenu.SetActive(false);
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
