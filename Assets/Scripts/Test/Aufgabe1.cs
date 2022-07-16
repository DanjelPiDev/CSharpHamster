/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aufgabe1 : MonoBehaviour
{
    #region Variablen definition
    public ItemCollection itemCollection;
    private WaitForSeconds wait;
    private float gameSpeed = 1;
    #endregion

    /* Dein Code startet hier ... */
    Hamster test;
    /* Dein Code endet hier ... */

    private void HamsterEigenschaften()
    {
        /* Dein Code startet hier ... */
        test = new Hamster();
        test.SetPosition(-5, -3);
        test.SetPlayerControls(true);
        /* Dein Code endet hier ... */
    }

    private IEnumerator HamsterBewegung()
    {
        #region Variablen definition
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
