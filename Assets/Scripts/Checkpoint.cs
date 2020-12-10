using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public DialogueTrigger thing;

    private bool done = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.lastCheckpointPos = transform.position;
            if (!done)
            {
                thing.TriggerDialogue();
                done = true;
            }
        }
    }
}
