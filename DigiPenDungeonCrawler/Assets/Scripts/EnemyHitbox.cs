using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    bool hit = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && !hit)
        {
            other.GetComponent<PlayerController>().TakeDamage();
            hit = true;
        }
    }
    private void OnDisable()
    {
        hit = false;
        transform.localScale = new Vector3();
    }

    private void OnEnable()
    {
        // transform altered only so ontrigger recognizes a change and deals damage upon being enabled more than once
        transform.localScale = new Vector3(1,1,1);
    }

}
