/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using UnityEngine;

public class ItemEvents : MonoBehaviour
{
    private static HamsterGameManager hamsterGameManager;

    private void Awake()
    {
        hamsterGameManager = GameObject.FindGameObjectWithTag("HamsterGameManager").GetComponent<HamsterGameManager>();
    }

    public void Heal(Item item)
    {
        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsUsingItem)
            {
                hamster.HealHamster(item.HealValue);
            }
        }
    }

    public void HealEndurance(Item item)
    {
        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsUsingItem)
            {
                hamster.HealEndurance(item.EnduranceHealValue);
            }
        }
    }

    public void Damage(Item item)
    {
        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsUsingItem)
            {
                hamster.Damage(item.DamageValue);
            }
        }
    }

    public void EnhanceDamage(Item item)
    {
        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsInInventory)
            {
                foreach (ItemSlot slot in hamster.Inventory)
                {
                    if (slot.item.Id == item.Id)
                    {
                        hamster.AttackPower += item.AttackPower;
                        hamster.EffectsActiv = true;
                        Territory.GetInstance().UpdateHamsterProperties(hamster);
                    }
                }
            }
        }
    }

    public void FasterMove(Item item)
    {
        foreach(Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsInInventory)
            {
                foreach (ItemSlot slot in hamster.Inventory)
                {
                    if (slot.item.Id == item.Id)
                    {
                        hamster.MoveSpeed += item.MoveSpeed;
                        hamster.EffectsActiv = true;
                        Territory.GetInstance().UpdateHamsterProperties(hamster);
                    }
                }
            }
        }
    }

    public void ResetEffect(Item item)
    {
        foreach (Hamster hamster in Territory.activHamsters)
        {
            if (hamster.IsInInventory)
            {
                foreach (ItemSlot slot in hamster.Inventory)
                {
                    if (slot.item.Id == item.Id &&
                        slot.item.hasSpecialEffects)
                    {
                        if (slot.item.MoveSpeed > 0)
                            hamster.MoveSpeed -= slot.item.MoveSpeed;
                        if (slot.item.AttackPower > 0)
                            hamster.AttackPower -= slot.item.AttackPower;
                        hamster.EffectsActiv = true;
                        Territory.GetInstance().UpdateHamsterProperties(hamster);
                    }
                }
            }
        }
    }
}
