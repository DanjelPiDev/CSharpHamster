using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aufgabe1 : MonoBehaviour
{
    #region Variablen defintion
    public ItemCollection itemCollection;
    private WaitForSeconds wait;
    private float gameSpeed = 1;
    #endregion

    /* Dein Code startet hier ... */
    Hamster balu;
    /* Dein Code endet hier ... */

    private void HamsterEigenschaften()
    {
        /* Dein Code startet hier ... */
        balu = new Hamster();
        balu.SetName("Balu");
        balu.SetPosition(-5, -3);
        balu.SetPlayerControls(true);
        balu.SetHealthPoints(3);
        balu.HealHamster(2);

        balu.DisplayName(true);
        balu.DisplayHealth(true);

        balu.SetGodMode(true);
        balu.Save();
        balu.SetGrainCount(100);
        /* Dein Code endet hier ... */
    }

    private IEnumerator HamsterBewegung()
    {
        #region Variablen defintion
        wait = new WaitForSeconds(HamsterGameManager.hamsterGameSpeed);
        yield return new WaitForSeconds(1.5f);

        HamsterEigenschaften();
        Territory.GetInstance().UpdateHamsterPosition();
        yield return wait;
        #endregion

        /* Dein Code startet hier ... */
        
        /* Dein Code endet hier ... */
    }


    #region Methoden
    private void Start()
    {
        gameSpeed = HamsterGameManager.hamsterGameSpeed;
        base.StartCoroutine(HamsterBewegung());
    }

    private void Update()
    {
        // Refresh the gamespeed if changed during runtime
        if (gameSpeed != HamsterGameManager.hamsterGameSpeed)
        {
            gameSpeed = HamsterGameManager.hamsterGameSpeed;
            wait = new WaitForSeconds(gameSpeed);
        }
    }
    #endregion
}
