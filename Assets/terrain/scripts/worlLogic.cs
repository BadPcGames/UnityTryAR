using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class worlLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject chunk;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private int range=1;
    [SerializeField]
    private int mainSeed=2;
    [SerializeField]
    private bool manualSeed;
    [SerializeField]
    private GameObject treeSpawner;

    private Vector2 lastposition;

    private IEnumerator coroutine;
    private int x;
    private int y;

    public void setSeed(string value)
    {
        mainSeed = int.Parse(value);
    }

    public void setIsManualSeed(bool value)
    {
        manualSeed = value;
    }


    private Vector2Int camChunk=new Vector2Int();
    private Dictionary<Vector2Int,int> chunkIsCreate=new Dictionary<Vector2Int,int>();
    private float size=0.45f;
    private List<GameObject> trees;

    void Start()
    {
        trees = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject tree = treeSpawner;
            tree.tag = "tree";
            trees.Add(tree);
        }

        lastposition = new Vector2(camera.transform.position.x, camera.transform.position.z);
        if (!manualSeed)
        {
            mainSeed = System.DateTime.Now.Millisecond;
        }
        Random.seed = mainSeed;

        camChunk = new Vector2Int(0, 0);
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                int chunkSeed = Random.Range(0, int.MaxValue);
                chunkIsCreate.Add(new Vector2Int(i, j), chunkSeed);
                chunk.GetComponent<MashGenerator>().setSeed(chunkSeed);
                chunk.GetComponent<MashGenerator>().makeChumk();
                GameObject somechunk=Instantiate(chunk, new Vector3(), new Quaternion());
                somechunk.transform.localPosition = new Vector3(i * size - size / 2, 0, j * size - size / 2);
                somechunk.transform.SetParent(transform);
            }
        }
        water.transform.localScale = new Vector3(0.15f * range, 1, 0.15f * range);
        GameObject waterPlane=Instantiate(water, new Vector3(), new Quaternion());
        waterPlane.transform.localPosition = new Vector3(camChunk.x * size , 0, camChunk.y * size);
        waterPlane.transform.SetParent(transform);


        //Instantiate(cloud, new Vector3((camChunk.x +2) * size, 0, (camChunk.y+0.5f) * size), new Quaternion());
    }

    //public void clearAndBuild()
    //{
    //    if (!manualSeed)
    //    {
    //        mainSeed = System.DateTime.Now.Millisecond;
    //    }
    //    Random.seed = mainSeed;
    //    chunkIsCreate = new Dictionary<Vector2Int, int>();

    //    camChunk = new Vector2Int(Mathf.FloorToInt(x / size),
    //                                    Mathf.FloorToInt(y / size));

    //    GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Ground");
    //    foreach (GameObject obj in objectsToDelete)
    //    {
    //            Destroy(obj);
    //    }

    //    objectsToDelete = GameObject.FindGameObjectsWithTag("tree");
    //    foreach (GameObject obj in objectsToDelete)
    //    {
    //        Destroy(obj);
    //    }


    //    for (int i = -range + camChunk.x; i < range; i++)
    //    {
    //        for (int j = -range + camChunk.y; j < range; j++)
    //        {

    //                int chunkSeed = Random.Range(0, int.MaxValue);
    //                chunkIsCreate.Add(new Vector2Int(i, j), chunkSeed);
    //                chunk.setSeed(chunkSeed);
    //                chunk.makeChumk();
    //                chunk.mekeTreeForChunk(i * size, j * size, trees);
    //                Instantiate(chunk, new Vector3(i * size, 0, j * size), new Quaternion());

    //        }
    //    }

    //    GameObject watterPlane = GameObject.FindGameObjectWithTag("Water");
    //    if (watterPlane != null)
    //        watterPlane.transform.position = new Vector3(camChunk.x * size + size / 2, 0, camChunk.y * size + size / 2);
    //    GameObject cloudSpawner = GameObject.FindGameObjectWithTag("Cloud");
    //    if (cloudSpawner != null)
    //        cloudSpawner.transform.position = new Vector3((camChunk.x + 2) * size, 0, (camChunk.y + 0.5f) * size);

    //}

    void Update()
    {
        if (camIsChangeChunk())
        {
            camChunk = new Vector2Int(x, y);
            lastposition = new Vector2(camera.transform.position.x, camera.transform.position.z);
            GameObject[] objectsToDelete = GameObject.FindGameObjectsWithTag("Ground");
            foreach (GameObject obj in objectsToDelete)
            {
                Destroy(obj);
            }

            for (int i1 = -range, i = camChunk.x - range; i <= camChunk.x + range; i1++, i++)
            {
                for (int j1 = -range, j = camChunk.y - range; j <= camChunk.y + range; j1++, j++)
                {
                    if (chunkIsCreate.ContainsKey(new Vector2Int(i, j)))
                    {
                        int chunkSeed = chunkIsCreate[new Vector2Int(i, j)];
                        chunk.GetComponent<MashGenerator>().setSeed(chunkSeed);
                    }
                    else
                    {
                        int chunkSeed = Random.Range(0, int.MaxValue);
                        chunkIsCreate.Add(new Vector2Int(i, j), chunkSeed);
                        chunk.GetComponent<MashGenerator>().setSeed(chunkSeed);
                    }
                    chunk.GetComponent<MashGenerator>().makeChumk();
                    GameObject somechunk = Instantiate(chunk, new Vector3(), new Quaternion());
                    somechunk.transform.localPosition = new Vector3(i1 * size - size / 2, 0, j1 * size - size / 2);
                    somechunk.transform.SetParent(transform);
                }
            }


            //        //objectsToDelete = GameObject.FindGameObjectsWithTag("tree");
            //        //foreach (GameObject obj in objectsToDelete)
            //        //{
            //        //    if (
            //        //        (obj.transform.position.x < (camChunk.x - range) * size) || (obj.transform.position.x > (camChunk.x + range) * size + size / 2)
            //        //        || (obj.transform.position.z < (camChunk.y - range) * size) || (obj.transform.position.z > (camChunk.y + range) * size+size/2)
            //        //        )
            //        //        Destroy(obj);
            //        //}

            //        //coroutine = addChunks(changeByX, changeByZ, true);
            //        //StartCoroutine(coroutine);

            //        //GameObject watterPlane = GameObject.FindGameObjectWithTag("Water");
            //        //if (watterPlane != null)
            //        //    watterPlane.transform.position = new Vector3(camChunk.x * size + size/2, 0, camChunk.y * size +size/2);
            //        //GameObject cloudSpawner = GameObject.FindGameObjectWithTag("Cloud");
            //        //if (cloudSpawner != null)
            //        //cloudSpawner.transform.position =new Vector3((camChunk.x + 2) * size, 0, (camChunk.y + 0.5f) * size);

        }
    }


    //private IEnumerator addChunks(int changeByX, int changeByZ, bool wait)
    //{
    //    if (changeByX != 0)
    //    {
    //        int i = camChunk.x + (range * changeByX);
    //        for (int j = camChunk.y - range; j <= camChunk.y + range; j++)
    //        {
    //            if (!chunkIsCreate.ContainsKey(new Vector2Int(i, j)))
    //            {
    //                int chunkSeed = Random.Range(0, int.MaxValue);
    //                chunkIsCreate.Add(new Vector2Int(i, j), chunkSeed);
    //                chunk.setSeed(chunkSeed);
    //                chunk.makeChumk();
    //                chunk.mekeTreeForChunk(i * size, j * size, trees);
    //                Instantiate(chunk, new Vector3(i * size, 0, j * size), new Quaternion());    
    //            }
    //            else
    //            {
    //                chunk.setSeed(chunkIsCreate[new Vector2Int(i, j)]);
    //                chunk.makeChumk();
    //                chunk.mekeTreeForChunk(i * size, j * size, trees);
    //                Instantiate(chunk, new Vector3(i * size, 0, j * size), new Quaternion());
    //            }

    //            if (wait)
    //            {
    //                yield return new WaitForSecondsRealtime(0.01f);
    //            }
    //        }
    //    }
    //    if (changeByZ != 0)
    //    {
    //        int j = camChunk.y + (range * changeByZ);
    //        for (int i = camChunk.x - range; i <= camChunk.x + range; i++)
    //        {

    //            if (!chunkIsCreate.ContainsKey(new Vector2Int(i, j)))
    //            {
    //                int chunkSeed = Random.Range(0, int.MaxValue);
    //                chunkIsCreate.Add(new Vector2Int(i, j), chunkSeed);
    //                chunk.setSeed(chunkSeed);
    //                chunk.makeChumk();
    //                chunk.mekeTreeForChunk(i * size, j * size, trees);
    //                Instantiate(chunk, new Vector3(i * size, 0, j * size), new Quaternion());
    //            }
    //            else
    //            {
    //                chunk.setSeed(chunkIsCreate[new Vector2Int(i, j)]);
    //                chunk.makeChumk();
    //                chunk.mekeTreeForChunk(i * size, j * size, trees);
    //                Instantiate(chunk, new Vector3(i * size, 0, j * size), new Quaternion());
    //            }
    //            if (wait)
    //            {
    //                yield return new WaitForSecondsRealtime(0.01f);
    //            }
    //        }
    //    }
    //}

    private bool camIsChangeChunk()
    {
        if(lastposition.x-camera.transform.position.x>0.2|| lastposition.x - camera.transform.position.x < -0.2||
            lastposition.y - camera.transform.position.z > 0.2 || lastposition.y - camera.transform.position.z < -0.2)
        {
            x = (int)(camera.transform.position.x * 2);
            y = (int)(camera.transform.position.z * 2);
            return camChunk != new Vector2Int(x, y);
        }
       else return false;
     
    }
}
