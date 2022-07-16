/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "HamsterGame/Quest/Quest", fileName = "new Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    [TextArea(5, 5)] public string questDescription;
    public List<StageInfo> stageInfos;
    public bool startQuestOnStartup = true;
#if UNITY_EDITOR
    [Help("Do not change QuestStarted, QuestDone or QuestFailed!\nOnly for testing!", UnityEditor.MessageType.Warning)]
#endif
    public bool debugQuests = false;
    [ConditionalHide("debugQuests")] public bool questStarted = false;
    [ConditionalHide("debugQuests")] public bool questDone = false;
    [ConditionalHide("debugQuests")] public bool questFailed = false;

    public void StartQuest()
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        if (!this.questStarted && !this.questDone && !this.questFailed)
        {
            this.questStarted = true;
            hamsterGameManager.questSelector.AddOptions(new List<TMP_Dropdown.OptionData> { new TMP_Dropdown.OptionData(this.questName) });
            foreach (StageInfo stageInfo in this.stageInfos)
            {
                if (stageInfo.onStartup)
                {
                    /* Display text in questlog */
                    GameObject n_quest = Instantiate(hamsterGameManager.questContainer, hamsterGameManager.questContent);
                    n_quest.GetComponent<TextMeshProUGUI>().text = stageInfo.stageDescription;
                    stageInfo.isActive = true;
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("Quest already failed, started or is done!");
        }
    }

    public void SetQuestStage(int stage)
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
        StageInfo info = null;

        foreach (StageInfo stageInfo in stageInfos)
        {
            if (stageInfo.stage == stage)
            {
                info = stageInfo;
                stageInfo.isActive = true;
            }
            else if (stageInfo.stage < stage)
            {
                stageInfo.isActive = false;
                stageInfo.isDone = true;
            }
            else if (stageInfo.stage > stage)
            {
                break;
            }
        }

        /* Display text in questlog */
        GameObject n_quest = Instantiate(hamsterGameManager.questContainer, hamsterGameManager.questContent);
        n_quest.GetComponent<TextMeshProUGUI>().text = info.stageDescription;
        info.isActive = true;
    }

    public void CompleteQuest(bool success = true)
    {
        if (success)
        {
            if (this.questStarted && !this.questDone && !this.questFailed)
            {
                this.questDone = true;
                foreach (StageInfo stageInfo in this.stageInfos)
                {
                    stageInfo.isActive = false;
                    stageInfo.isDone = true;
                }
            }
            else
            {
                Debug.LogError("Quest already failed or is done!");
            }
        }
        else
        {
            if (this.questStarted && !this.questDone && !this.questFailed)
            {
                this.questFailed = true;
                foreach (StageInfo stageInfo in this.stageInfos)
                {
                    stageInfo.isActive = false;
                    stageInfo.failed = true;
                }
            }
            else
            {
                Debug.LogError("Quest already failed or is done!");
            }
        }
    }
}

[System.Serializable]
public class QuestCommands
{
    public Commands command;
    public string commandString;


    public enum Commands
    {
        SetStage,
        CheckGrainCount
    };
}

[System.Serializable]
public class StageInfo
{
    public int stage;
    [TextArea(5, 5)] public string stageDescription;
    public Condition condition;
    public Condition failCondition;
    public List<QuestCommands> questCommands;
    public bool onStartup;
    public bool questDone;
    public bool questFailed;
    public bool debug;
    [ConditionalHide("debug")] public bool isActive;
    [ConditionalHide("debug")] public bool isDone;
    [ConditionalHide("debug")] public bool failed;

    
}

