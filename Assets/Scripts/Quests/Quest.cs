using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "HamsterGame/Quest/Quest", fileName = "new Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea(5, 5)] public string questDescription;
    public StageInfo stageInfo;

#if UNITY_EDITOR
    [Help("Do not change QuestDone or QuestFailed!\nOnly for testing!", UnityEditor.MessageType.Warning)]
#endif
    public bool questDone = false;
    public bool questFailed = false;
}

[System.Serializable]
public struct StageInfo
{
    [TextArea(5, 5)] public string stageDescription;
    public Condition condition;
}
