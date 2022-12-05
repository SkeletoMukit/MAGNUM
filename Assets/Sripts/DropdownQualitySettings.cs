using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownQualitySettings : MonoBehaviour
{
    TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.value = PlayerPrefs.GetInt("GraphicQuality");   
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dropdown.value);
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("GraphicQuality", dropdown.value);
    }
}
