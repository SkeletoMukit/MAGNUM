using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIT : MonoBehaviour
{
    public int hit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            hit++;
            Debug.Log(hit);
        }
    }
}
