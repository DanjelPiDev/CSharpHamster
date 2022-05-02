using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private HamsterGameManager hamsterGameManager;

    private Hamster _ham1;
    private Hamster _ham2;

    private const string HAMSTER_NAME_PH = "[name]";


    private void Start()
    {
        sentences = new Queue<string>();
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
    }

    public void StartDialogue(Dialogue dialogue, Hamster hamster1, Hamster hamster2)
    {
        sentences.Clear();


        _ham1 = hamster1;
        _ham2 = hamster2;

        /* Search for the hamster which is in the dialogue */
        hamsterGameManager.npcHamsterNameDialogue.SetText(hamster1.Name);
        hamsterGameManager.npcHamsterImageDialogue.sprite = hamster1.HamsterSpriteRenderer.sprite;


        foreach (string sentence in dialogue.sentences)
        {
            /* Change keywords in dialogue */
            if (sentence.Contains(HAMSTER_NAME_PH))
            {
                StringBuilder builder = new StringBuilder(sentence);
                builder.Replace(HAMSTER_NAME_PH, _ham1.Name);
                string _sentence = builder.ToString();
                sentences.Enqueue(_sentence);
            }
            else
            {
                sentences.Enqueue(sentence);
            }
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();


        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        hamsterGameManager.npcHamsterSentenceDialogue.SetText("");
        foreach (char c in sentence)
        {
            hamsterGameManager.npcHamsterSentenceDialogue.text += c;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        _ham1.IsTalking = false;
        _ham2.IsTalking = false;
        Territory.GetInstance().UpdateHamsterProperties(_ham1);
        Territory.GetInstance().UpdateHamsterProperties(_ham2);

        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.dialogueCanvas, false);
    }
}
