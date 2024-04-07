using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Dialogue : MonoBehaviour
{
    public float speedText = 0.02f;
    public Text dialogueText;
    public bool activateOnlyOnce;
    public bool onButtonClick;
    public bool stopPlayer;
    public GameObject button;
    [TextArea(3, 10)]
    public string[] lines;

    private int index;
    private bool isActivated;
    private bool inArea;
    private Player player;
    private float moveSpeed;
    private float jumpForce;

    void Start()
    {
        index = 0;
        isActivated = false;
    }

    private void Update()
    {
        if (!isActivated && inArea && onButtonClick && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isActivated)
        {
            skipTextClick();
        }
    }

    public void StartDialogue()
    {
        if (!isActivated)
        {
            if (stopPlayer)
            {
                player.enabled = false; //лучше переделать как-нибудь
            }
            isActivated = true;
            button.SetActive(false);
            dialogueText.gameObject.SetActive(true);
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    public void EndDialogue()
    {
        if (stopPlayer)
        {
            player.enabled = true; //лучше переделать как-нибудь
        }
        index = 0;
        isActivated = false;
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = string.Empty;
        if (activateOnlyOnce)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }

    public void skipTextClick()
    {
        if (dialogueText.text == lines[index])
        {
            NextLines();
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
        }
    }

    void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject.GetComponentInParent<Player>();
            if (!isActivated && !onButtonClick)
            {
                StartDialogue();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = true;
            if (onButtonClick && !isActivated)
            {
                button.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inArea = false;
            if (onButtonClick)
            {
                button.SetActive(false);
            }

            EndDialogue();
        }
    }
}
