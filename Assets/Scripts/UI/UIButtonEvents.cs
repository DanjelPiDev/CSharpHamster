using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonEvents : MonoBehaviour
{
    public void StopTrading()
    {
        HamsterGameManager.hamster1.IsTrading = false;
        HamsterGameManager.hamster1.DisplayTradeWindow(HamsterGameManager.hamster1, HamsterGameManager.hamster2);
    }
}
