using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlayed : MonoBehaviour
{
    public static float timePlayed = 0F;

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;
    }
}
