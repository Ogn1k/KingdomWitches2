using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float speedText = 0.02f;
    public Text dialogueText;
    [TextArea(3, 10)]
    public string[] lines;

    private int index;

    void Start()
    {
        index = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            skipTextClick();
        }
    }

    public void StartDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
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
            StartDialogue();
        }
        else
        {
            index = 0;
            dialogueText.gameObject.SetActive(false);
        }
    }
}
