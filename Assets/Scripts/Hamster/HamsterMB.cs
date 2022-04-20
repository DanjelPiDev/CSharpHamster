using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterMB : MonoBehaviour
{
    public Hamster hamster;

    private HamsterGameManager hamsterGameManager;

    private void Start()
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        base.StartCoroutine(ReadInAllHamsterInformation());
    }


    private IEnumerator ReadInAllHamsterInformation()
    {
        yield return new WaitForSeconds(1.5f);

        int idCounter = hamsterGameManager.hamsterCollection.childCount;

        for (int i = 0; i < hamsterGameManager.npcHamsterCollection.childCount; i++)
        {
            Hamster hamster = hamsterGameManager.npcHamsterCollection.GetChild(i).GetComponent<HamsterMB>().hamster;

            if (hamster.IsNPC)
            {
                if (hamster.IsDisplayingHealth)
                {
                    hamsterGameManager.npcHamsterCollection.GetChild(i).GetChild(0).gameObject.SetActive(true);
                }

                /* Set all information */
                hamsterGameManager.npcHamsterCollection.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
                hamster.HamsterObject = hamsterGameManager.npcHamsterCollection.GetChild(i).gameObject;

                hamster.Column = (int)Math.Round((hamsterGameManager.npcHamsterCollection.GetChild(i).position.x / 2.56f), MidpointRounding.ToEven);
                hamster.Row = (int)Math.Round((hamsterGameManager.npcHamsterCollection.GetChild(i).position.y / 2.56f), MidpointRounding.ToEven);

                hamster.Id = idCounter;

                if (hamster.Pattern == Hamster.MovementPattern.LeftRight &&
                    (hamster.Direction != Hamster.LookingDirection.West || hamster.Direction != Hamster.LookingDirection.East))
                {
                    hamster.SetLookingDirection(Hamster.LookingDirection.West);
                }
                else if (hamster.Pattern == Hamster.MovementPattern.UpDown &&
                    (hamster.Direction != Hamster.LookingDirection.North || hamster.Direction != Hamster.LookingDirection.South))
                {
                    hamster.SetLookingDirection(Hamster.LookingDirection.North);
                }


                
                Territory.GetInstance().AddHamster(hamster);
                Territory.GetInstance().UpdateHamsterProperties(hamster, createHealthUI: true, createNameUI: true, createEnduranceUI: true);
            }
            idCounter += 1;
        }
        
    }

    private void Update()
    {
        if (hamsterGameManager.saveEachFrame && hamsterGameManager.saveNPCs)
        {
            //hamster.Save();
        }
    }
}
