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
    private PanelManager _panelManager;
    public GameObject dialogueImage;
    public GameObject shootController;
    public void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
        _playerRigidbodyConstraints = _playerRigidbody.constraints;
        _panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
        
    }

    public void DialogueStart()
    {
        dialogueImage.SetActive(true);
        shootController.SetActive(false);
        _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("1111");
        _panelManager.enabled = false;
        _playerController.enabled = false;

    }

    public void DialogueEnd()
    {
        dialogueImage.SetActive(false);
        shootController.SetActive(true);
        _playerRigidbody.constraints = _playerRigidbodyConstraints;
        _panelManager.enabled = true;
        _playerController.enabled = true;

    }
   
}
