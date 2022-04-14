using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aufgabe1 : MonoBehaviour
{
    #region Variablen defintion
    public ItemCollection itemCollection;
    private WaitForSeconds wait;
    #endregion

    /* Dein Code startet hier ... */
    Hamster meinHamster;
    Hamster meinHamster2;
    /* Dein Code endet hier ... */

    private void HamsterEigenschaften()
    {
        /* Dein Code startet hier ... */
        meinHamster = new Hamster();
        meinHamster.SetName("Hamtaro");
        meinHamster.SetPosition(-5, -3);
        

        meinHamster2 = new Hamster();
        meinHamster2.SetName("Loou");
        meinHamster2.SetPosition(-5, -2);
        meinHamster2.SetColor(Hamster.HamsterColor.Blue);

        meinHamster.SetPlayerControls(true);
        meinHamster2.SetPlayerControls(true);
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
        base.StartCoroutine(HamsterBewegung());
    }

    private void Update()
    {
        // Aktualisiere die gameSpeed
        wait = new WaitForSeconds(HamsterGameManager.hamsterGameSpeed);
    }
    #endregion
}
