using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public bool defaultDialogue = false;
    [TextArea(3, 10)]
    public string[] sentences;

    public List<DialogueCondition> conditions = new List<DialogueCondition>();
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    public void OnDialogueStart()
    {
        onDialogueEnd?.Invoke();
    }

    public void OnDialogueEnd()
    {
        onDialogueEnd?.Invoke();
    }
}
