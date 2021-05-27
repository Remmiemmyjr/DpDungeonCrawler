using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRegistry : MonoBehaviour
{
    bool hit = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !hit)
        {
            other.GetComponent<EnemyProfile>().health -= PlayerController.damage;
            hit = true;
            Debug.Log(other.GetComponent<EnemyProfile>().health);
        }
    }
    private void OnDisable()
    {
        hit = false;
    }
}
