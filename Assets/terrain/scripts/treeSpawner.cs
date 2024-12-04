using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class treeSpawner : MonoBehaviour
{
    [SerializeField]
    private int iteration = 5;
    [SerializeField]
    private float length = 10;
    [SerializeField]
    private float angle = 30f;
    public float variance = 10f;
    [SerializeField]
    private GameObject wood;
    [SerializeField]
    private GameObject leaves;

    private const string axiom = "X";
    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;
    private float[] randomRotationValues = new float[100];

    private List<Vector3> listForLeaves=new List<Vector3>();

    private void Start()
    {
        angle = 30;
        transformStack = new Stack<TransformInfo>();
        listForLeaves.Add(new Vector3(0, -100, 100));
        for (int i = 0; i < randomRotationValues.Length; i++)
        {
            randomRotationValues[i] = Random.Range(-1f, 1f);
        }

        rules = new Dictionary<char, string>
        {
            { 'X', "[F[-X+F[+FX]][*-X+F[+FX]][/-X+F[+FX]-X]]" },
            //{ 'X', "[FX][-FX][+FX]" },
            //{ 'X', "F[+X][-X]FX" },

            { 'F', "FF" }
        };
        GenerateTree();
    }

    private void GenerateTree()
    {
        currentString = axiom;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iteration; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }
            currentString = sb.ToString();
            sb = new StringBuilder();
        }
        for (int i = 0; i < currentString.Length; i++)
        {
            switch (currentString[i])
            {
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);

                    GameObject branch = Instantiate(wood);
                    branch.transform.SetParent(transform);
                    Vector3 midPoint = (initialPosition + transform.position) / 2;
                    branch.transform.localPosition = midPoint;

                    Vector3 direction = transform.position - initialPosition;
                    branch.transform.up = direction.normalized;

                    float lengthOfBranch = Vector3.Distance(initialPosition, transform.position);
                    branch.transform.localScale = new Vector3(lengthOfBranch / 5, lengthOfBranch, lengthOfBranch / 5);

                    if (currentString[(i + 1) % currentString.Length] == 'X' || currentString[(i + 3) % currentString.Length] == 'F' && currentString[(i + 4) % currentString.Length] == 'X')
                    {
                        GameObject leavesCube = Instantiate(leaves, new Vector3(), new Quaternion());
                        leavesCube.transform.SetParent(transform);
                        leavesCube.transform.position = transform.position;
                        leavesCube.transform.localScale = new Vector3(lengthOfBranch / 1.5f, lengthOfBranch / 1.5f, lengthOfBranch / 1.5f);
                    }
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '*':
                    transform.Rotate(Vector3.up * 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '/':
                    transform.Rotate(Vector3.down * 120 * (1 + variance / 100 * randomRotationValues[i % randomRotationValues.Length]));
                    break;

                case '[':
                    transformStack.Push(new TransformInfo
                    {
                        position = transform.position,
                        rotation = transform.rotation,
                    });
                    break;

                case ']':
                    TransformInfo transformInfo = transformStack.Pop();
                    transform.position = transformInfo.position;
                    transform.rotation = transformInfo.rotation;
                    break;
            }
        }
        foreach (Vector3 el in listForLeaves)
        {
            if (el.y != -100)
            {
                GameObject leavesCube = Instantiate(leaves);
                leavesCube.transform.SetParent(transform);
                leavesCube.transform.position = el;
                leavesCube.transform.localScale = new Vector3(length, length, length);
            }
        }
    }


}


public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
}

//{
//    GameObject leavesCube = Instantiate(leaves);
//    leavesCube.transform.position = transform.position;
//    leavesCube.transform.localScale = new Vector3(lengthOfBranch, lengthOfBranch, lengthOfBranch);
//}