using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    public Player player;

    void Start()
    {
        if (sentences == null)
        {
            sentences = new Queue<string>();
        }
    }

    public void StartDialogue (Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        Debug.Log("Why");

        player.dialogue = true;

        Debug.Log("Starting conversation with " + dialogue.name);

        nameText.text = dialogue.name;

        if (sentences != null)
        {
            sentences.Clear();
        }
        else
        {
            sentences = new Queue<string>();
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        dialogueText.text = sentence;
    }

    public void EndDialogue ()
    {
        player.dialogue = false;
        animator.SetBool("IsOpen", false);
        Debug.Log("End of conversation");
    }
}
