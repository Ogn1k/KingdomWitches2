using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class EventTrigger : MonoBehaviour
{
    private bool isActivated;
    private bool inArea;

    public bool onButtonClick;
    public GameObject button;
    public UnityEvent TriggerActivate;

    private void Update()
    {
        if (isActivated || !onButtonClick || !inArea)
        {
            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isActivated = true;
                TriggerActivate.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isActivated)
        {
            inArea = true;
            if (!onButtonClick)
            {
                isActivated = true;
                TriggerActivate.Invoke();
            }
            else
            {
                button.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && onButtonClick)
        {
            inArea = false;
            button.SetActive(false);
        }
    }

    
}
