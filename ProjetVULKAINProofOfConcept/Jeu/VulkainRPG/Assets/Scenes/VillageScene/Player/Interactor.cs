using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private PlayerInput inputs;
    private bool interactInput;

    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI interactionPrompt;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private IInteractable interactable;
    private Outline interactableOutline;

    private void Start()
    {
        inputs = GetComponent<PlayerInput>();
        inputs.actions["Interact"].started += _ => { interactInput = true; };
        inputs.actions["Interact"].canceled += _ => { interactInput = false; };
        interactInput = false;
    }

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);
        if(_numFound > 0)
        {
            var newInteractable = _colliders[0].GetComponent<IInteractable>();
            if (interactable != newInteractable)
            {
                interactable = newInteractable;
                interactableOutline = _colliders[0].transform.Find("Object").Find("Model").gameObject.GetComponent<Outline>();
                interactionPrompt.isDisplayed = false;
            }
            if(interactable != null)
            {
                if (!interactionPrompt.isDisplayed) interactionPrompt.SetUp(interactable.InteractionPrompt);
                if (!interactableOutline.enabled) interactableOutline.enabled = true;
                if (interactInput) interactable.Interact(this);
            }
        }
        else
        {
            if (interactable != null)
            {
                interactableOutline.enabled = false;
                interactable = null;
            }
            if (interactionPrompt.isDisplayed) interactionPrompt.Close();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
