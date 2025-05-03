using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMovement : MonoBehaviour
{
    Transform cam; // Main Camera
    Vector3 camStartPos;
    Vector2 distance; // Distance in x and y

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 1f)]
    public float parallaxSpeed;

    public float yOffset = -3.5f;


    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++) // find farthest background
        {
            float zDiff = backgrounds[i].transform.position.z - cam.position.z;
            if (zDiff > farthestBack)
            {
                farthestBack = zDiff;
            }
        }

        for (int i = 0; i < backCount; i++) // set speed of each background
        {
            float zDiff = backgrounds[i].transform.position.z - cam.position.z;
            backSpeed[i] = 1 - (zDiff / farthestBack);
        }
    }

    private void LateUpdate()
    {
        distance = new Vector2(cam.position.x - camStartPos.x, cam.position.y - camStartPos.y);
        transform.position = new Vector3(cam.position.x - 3, cam.position.y + yOffset, 9.92f);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backSpeed[i] * parallaxSpeed;
            Vector2 offset = distance * speed;
            mat[i].SetTextureOffset("_MainTex", offset);
        }
    }
}

