using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloud : MonoBehaviour
{

    public float hieght;
    public GameObject Cloud;
    private float randomZ;
    private Vector3 CloudSpawn;
    public float spawnDellay;
    float nextSpawn;

    public void Dellay(float value)
    {
        spawnDellay=value;
    }
    public void Hieght(float value)
    {
        hieght =value;
    }


    private float spawnNight;
    private float spawnDay;

    // Start is called before the first frame update
    public void Start()
    {
        spawnNight = (spawnDellay/1.5f)/5;
        spawnDay = (spawnDellay*3)/5;

    }

    // Update is called once per frame
    void Update()
    {
        if (SunPosition.timeProgress >0.6)
        {
            spawnDellay = spawnNight;
        }
        else
        {
            spawnDellay = spawnDay;
        }

        if (Time.time > nextSpawn)
        {
            nextSpawn= Time.time+spawnDellay;
            randomZ = Random.Range(-0.75f, 0.75f);
            CloudSpawn = new Vector3(0,0, randomZ);
            GameObject cloud = Instantiate(Cloud,transform.position + CloudSpawn, Quaternion.identity);
            cloud.transform.parent = transform;
            cloud.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Destroy(cloud,4f);
        }


    }
}
