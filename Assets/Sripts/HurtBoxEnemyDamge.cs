using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HurtBoxEnemyDamge : MonoBehaviour
{
    public short damage = 15;
    public PlayerController takeDamage;

    bool bitting = false;
    public float cooldown = 0.5F;
    float cooldownRemaining;

    public EnemyRunnerAI enemyAiScript;

    // Start is called before the first frame update
    void Start()
    {
        cooldownRemaining = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (bitting == true)
        {
            cooldownRemaining -= Time.deltaTime;
            enemyAiScript.speed = 0F;
        }
        if (cooldownRemaining <= 0F)
        {
            bitting = false;
            cooldownRemaining = cooldown;
            enemyAiScript.speed = enemyAiScript.speedDefault;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {   if (bitting == false)
            {
                bitting = true;
                takeDamage.TakeDamage((short)(damage * Random.Range(0.8F, 1.2F)));
            }
        }
    }
}
