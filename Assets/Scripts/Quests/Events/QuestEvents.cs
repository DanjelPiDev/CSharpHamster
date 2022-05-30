using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public void CheckGlobalGrainCount(Quest quest)
    {
        foreach (StageInfo stageInfo in quest.stageInfos)
        {
            if (stageInfo.isActive && Territory.globalGrainCount == 0 && stageInfo.condition.pickUpAllGrains && stageInfo.questDone)
            {
                //quest.questDone = true;
                stageInfo.isDone = true;
            }
            else if (stageInfo.isActive && Territory.globalGrainCount == Int32.Parse(stageInfo.questCommands[0].commandString) && stageInfo.condition.dropAllGrains && stageInfo.questDone)
            {
                //quest.questDone = true;
                stageInfo.isDone = true;
            }
            else if (stageInfo.isActive && Territory.globalGrainCount == 0 && stageInfo.condition.pickUpAllGrains)
            {
                foreach (QuestCommands command in stageInfo.questCommands)
                {
                    if (command.command == QuestCommands.Commands.SetStage)
                    {
                        quest.SetQuestStage(Int32.Parse(command.commandString));
                    }
                }
            }
        }
    }

    public void CheckHamsterGrainCount(Quest quest)
    {
        int activHamsters = Territory.activHamsters.Count;
        int counter = 0;

        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.GetGrainCount() == 0)
            {
                counter += 1;
            }
        }

        if (counter == activHamsters)
        {
            quest.questDone = true;
        }
    }

    public void AllTilesNeedSpecificGrainCount(Quest quest)
    {
        foreach (StageInfo stageInfo in quest.stageInfos)
        {
            if (!stageInfo.condition.needSpecificAmountOfGrains || !stageInfo.isActive) return;
        }

        int tileCount = 0;
        int tileGrainCounter = 0;

        for (int i = 0; i < Territory.tileCollection.childCount; i++)
        {
            foreach (StageInfo stageInfo in quest.stageInfos)
            {
                if (Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.type != Tile.TileType.Wall)
                {
                    tileCount += 1;
                }

                if (Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.type != Tile.TileType.Wall &&
                    Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount == stageInfo.condition.specificAmountOfGrains)
                {
                    tileGrainCounter += 1;
                }
            }    
            
        }

        if (tileGrainCounter == tileCount)
        {
            if (quest.questStarted)
            {
                quest.questDone = true;
            }
        }
    }

    public void VerifyHamsterTriggerPosition(Quest quest)
    {
        Vector2 exitPos = Vector2.zero;

        foreach (StageInfo stageInfo in quest.stageInfos)
        {
            if (!stageInfo.condition.findExit || !stageInfo.isActive) return;
            else exitPos = stageInfo.condition.exitTransform.position;
        }
        

        

        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (Vector2.Equals(hamster.GetHamsterPosition(), exitPos))
            {
                quest.questDone = true;
                return;
            }
        }
    }

    public void LimitedEndurance(Quest quest)
    {
        Vector2 exitPos = Vector2.zero;

        foreach (StageInfo stageInfo in quest.stageInfos)
        {
            if ((!stageInfo.condition.hasLimitedEndurance && !stageInfo.condition.needToFindExit) ||
            quest.questFailed || !stageInfo.isActive) return;
            else exitPos = stageInfo.condition.exitTransform.position;
        }

        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (Vector2.Equals(hamster.GetHamsterPosition(), exitPos))
            {
                quest.questDone = true;
                return;
            }
            if (hamster.EndurancePointsFull == 0)
            {
                quest.questFailed = true;
                Debug.Log("Aufgabe konnte nicht abgeschlossen werden.");
                return;
            }
        }
    }
}