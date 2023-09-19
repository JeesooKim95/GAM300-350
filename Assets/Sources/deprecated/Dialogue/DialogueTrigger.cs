using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }


    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("Check!");
        if(other.gameObject.CompareTag("Player"))
        {
            TriggerDialogue();
        }   
    }
    
}
