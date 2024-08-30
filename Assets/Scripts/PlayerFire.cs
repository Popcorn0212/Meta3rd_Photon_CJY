﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브의 Prefab
    public GameObject cubeFactory;

    // Impact Prefab
    public GameObject impactFactory;

    // 총알 Prefab
    public GameObject rpcBulletFactory;

    // 총구의 Transform
    public Transform firePos;

    // Animator
    Animator anim;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 만약에 내 것이 아니라면
        if (!photonView.IsMine) return;

        // 마우스 왼쪽 버튼 누르면
        if (Input.GetMouseButtonDown(0))
        {
            // 총쏘는 애니메이션 실행(Fire 트리거 발생)
            photonView.RPC(nameof(SetTrigger), RpcTarget.All, "Fire");
            // 총알공장에서 총알 생성, 총구위치 세텡, 총구회전 세팅
            PhotonNetwork.Instantiate("Bullet", firePos.position, Camera.main.transform.rotation);
        }
        // 마우스 가운데 휠 버튼 눌렀을때
        if(Input.GetMouseButtonDown(2))
        {
            photonView.RPC(nameof(CreateBullet), RpcTarget.All, firePos.position, Camera.main.transform.rotation);
        }

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
            //// 큐브 공장에서 큐브를 생성, 위치, 회전
            //PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
            photonView.RPC(nameof(CreateCube), RpcTarget.All, pos);
        }
    }

    [PunRPC]
    void SetTrigger(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    [PunRPC]
    void CreateBullet(Vector3 position, Quaternion rotation)
    {
        Instantiate(rpcBulletFactory, position, rotation);
    }

    [PunRPC]
    void CreateImpact(Vector3 position)
    {
        GameObject impact = Instantiate(impactFactory);
        impact.transform.position = position;
    }

    [PunRPC]
    void CreateCube(Vector3 position)
    {
        Instantiate(cubeFactory, position, Quaternion.identity);
    }
}
