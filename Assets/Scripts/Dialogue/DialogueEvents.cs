using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    private Hamster playerHamster;
    private Hamster npcTalkingHamster;

    public void SetHamster()
    {
        foreach (Hamster hamster in Territory.GetInstance().GetHamsters())
        {
            if (!hamster.IsNPC && hamster.IsTalking)
            {
                this.playerHamster = hamster;
            }
            else if (hamster.IsNPC && hamster.IsTalking)
            {
                this.npcTalkingHamster = hamster;
            }
        }
    }

    public void AddGrain(int amount)
    {
        this.playerHamster.SetGrainCount(this.playerHamster.GetGrainCount() + amount);
    }

    public void RemoveGrain(int amount)
    {
        this.playerHamster.SetGrainCount(this.playerHamster.GetGrainCount() - amount);
    }

    public void AddItem(Item item)
    {
        if (this.playerHamster.GetDialogue() != null && this.playerHamster != null)
        {
            if (this.playerHamster.GetDialogue().DialogueStarts)
            {
                Debug.Log("Here 2");
                for (int i = 0; i < this.playerHamster.GetDialogue().onDialogueStartCommands.Count; i++)
                {
                    if (this.playerHamster.GetDialogue().onDialogueStartCommands[i].command == DialogueCommands.Commands.AddItem)
                    {
                        this.playerHamster.AddItem(item, Int32.Parse(this.playerHamster.GetDialogue().onDialogueStartCommands[i].commandString));
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.playerHamster.GetDialogue().onDialogueEndCommands.Count; i++)
                {
                    if (this.playerHamster.GetDialogue().onDialogueEndCommands[i].command == DialogueCommands.Commands.AddItem)
                    {
                        this.playerHamster.AddItem(item, Int32.Parse(this.playerHamster.GetDialogue().onDialogueEndCommands[i].commandString));
                    }
                }
            }
            
        }
    }

    public void RemoveItem(Item item)
    {
        if (this.playerHamster != null)
        {
            if (this.npcTalkingHamster.GetDialogue().DialogueStarts)
            {
                for (int i = 0; i < this.playerHamster.GetDialogue().onDialogueStartCommands.Count; i++)
                {
                    if (this.npcTalkingHamster.GetDialogue().onDialogueStartCommands[i].command == DialogueCommands.Commands.RemoveItem)
                    {
                        this.playerHamster.RemoveItem(item, Int32.Parse(this.playerHamster.GetDialogue().onDialogueStartCommands[i].commandString));
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.playerHamster.GetDialogue().onDialogueEndCommands.Count; i++)
                {
                    if (this.playerHamster.GetDialogue().onDialogueEndCommands[i].command == DialogueCommands.Commands.RemoveItem)
                    {
                        this.playerHamster.RemoveItem(item, Int32.Parse(this.playerHamster.GetDialogue().onDialogueEndCommands[i].commandString));
                    }
                }
            }
        }
    }
}
