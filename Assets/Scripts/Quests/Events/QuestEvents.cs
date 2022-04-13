using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvents : MonoBehaviour
{
    public void CheckGlobalGrainCount(Quest quest)
    {
        if (Territory.globalGrainCount == 0 && quest.stageInfo.condition.pickUpAllGrains)
        {
            quest.questDone = true;
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
        if (!quest.stageInfo.condition.needSpecificAmountOfGrains) return;

        int tileCount = 0;
        int tileGrainCounter = 0;

        for (int i = 0; i < Territory.tileCollection.childCount; i++)
        {
            if (Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.type != Tile.TileType.Wall)
            {
                tileCount += 1;
            }

            if (Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.type != Tile.TileType.Wall &&
                Territory.tileCollection.GetChild(i).GetComponent<TileHolder>().tile.grainCount == quest.stageInfo.condition.specificAmountOfGrains)
            {
                tileGrainCounter += 1;
            }
        }

        if (tileGrainCounter == tileCount)
        {
            quest.questDone = true;
        }
    }

    public void VerifyHamsterTriggerPosition(Quest quest)
    {
        if (!quest.stageInfo.condition.findExit) return;

        Vector2 exitPos = quest.stageInfo.condition.exitTransform.position;

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
        if ((!quest.stageInfo.condition.hasLimitedEndurance && !quest.stageInfo.condition.needToFindExit) ||
            quest.questFailed) return;

        Vector2 exitPos = quest.stageInfo.condition.exitTransform.position;

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