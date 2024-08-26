using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    float rotSpeed = 200;

    float rotX;
    float rotY;

    // 회전 가능 여부
    public bool useRotX;
    public bool useRotY;


    void Start()
    {
        
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        if(useRotX) rotX += my * rotSpeed * Time.deltaTime;
        if(useRotY) rotY += mx * rotSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -60, 60);

        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);
    }
}
