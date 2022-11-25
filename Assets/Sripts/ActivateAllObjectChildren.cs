using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAllObjectChildren : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                parentObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
