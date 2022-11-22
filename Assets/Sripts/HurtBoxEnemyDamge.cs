using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtBoxEnemyDamge : MonoBehaviour
{
    public short damage = 3;
    public TextMeshProUGUI healthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().health--;
            healthText.text = other.gameObject.GetComponent<PlayerController>().health.ToString();
        }
    }
}
