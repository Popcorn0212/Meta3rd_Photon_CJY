﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브의 Prefab
    public GameObject cubeFactory;

    public GameObject impactFactory;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 만약에 내 것이 아니라면
        if (!photonView.IsMine) return;

        // 마우스 오른쪽 버튼 누르면
        if (Input.GetMouseButtonDown(1))
        {
            // 카메라 위치, 카메라 앞방향으로 된 Ray를 만들자.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            // 만들어진 Ray를 이용해서 Raycast 하자.
            RaycastHit hit;
            // 만약 부딪힌 지점이 있으면
            if(Physics.Raycast(ray, out hit))
            {
                // 폭발효과를 생성하고 부딪힌 위치에 놓자.
                //CreateImpact(hit.point);
                photonView.RPC(nameof(CreateImpact), RpcTarget.All, hit.point);
            }
        }
        
        // 1번키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 카메라의 앞방향으로 5만큼 떨어진 위치를 구하자.
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
            // 큐브 공장에서 큐브를 생성, 위치, 회전
            PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
        }
    }

    [PunRPC]
    void CreateImpact(Vector3 position)
    {
        GameObject impact = Instantiate(impactFactory);
        impact.transform.position = position;
    }
}
