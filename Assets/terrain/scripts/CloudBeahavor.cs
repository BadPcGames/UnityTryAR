using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudBeahavor : MonoBehaviour
{
    public GameObject cloudSpherePrefab; // Префаб для сферы облака
    [SerializeField]
    private float speed = 0.15f;

    private bool called = false;
    void Start()
    {
        for (int i = 0; i <= Random.RandomRange(3,5); i++)
        {
            float scale = Random.RandomRange(0.02f, 0.12f);
            Vector3 randomPosition = new Vector3(transform.position.x-scale-Random.RandomRange(-0.02f,0.02f),transform.position.y + Random.RandomRange(-0.02f, 0.03f), transform.position.z+ Random.RandomRange(-0.03f, 0.04f));
            
            cloudSpherePrefab.transform.localScale = new Vector3(scale,scale,scale);
            GameObject cloudSphere = Instantiate(cloudSpherePrefab, randomPosition, Quaternion.identity);
            cloudSphere.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!called)
        {
            StartCoroutine(CallMethodInSeconds());
            called = true;
        }
    }

    IEnumerator CallMethodInSeconds()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        called = false;
    }
  
}
