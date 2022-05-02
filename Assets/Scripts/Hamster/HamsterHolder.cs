using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterHolder : MonoBehaviour
{
    public Hamster hamster;

    private IEnumerator StartBlinkEffect()
    {
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.enabled = false;
        yield return new WaitForSeconds(0.05f);
        renderer.enabled = true;
    }

    private void Update()
    {
        if (hamster.TookDamage)
        {
            hamster.TookDamage = false;
            StartCoroutine(StartBlinkEffect());
        }
    }
}
