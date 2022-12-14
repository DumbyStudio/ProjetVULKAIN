using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressRenderer : MonoBehaviour
{
    private SpriteRenderer SR;
    
    
    [SerializeField]
    private Sprite unpressedSprite;
    private Sprite pressedeSprite;
    private KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void OnAKey()
    {
        Debug.Log("A pressed");
    }
}
