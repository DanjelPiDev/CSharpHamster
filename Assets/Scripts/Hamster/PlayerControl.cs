using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
#if UNITY_EDITOR
    [Help("Falls 2 Spieler mithilfe einer Tastatur spielen wollen (Offline)!\nTaste 1 für Spieler 1 und Taste 2 für Spieler 2\n\nWenn Du eine doppelbelegung willst, musst Du hier weitere Tasten Einstellen. Füge ein weiteres Element dem Array mit dem '+' Zeichen hinzu und anschließend die Taste.(Funktioniert nicht mit multiplePlayers!)", UnityEditor.MessageType.Info)]
#endif
    [Header("Player Options")]
    public bool multiplayer = false;
    /* 
     * Wenn Du eine Doppelbelegung willst, musst Du eine weitere Taste im Inspector Einstellen.
     * Nicht hier im Script! Da das keinen Effekt hätte. Füge ein weiteres Element dem array hinzu und anschließend die Taste.
     */
    [Header("Keybindings")]
    public KeyCode[] moveRight = new KeyCode[] { KeyCode.RightArrow };
    public KeyCode[] moveLeft = new KeyCode[] { KeyCode.LeftArrow };
    public KeyCode[] moveUp = new KeyCode[] { KeyCode.UpArrow };
    public KeyCode[] moveDown = new KeyCode[] { KeyCode.DownArrow };
    public KeyCode[] pickUpGrain = new KeyCode[] { KeyCode.Space };
    public KeyCode[] pickUpItem = new KeyCode[] { KeyCode.F };
    public KeyCode[] dropGrain = new KeyCode[] { KeyCode.D };
    public KeyCode[] talk = new KeyCode[] { KeyCode.E };
    public KeyCode[] trade = new KeyCode[] { KeyCode.T };
    public KeyCode[] inventory = new KeyCode[] { KeyCode.I };
    public KeyCode[] controlEffects = new KeyCode[] { KeyCode.Z };

    private Hamster hamster;
    private Hamster hamster2;

    private void Start()
    {
        base.StartCoroutine(SetPlayerHamster());
    }

    private IEnumerator SetPlayerHamster()
    {
        yield return new WaitForSeconds(2);
        Hamster playerHamster = null;
        Hamster playerHamster2 = null;

        // Für Spieler 1
        foreach(Hamster ham in Territory.activHamsters)
        {
            if(ham.PlayerControl)
            {
                hamster = ham;
                playerHamster = ham;
                Territory.GetInstance().UpdateHamsterProperties(ham);
                break;
            }
        }

        // Für Spieler 2
        if (multiplayer)
        {
            foreach (Hamster ham in Territory.activHamsters)
            {
                if (ham != playerHamster && ham.PlayerControl)
                {
                    hamster2 = ham;
                    playerHamster2 = ham;
                    Territory.GetInstance().UpdateHamsterProperties(ham);
                    break;
                }
            }
        }

        if (hamster2 == null)
        {
            multiplayer = false;
        }
        

        foreach (Hamster ham in Territory.activHamsters)
        {
            if (ham != playerHamster || ham != playerHamster2)
            {
                ham.PlayerControl = false;
                Territory.GetInstance().UpdateHamsterProperties(ham);
            }
        }
    }

    


    private void ManagePlayerMovement()
    {
        if (hamster == null) return;

        /*
        * Für den Fall das 2 auf einer Tastatur spielen
        */
        if (multiplayer)
        {
            if (Input.GetKeyDown(moveRight[0]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster.Direction)
                {
                    case Hamster.LookingDirection.North:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        hamster.TurnLeft();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        hamster.Move();
                        Territory.playerCanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveRight[1]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster2.Direction)
                {
                    case Hamster.LookingDirection.North:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        hamster2.TurnLeft();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        hamster2.Move();
                        Territory.player2CanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveLeft[0]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster.Direction)
                {
                    case Hamster.LookingDirection.North:
                        hamster.TurnLeft();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        hamster.Move();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveLeft[1]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster2.Direction)
                {
                    case Hamster.LookingDirection.North:
                        hamster2.TurnLeft();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        hamster2.Move();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveUp[0]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster.Direction)
                {
                    case Hamster.LookingDirection.North:
                        hamster.Move();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        hamster.TurnLeft();
                        Territory.playerCanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveUp[1]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster2.Direction)
                {
                    case Hamster.LookingDirection.North:
                        hamster2.Move();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        hamster2.TurnLeft();
                        Territory.player2CanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveDown[0]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster.Direction)
                {
                    case Hamster.LookingDirection.North:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        hamster.Move();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        hamster.TurnLeft();
                        Territory.playerCanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster.TurnLeft();
                        }
                        Territory.playerCanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(moveDown[1]))
            {
                /* 
                 * Check here in which direction the hamster is looking at, 
                 * if hamster is not looking to east, then the hamster will first turn to this side 
                 */
                switch (hamster2.Direction)
                {
                    case Hamster.LookingDirection.North:
                        for (int i = 0; i < 2; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.South:
                        hamster2.Move();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.West:
                        hamster2.TurnLeft();
                        Territory.player2CanMove = false;
                        break;
                    case Hamster.LookingDirection.East:
                        for (int i = 0; i < 3; i++)
                        {
                            hamster2.TurnLeft();
                        }
                        Territory.player2CanMove = false;
                        break;
                }
            }
            else if (Input.GetKeyDown(pickUpGrain[0]))
            {
                hamster.PickUpGrain();
            }
            else if (Input.GetKeyDown(pickUpGrain[1]))
            {
                hamster2.PickUpGrain();
            }
            else if (Input.GetKeyDown(dropGrain[0]))
            {
                hamster.DropGrain();
            }
            else if (Input.GetKeyDown(dropGrain[1]))
            {
                hamster2.DropGrain();
            }
            else if (Input.GetKeyDown(talk[0]))
            {
                hamster.Talk();
            }
            else if (Input.GetKeyDown(talk[1]))
            {
                hamster2.Talk();
            }
            else if (Input.GetKeyDown(trade[0]))
            {
                hamster.Trade();
            }
            else if (Input.GetKeyDown(trade[1]))
            {
                hamster2.Trade();
            }
            else if (Input.GetKeyDown(inventory[0]))
            {
                hamster.DisplayInventory();
            }
            else if (Input.GetKeyDown(inventory[1]))
            {
                hamster2.DisplayInventory();
            }
            else if (Input.GetKeyDown(controlEffects[0]))
            {
                hamster.SetEquipmentEffects(!hamster.EffectsActiv);
            }
            else if (Input.GetKeyDown(controlEffects[1]))
            {
                hamster2.SetEquipmentEffects(!hamster.EffectsActiv);
            }
            else if (Input.GetKeyDown(pickUpItem[0]))
            {
                hamster.PickUpItem();
            }
            else if (Input.GetKeyDown(pickUpItem[1]))
            {
                hamster2.PickUpItem();
            }

            return;
        }

        if (Territory.playerCanMove)
        {
            

            /*
             * Für den Fall der Doppelbelegung
             * Nicht kompatibel mit mehrspieler!
             */


            foreach (KeyCode keycode in moveRight)
            {
                if (Input.GetKeyDown(keycode))
                {
                    /* 
                     * Check here in which direction the hamster is looking at, 
                     * if hamster is not looking to east, then the hamster will first turn to this side 
                     */
                    switch (hamster.Direction)
                    {
                        case Hamster.LookingDirection.North:
                            for (int i = 0; i < 3; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.South:
                            hamster.TurnLeft();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.West:
                            for (int i = 0; i < 2; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.East:
                            hamster.Move();
                            Territory.playerCanMove = false;
                            return;
                    }
                }
            }

            foreach (KeyCode keycode in moveLeft)
            {
                if (Input.GetKeyDown(keycode))
                {
                    /* 
                     * Check here in which direction the hamster is looking at, 
                     * if hamster is not looking to east, then the hamster will first turn to this side 
                     */
                    switch (hamster.Direction)
                    {
                        case Hamster.LookingDirection.North:
                            hamster.TurnLeft();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.South:
                            for (int i = 0; i < 3; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.West:
                            hamster.Move();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.East:
                            for (int i = 0; i < 2; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                    }
                    break;
                }
            }

            foreach (KeyCode keycode in moveUp)
            {
                if (Input.GetKeyDown(keycode))
                {
                    /* 
                     * Check here in which direction the hamster is looking at, 
                     * if hamster is not looking to east, then the hamster will first turn to this side 
                     */
                    switch (hamster.Direction)
                    {
                        case Hamster.LookingDirection.North:
                            hamster.Move();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.South:
                            for (int i = 0; i < 2; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.West:
                            for (int i = 0; i < 3; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.East:
                            hamster.TurnLeft();
                            Territory.playerCanMove = false;
                            return;
                    }
                    break;
                }
            }

            foreach (KeyCode keycode in moveDown)
            {
                if (Input.GetKeyDown(keycode))
                {
                    /* 
                     * Check here in which direction the hamster is looking at, 
                     * if hamster is not looking to east, then the hamster will first turn to this side 
                     */
                    switch (hamster.Direction)
                    {
                        case Hamster.LookingDirection.North:
                            for (int i = 0; i < 2; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.South:
                            hamster.Move();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.West:
                            hamster.TurnLeft();
                            Territory.playerCanMove = false;
                            return;
                        case Hamster.LookingDirection.East:
                            for (int i = 0; i < 3; i++)
                            {
                                hamster.TurnLeft();
                            }
                            Territory.playerCanMove = false;
                            return;
                    }
                }
            }

            foreach (KeyCode keycode in pickUpGrain)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.PickUpGrain();
                    return;
                }
            }

            foreach (KeyCode keycode in dropGrain)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.DropGrain();
                    return;
                }
            }

            foreach (KeyCode keycode in talk)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.Talk();
                    return;
                }
            }

            foreach (KeyCode keycode in trade)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.Trade();
                    return;
                }
            }

            foreach (KeyCode keycode in inventory)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.DisplayInventory();
                    return;
                }
            }

            foreach (KeyCode keycode in controlEffects)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.SetEquipmentEffects(!hamster.EffectsActiv);
                    return;
                }
            }

            foreach (KeyCode keycode in pickUpItem)
            {
                if (Input.GetKeyDown(keycode))
                {
                    hamster.PickUpItem();
                    return;
                }
            }


            


        }
    }

    private void Update()
    {
        ManagePlayerMovement();
    }
}
