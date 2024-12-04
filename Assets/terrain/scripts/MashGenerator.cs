using Assets;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MashGenerator : MonoBehaviour
{
    [SerializeField]
    private int xSize=20;
    [SerializeField]
    private int zSize=20;
    [SerializeField]
    private float minY =-3;
    [SerializeField]
    private float maxY = 5;
    [SerializeField]
    private int steps = 20;
    [SerializeField]
    private int players = 20;
    [SerializeField]
    private float dif = 3;
    [SerializeField]
    private int smoothIteration=2;
    [SerializeField]
    private Material material;
    [SerializeField] List<Layer> layers=new List<Layer>();
    [SerializeField] GameObject rock;


    [Range(0, 1)] public float startOfTreeHieght;
    [Range(0, 1)] public float endOfTreeHieght;
    public void setXSize(float value)
    {
        xSize=(int)value;
    }
    public void setZSize(float value)
    {
        zSize = (int)value;
    }
    public void setMinY(float value)
    {
            minY = value;
    }
    public void setMaxY(float value)
    {
            maxY = value;
    }
    public void setSteps(float value)
    {
            steps = (int)value;
    }
    public void setPlayers(float value)
    {
            players = (int)value;
    }
    public void setDif(float value)
    {
            dif =value;
    }
    public void setSmooth(float value)
    {
            smoothIteration = (int)value;
    }
    public Vector3[] getVerteces()
    {
        return vertices;
    }

    float scale;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    int seed;

    public void setSeed(int value)
    {
        seed = value;
        Random.seed = seed;
    }

    public void makeChumk()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        Create();
        UpdateMesh();
        GenerateTexture();
        scale = 1.0f / (xSize + zSize);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    

        //public void makeTreeForChunk(float x, float z, List<GameObject> trees)
        //{
        //    float[] changeForTree = new float[vertices.Length];

        //    for (int i = 0; i < vertices.Length; i++)
        //    {
        //        float hieght = (vertices[i].y * scale - minY) / (maxY - minY);
        //        if (hieght > startOfTreeHieght && hieght < endOfTreeHieght)
        //        {
        //            changeForTree[i] = 1;
        //        }
        //    }
        //    for (int i = 0, j = 0; i < changeForTree.Length; i++, j++)
        //    {
        //        if (changeForTree[i] != 0)
        //        {
        //            if (i > xSize)
        //            {
        //                changeForTree[i] += changeForTree[i - 41];
        //            }
        //            if (i < changeForTree.Length - xSize)
        //            {
        //                changeForTree[i] += changeForTree[i + 41];
        //            }
        //            if (j > 0)
        //            {
        //                changeForTree[i] += changeForTree[i - 1];
        //            }
        //            if (j < xSize)
        //            {
        //                changeForTree[i] += changeForTree[i + 1];
        //            }
        //        }
        //        if (j == xSize)
        //        {
        //            j = 0;
        //        }
        //    }
        //    for (int i = 0,j=0; i < vertices.Length; i++)
        //    {
        //        float height = (vertices[i].y * scale - minY) / (maxY - minY);
        //        if (height >startOfTreeHieght && height < endOfTreeHieght&& changeForTree[i]>4)
        //        {
        //            if (Random.Range(-2, 2) > 0)
        //            {
        //                Vector3 treePosition = new Vector3(x + vertices[i].x * scale, vertices[i].y * scale, z + vertices[i].z * scale);
        //                Instantiate(trees[j], treePosition,new Quaternion());
        //                if(j<trees.Count-1)
        //                j++;
        //                else j = 0;
        //            }  
        //        }
        //        else
        //        {
        //            if (Random.Range(-20, 2) > 0)
        //            {
        //                Vector3 rockPosition = new Vector3(x + vertices[i].x * scale, vertices[i].y * scale, z + vertices[i].z * scale);
        //                rock.transform.localScale = new Vector3(scale,scale,scale);
        //                Instantiate(rock, rockPosition, new Quaternion());
        //            }
        //        }
        //    }
    //}


    void Create()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        Generator generator = new Generator(seed);
        generator.MinY = minY;
        generator.MaxY = maxY;

        vertices = generator.GetVertices(xSize, zSize, (int)(steps*Random.Range(0.7f,1.5f)), (int)(players * Random.Range(0.7f, 1.5f)), (dif * Random.Range(0.7f, 1.5f)), smoothIteration);

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }


    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    private void GenerateTexture()
    {
        float trueMin = minY*scale;
        float trueMax = maxY*scale;

        material.SetFloat("minTerrainHeight", trueMin);
        material.SetFloat("maxTerrainHeight", trueMax);

        int layersCount = layers.Count;
        material.SetInt("numTextures", layersCount);

        float[] heights = new float[layersCount];
        int index = 0;
        foreach (Layer l in layers)
        {
            heights[index] = l.startHieght;
            index++;
        }
        material.SetFloatArray("terrainHeights", heights);

        Texture2DArray textures = new Texture2DArray(64, 64, layersCount, TextureFormat.RGBA32, true);

        for (int i = 0; i < layersCount; i++)
        {
            textures.SetPixels(layers[i].texture.GetPixels(), i);
        }

        textures.Apply();
        material.SetTexture("terrainTextures", textures);
    }
}
[System.Serializable]
class Layer
{
    public Texture2D texture;
    [Range (0,1)]public float startHieght;
}
