using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HamsterGame/Dialogue/Condition", fileName = "new dialogue Condition")]
public class DialogueCondition : ScriptableObject
{
    [Header("Default Dialogue properties")]
    [SerializeField] private bool isDefault = true;
    [SerializeField] private bool isDone = false;
    [SerializeField] private bool needsItem = false;
    [SerializeField, ConditionalHide("needsItem")] private Item neededItem;
    [SerializeField] private bool needsQuestDone;
    [SerializeField, ConditionalHide("needsQuestDone")] private Quest questDone;
    [SerializeField] private bool needsQuestStarted;
    [SerializeField, ConditionalHide("needsQuestDone")] private Quest questStarted;

    #region GETTER_SETTER
    public bool IsDefault
    {
        get { return isDefault; }
        set { isDefault = value; }
    }

    public bool IsDone
    {
        get { return isDone; }
        set { isDone = value; }
    }

    public bool NeedsItem
    {
        get { return needsItem; }
        set { needsItem = value; }
    }

    public Item NeededItem
    {
        get { return neededItem; }
        set { neededItem = value; }
    }

    public Quest QuestDone
    {
        get { return questDone; }
        set { questDone = value; }
    }

    public Quest QuestStarted
    {
        get { return questStarted; }
        set { questStarted = value; }
    }
    #endregion
}
