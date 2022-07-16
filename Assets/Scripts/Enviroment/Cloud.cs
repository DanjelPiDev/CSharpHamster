using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    void Update()
    {
        if (this.transform.position.x <= -(0.5f * this.transform.parent.GetComponent<BoxCollider2D>().size.x))
        {
            Destroy(this.gameObject);
        }
    }
}
