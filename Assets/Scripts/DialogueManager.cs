using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
public class DialogueManager : MonoBehaviour
{
    //public DialogueTreeController _dialogueTreeController;
    private PlayerController _playerController;
    private Rigidbody _playerRigidbody;
    private RigidbodyConstraints _playerRigidbodyConstraints;
    public void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        _playerRigidbodyConstraints = _playerRigidbody.constraints;
    }

    public void DialogueStart()
    {
        _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("1111");
        _playerController.enabled = false;

    }

    public void DialogueEnd()
    {
        _playerRigidbody.constraints = _playerRigidbodyConstraints;
        _playerController.enabled = true;

    }
   
}
