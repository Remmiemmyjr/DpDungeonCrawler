using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Sets properties for the
// enemy
// DATE MODIFIED: 5/26/2021
// ===============================

public class EnemyProfile : MonoBehaviour
{
    ContentManager info;
    GameObject manager;

    EnemyAI aiController;

    public enum EnemyType { Slime, Cyclops };
    public EnemyType enemy;

    // values determined by information for the stage
    public float health;
    int orbMin;
    int orbMax;


    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        info = manager.GetComponent<ContentManager>();

        aiController = GetComponent<EnemyAI>();

        if (enemy == EnemyType.Slime)
        {
            health = info.slimeHealth;
        }

        if (enemy == EnemyType.Cyclops)
        {
            health = info.cyclopsHealth;
        }

        orbMin = info.minOrbs;
        orbMax = info.maxOrbs;
    }

    private void Update()
    {
        if (health <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(this.gameObject);
    }
}