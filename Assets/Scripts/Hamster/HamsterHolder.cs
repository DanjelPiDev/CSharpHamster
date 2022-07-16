/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HamsterHolder : MonoBehaviour
{
    public Hamster hamster { get; set; }

    private void Start()
    {
        this.hamster.HamsterObject = this.gameObject;
    }

    public IEnumerator DisplaySpeechBubble(float time)
    {
        yield return new WaitForSeconds(time);
        this.transform.parent.GetChild(7).GetComponent<TextMeshPro>().SetText("");
        this.transform.parent.GetChild(6).gameObject.SetActive(false);
        this.transform.parent.GetChild(7).gameObject.SetActive(false);

    }

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
