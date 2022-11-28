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

    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        slider.value = PlayerPrefs.GetFloat("Sens")/10F;

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
