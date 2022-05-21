using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private HamsterGameManager hamsterGameManager;

    private Hamster hamster1;
    private Hamster hamster2;
    private DialogueCondition condition;
    private Dialogue dialogue;

    private const string HAMSTER_NAME_PH = "[name]";
    private const string HAMSTER_PLAYER_PH = "[playername]";


    private void Start()
    {
        sentences = new Queue<string>();
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        
    }

    public void StartDialogue(Dialogue dialogue, Hamster hamster1, Hamster hamster2, DialogueCondition condition = null)
    {
        sentences.Clear();
        this.dialogue = dialogue;
        this.dialogue.OnDialogueStart();

        this.hamster1 = hamster1;
        this.hamster2 = hamster2;
        this.condition = condition;

        /* Search for the hamster which is in the dialogue */
        hamsterGameManager.npcHamsterNameDialogue.SetText(hamster1.Name);
        hamsterGameManager.npcHamsterImageDialogue.sprite = hamster1.HamsterSpriteRenderer.sprite;


        foreach (string sentence in dialogue.sentences)
        {
            /* Change keywords in dialogue */
            /* Replace "[name]" with the actual hamstername */
            if (sentence.Contains(HAMSTER_NAME_PH))
            {
                StringBuilder builder = new StringBuilder(sentence);
                builder.Replace(HAMSTER_NAME_PH, this.hamster1.Name);
                string _sentence = builder.ToString();
                sentences.Enqueue(_sentence);
            }
            /* Replace "[playername]" with the actual hamstername from hamster2 */
            else if (sentence.Contains(HAMSTER_PLAYER_PH))
            {
                StringBuilder builder = new StringBuilder(sentence);
                builder.Replace(HAMSTER_PLAYER_PH, this.hamster2.Name);
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
        this.dialogue.OnDialogueEnd();
        this.hamster1.IsTalking = false;
        this.hamster2.IsTalking = false;
        Territory.GetInstance().UpdateHamsterProperties(this.hamster1);
        Territory.GetInstance().UpdateHamsterProperties(this.hamster2);

        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.dialogueCanvas, false);
        hamsterGameManager.SetCanvasVisibility(hamsterGameManager.generalUI, true);
        this.dialogue = null;
    }
}
