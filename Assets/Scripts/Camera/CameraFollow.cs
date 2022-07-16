/************************************************
 * Created by:  Danjel Galic
 * 
 * Modified by: -
 ************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            this.gameObject.transform.position = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        }
    }
}
