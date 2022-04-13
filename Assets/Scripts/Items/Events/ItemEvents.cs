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
                hamster.DamageHamster(item.DamageValue);
            }
        }
    }

    public void FasterMove(Item item)
    {
        foreach(Hamster hamster in Territory.activHamsters)
        {
            foreach (ItemSlot slot in hamster.Inventory)
            {
                if (slot.item.Id == item.Id)
                {
                    hamster.MoveSpeed = item.MoveSpeed;
                    Territory.GetInstance().UpdateHamsterProperties(hamster);
                }
            }
        }
    }
}
