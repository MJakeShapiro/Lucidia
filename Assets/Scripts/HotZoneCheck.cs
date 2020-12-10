using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyBehaviour enemyParent;
    private bool nRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyBehaviour>();
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (nRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Walk"))
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
            nRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Attack", false);
            nRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            anim.SetBool("canWalk", true);
            enemyParent.inRange = false;
            enemyParent.SelectTarget();
        }
    }
}
