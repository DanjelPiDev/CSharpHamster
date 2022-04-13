using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
}
