using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SmithingTable : MonoBehaviour, IInteractable
{

    [SerializeField] private Canvas myUI;
    [SerializeField] private Button closeBtn;
    [SerializeField] private PlayerInput inputs;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    void Start()
    {
        myUI.enabled = false;
        closeBtn.onClick.AddListener(closeUI);
    }

    //called when player is clicking on the object
    public bool Interact(Interactor interactor)
    {
        myUI.enabled = true;
        inputs.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        return true;
    }

    public void closeUI()
    {
        myUI.enabled = false;
        inputs.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
