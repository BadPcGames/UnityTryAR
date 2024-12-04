using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class CameraPosition : MonoBehaviour
{
    private int angleX = 0;
    private int angleZ = 0;
    [SerializeField]
    private float speed = 0.5f; 
    private Vector3 lastMousePosition;
    [SerializeField] private int radius;
    [SerializeField] GameObject canvas;
    private bool isMenu = false;
    private Camera camera;

    void Start()
    {
        canvas.SetActive(false);
        lastMousePosition = Input.mousePosition;
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
        camera.stereoTargetEye=StereoTargetEyeMask.Both;
    }

    public Material anaglyphMaterial;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (anaglyphMaterial != null)
        {
            Graphics.Blit(source, destination, anaglyphMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }


    void Update()
    {
        float deltaX = Input.mousePosition.x - lastMousePosition.x;
        angleX += Mathf.RoundToInt(deltaX * 0.5f);

        float deltaZ = Input.mousePosition.y - lastMousePosition.y;
        angleZ += Mathf.RoundToInt(deltaZ * 0.2f);

        lastMousePosition = Input.mousePosition;

        if (!isMenu)
        {
            transform.rotation = Quaternion.Euler(-angleZ, angleX, 0);

            if (Input.GetKey(KeyCode.W))
            {
                transform.position = new Vector3(transform.position.x + (speed * Mathf.Sin((angleX * Mathf.PI) / 180))
                    , transform.position.y,
                    transform.position.z + (speed * Mathf.Cos((angleX * Mathf.PI) / 180)));
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = new Vector3(transform.position.x - (speed * Mathf.Sin((angleX * Mathf.PI) / 180))
                    , transform.position.y,
                    transform.position.z - (speed * Mathf.Cos((angleX * Mathf.PI) / 180)));
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = new Vector3(transform.position.x + (speed * Mathf.Sin(((angleX + 90) * Mathf.PI) / 180))
                    , transform.position.y,
                    transform.position.z + (speed * Mathf.Cos(((angleX + 90) * Mathf.PI) / 180)));
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = new Vector3(transform.position.x + (speed * Mathf.Sin(((angleX - 90) * Mathf.PI) / 180))
                    , transform.position.y,
                    transform.position.z + (speed * Mathf.Cos(((angleX - 90) * Mathf.PI) / 180)));
            }
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position = new Vector3(transform.position.x
                    , transform.position.y + speed,
                    transform.position.z);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position = new Vector3(transform.position.x
                    , transform.position.y - speed,
                    transform.position.z);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                transform.position = new Vector3(transform.position.x, 45, transform.position.z);
                transform.localRotation = Quaternion.Euler(45,45,0);
                canvas.SetActive(true);
                isMenu = true;
                return;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                canvas.SetActive(false);
                isMenu = false;
                transform.position = new Vector3(transform.position.x , 3, transform.position.z);
                angleX = 0;
                return;
            }
        }
    }

}






