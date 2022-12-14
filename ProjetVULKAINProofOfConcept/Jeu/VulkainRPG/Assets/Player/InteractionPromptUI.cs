using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Text _promptText;

    private void Start()
    {
        mainCam = Camera.main;
        uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        var rotation = mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool isDisplayed = false;

    public void SetUp(string text)
    {
        _promptText.text = text;
        isDisplayed = true;
        uiPanel.SetActive(true);
    }

    public void Close()
    {
        isDisplayed = false;
        uiPanel.SetActive(false);
    }
}
