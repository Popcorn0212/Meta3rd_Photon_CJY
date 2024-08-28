using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviourPun
{
    // 큐브의 Prefab
    public GameObject cubeFactory;

    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내 것이 아니라면
        if (!photonView.IsMine) return;
        
        // 1번키 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 카메라의 앞방향으로 5만큼 떨어진 위치를 구하자.
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 5;
            // 큐브 공장에서 큐브를 생성, 위치, 회전
            PhotonNetwork.Instantiate("Cube", pos, Quaternion.identity);
        }
    }
}
