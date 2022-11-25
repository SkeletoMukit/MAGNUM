using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateObject : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other)
    {
        objectToActivate.SetActive(true);
        Destroy(this.gameObject);
    }
}
