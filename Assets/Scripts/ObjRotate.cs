using Photon.Pun;
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

    // PhotonView
    public PhotonView pv;


    void Start()
    {
        
    }

    void Update()
    {
        // 만약 내것 이라면
        if (pv.IsMine)
        {
            // 마우스의 lockMode 가 None 이면 (마우스 포인터가 활성화 되어 있다면)
            if (Cursor.lockState == CursorLockMode.None)
                return;

            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            if (useRotX) rotX += my * rotSpeed * Time.deltaTime;
            if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -60, 60);

            transform.localEulerAngles = new Vector3(-rotX, rotY, 0);
        }
    }
}
