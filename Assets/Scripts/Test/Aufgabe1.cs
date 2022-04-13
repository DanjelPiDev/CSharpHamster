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
    /* Dein Code endet hier ... */

    private void HamsterEigenschaften()
    {
        /* Dein Code startet hier ... */
        meinHamster = new Hamster();
        meinHamster.SetName("Hamtaro");
        meinHamster.SetPosition(-5, -3);
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
