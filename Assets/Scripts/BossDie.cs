using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NodeCanvas.DialogueTrees;

public class BossDie : MonoBehaviour
{
    public DialogueTreeController _dialogueTreeController;
    public void OnDestroy()
    {
        _dialogueTreeController.StartDialogue();
    }
}
