using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterMB : MonoBehaviour
{
    public Hamster hamster;

    private HamsterGameManager hamsterGameManager;
    private WaitForSecondsRealtime wait;

    private void Start()
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();

        this.hamster = Instantiate(hamster);

        wait = new WaitForSecondsRealtime(this.hamster.AttackSpeedDelay);
        base.StartCoroutine(ReadInAllHamsterInformation());
        base.StartCoroutine(HitTargetRegistry());
    }

    private void Guard(int aggroRadius = 1)
    {
        if (!this.hamster.IsEvil) return;

        foreach (Hamster ham in Territory.activHamsters)
        {
            if (Vector2.Equals(new Vector2(this.hamster.GetHamsterPosition().x + aggroRadius, this.hamster.GetHamsterPosition().y), ham.GetHamsterPosition()))
            {
                this.hamster.SetLookingDirection(Hamster.LookingDirection.East);
                this.hamster.Hit();
            }
            else if (Vector2.Equals(new Vector2(this.hamster.GetHamsterPosition().x - aggroRadius, this.hamster.GetHamsterPosition().y), ham.GetHamsterPosition()))
            {
                this.hamster.SetLookingDirection(Hamster.LookingDirection.West);
                this.hamster.Hit();
            }
            else if (Vector2.Equals(new Vector2(this.hamster.GetHamsterPosition().x, this.hamster.GetHamsterPosition().y + aggroRadius), ham.GetHamsterPosition()))
            {
                this.hamster.SetLookingDirection(Hamster.LookingDirection.North);
                this.hamster.Hit();
            }
            else if (Vector2.Equals(new Vector2(this.hamster.GetHamsterPosition().x, this.hamster.GetHamsterPosition().y - aggroRadius), ham.GetHamsterPosition()))
            {
                this.hamster.SetLookingDirection(Hamster.LookingDirection.South);
                this.hamster.Hit();
            }
        }
    }

    private IEnumerator HitTargetRegistry()
    {
        while (true)
        {
            yield return wait;
            Guard(this.hamster.AggroRadius);
        }
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

                /*
                 * Look if the npc hamster should load the data from the file
                 * instead of loading it from the gameObject position 
                 */
                if (!hamsterGameManager.loadStoredInfo)
                {
                    hamster.Column = (int)Math.Round((hamsterGameManager.npcHamsterCollection.GetChild(i).position.x / 2.56f), MidpointRounding.ToEven);
                    hamster.Row = (int)Math.Round((hamsterGameManager.npcHamsterCollection.GetChild(i).position.y / 2.56f), MidpointRounding.ToEven);
                }

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
}
