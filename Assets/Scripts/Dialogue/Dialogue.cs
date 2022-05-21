using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueCommands
{
    public Commands command;
    public string commandString;


    public enum Commands
    {
        AddItem,
        RemoveItem,
        AddGrain,
        RemoveGrain
    };
}

[System.Serializable]
public class Dialogue
{
    public bool defaultDialogue = false;
    [TextArea(3, 10)]
    public string[] sentences;

    public List<DialogueCondition> conditions = new List<DialogueCondition>();
    public UnityEvent onDialogueStart;
    public List<DialogueCommands> onDialogueStartCommands;
    public UnityEvent onDialogueEnd;
    public List<DialogueCommands> onDialogueEndCommands;

    private bool dialogueStarts = false;
    private bool dialogueEnds = false;

    public bool DialogueStarts
    {
        get { return dialogueStarts; }
    }

    public bool DialogueEnds
    {
        get { return dialogueEnds; }
    }

    public void OnDialogueStart()
    {
        dialogueStarts = true;
        onDialogueStart?.Invoke();
    }

    public void OnDialogueEnd()
    {
        dialogueStarts = false;
        dialogueEnds = true;
        onDialogueEnd?.Invoke();
        dialogueEnds = false;
    }
}
