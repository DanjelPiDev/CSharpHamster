using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public GameObject[] clouds;

    private void Start()
    {
        CloudController();
        base.StartCoroutine(SpawnClouds());
    }

    private void CloudController()
    {
        float xBound = 0.5f * this.transform.GetComponent<BoxCollider2D>().size.x;
        // Spawn clouds far right (Clouds moving from right to left)
        int rndAmount = Random.Range(1, clouds.Length);

        for (int i = 0; i < rndAmount; i++)
        {
            float rndY = Random.Range(this.transform.GetComponent<BoxCollider2D>().bounds.min.y, this.transform.GetComponent<BoxCollider2D>().bounds.max.y);
            
            int rndCloud = Random.Range(0, clouds.Length);
            GameObject cloud = Instantiate(clouds[rndCloud]);

            cloud.transform.parent = this.transform;
            cloud.transform.position = new Vector3(xBound, rndY, Camera.main.transform.position.z - 10);
            cloud.GetComponent<Rigidbody2D>().AddForce((new Vector2(-5f, 0)) * 50f);
        }
    }

    private IEnumerator SpawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            CloudController();
            yield return null;
        }
    }
}