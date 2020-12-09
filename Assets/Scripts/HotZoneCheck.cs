using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyBehaviour enemyParent;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyBehaviour>();
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Fear_attack"))
        {
            Debug.Log("Something happened here");
            enemyParent.Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Outside hotzone trigger");
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Inside hotzone trigger");
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
