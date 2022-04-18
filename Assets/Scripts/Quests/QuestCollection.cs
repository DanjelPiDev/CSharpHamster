using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCollection : MonoBehaviour
{
    public List<Quest> quests = new List<Quest>();

    private void Start()
    {
        
    }

    private void Update()
    {
        foreach(Quest quest in quests)
        {
            quest.stageInfo.condition.OnAddGrain();
            quest.stageInfo.condition.OnRemoveGrain();
        }
    }
}
