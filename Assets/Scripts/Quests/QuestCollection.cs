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
            if (quest.questStarted)
            {
                foreach (StageInfo stageInfo in quest.stageInfos)
                {
                    if (stageInfo.isActive)
                    {
                        stageInfo.condition.OnAddGrain();
                        stageInfo.condition.OnRemoveGrain();
                    }
                }
            }
        }
    }
}
