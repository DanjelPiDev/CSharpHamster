/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    private static Hamster playerHamster;
    private static Hamster npcTalkingHamster;

    private void SetHamster()
    {
        foreach (Hamster hamster in Territory.GetInstance().GetHamsters())
        {
            if (!hamster.IsNPC && hamster.IsTalking)
            {
                playerHamster = hamster;
            }
            else if (hamster.IsNPC && hamster.IsTalking)
            {
                npcTalkingHamster = hamster;
            }
        }
    }

    public void AddGrain(int amount)
    {
        SetHamster();
        playerHamster.SetGrainCount(playerHamster.GetGrainCount() + amount);
    }

    public void RemoveGrain(int amount)
    {
        SetHamster();
        playerHamster.SetGrainCount(playerHamster.GetGrainCount() - amount);
    }

    public void StartQuest(Quest quest)
    {
        
    }

    public void SetQuestStage(Quest quest)
    {

    }

    public void CompleteQuest(Quest quest)
    {

    }

    public void AddItem(Item item)
    {
        SetHamster();
        if (playerHamster.GetDialogue() != null && playerHamster != null)
        {
            if (playerHamster.GetDialogue().DialogueStarts)
            {
                for (int i = 0; i < playerHamster.GetDialogue().onDialogueStartCommands.Count; i++)
                {
                    if (playerHamster.GetDialogue().onDialogueStartCommands[i].command == DialogueCommands.Commands.AddItem)
                    {
                        playerHamster.AddItem(item, Int32.Parse(playerHamster.GetDialogue().onDialogueStartCommands[i].commandString));
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerHamster.GetDialogue().onDialogueEndCommands.Count; i++)
                {
                    if (playerHamster.GetDialogue().onDialogueEndCommands[i].command == DialogueCommands.Commands.AddItem)
                    {
                        playerHamster.AddItem(item, Int32.Parse(playerHamster.GetDialogue().onDialogueEndCommands[i].commandString));
                    }
                }
            }
            
        }
    }

    public void RemoveItem(Item item)
    {
        SetHamster();
        if (playerHamster != null)
        {
            if (npcTalkingHamster.GetDialogue().DialogueStarts)
            {
                for (int i = 0; i < playerHamster.GetDialogue().onDialogueStartCommands.Count; i++)
                {
                    if (npcTalkingHamster.GetDialogue().onDialogueStartCommands[i].command == DialogueCommands.Commands.RemoveItem)
                    {
                        playerHamster.RemoveItem(item, Int32.Parse(playerHamster.GetDialogue().onDialogueStartCommands[i].commandString));
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerHamster.GetDialogue().onDialogueEndCommands.Count; i++)
                {
                    if (playerHamster.GetDialogue().onDialogueEndCommands[i].command == DialogueCommands.Commands.RemoveItem)
                    {
                        playerHamster.RemoveItem(item, Int32.Parse(playerHamster.GetDialogue().onDialogueEndCommands[i].commandString));
                    }
                }
            }
        }
    }
}
