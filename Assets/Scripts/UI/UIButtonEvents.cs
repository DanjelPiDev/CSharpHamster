/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEvents : MonoBehaviour
{
    private GameObject hamsterUI;

    private void Start()
    {
        hamsterUI = GameObject.Find("HamsterUI");
    }

    public void DisplayTasks()
    {
        hamsterUI.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = hamsterUI.transform.GetChild(3).GetComponent<Toggle>().isOn ? 1 : 0;
        hamsterUI.transform.GetChild(2).GetComponent<CanvasGroup>().blocksRaycasts = hamsterUI.transform.GetChild(3).GetComponent<Toggle>().isOn;
        hamsterUI.transform.GetChild(2).GetComponent<CanvasGroup>().interactable = hamsterUI.transform.GetChild(3).GetComponent<Toggle>().isOn;
    }

    public void StopTrading()
    {
        HamsterGameManager.hamster1.IsTrading = false;
        HamsterGameManager.hamster1.DisplayTradeWindow(HamsterGameManager.hamster1, HamsterGameManager.hamster2);
    }

    public void ChangeActiveQuest()
    {
        HamsterGameManager hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
        QuestCollection questCollection = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<QuestCollection>();

        // Remove all the quest stages from the previous selected quest
        for (int i = 0; i < hamsterGameManager.questContent.childCount; i++)
        {
            Destroy(hamsterGameManager.questContent.GetChild(i).gameObject);
        }

        // Add all the quest stages for the new selected quest
        foreach (Quest quest in questCollection.quests)
        {
            if (string.Compare(quest.questName, hamsterGameManager.questSelector.options[hamsterGameManager.questSelector.value].text) == 0)
            {
                foreach (StageInfo stageInfo in quest.stageInfos)
                {
                    if (stageInfo.isActive || stageInfo.failed || stageInfo.isDone)
                    {
                        GameObject n_quest = Instantiate(hamsterGameManager.questContainer, hamsterGameManager.questContent);
                        n_quest.GetComponent<TextMeshProUGUI>().text = stageInfo.stageDescription;
                    }
                }
            }
        }
    }
}
