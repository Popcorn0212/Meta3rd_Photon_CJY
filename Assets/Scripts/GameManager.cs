using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Spawn 위치를 담아놓을 변수
    public Vector3[] spawnPos;
    public Transform spawnCenter;


    void Start()
    {
        SetSpawnPos();

        // RPC 보내는 빈도 설정
        PhotonNetwork.SendRate = 60;
        // OnPhotonSerializeView 보내고 받고 하는 설정
        PhotonNetwork.SerializationRate = 60;

        // 네가 위치해야 하는 inx를 알아오자 (현재 방에 들어와 있는 인원수)
        int idx = PhotonNetwork.CurrentRoom.PlayerCount;

        // 플레이어를 생성 (현재 Room에 접속 되어있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", spawnPos[idx], Quaternion.identity);

    }

    void Update()
    {
        
    }

    
    void SetSpawnPos()
    {
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;

        // 최대 인원 만큼 spawnPos 의 공간을 할당
        spawnPos = new Vector3[maxPlayer];

        // spawnPos 간의 간격(각도)
        float angle = 360.0f / maxPlayer;
        
        // maxPlayer 만큼 반복
        for(int i = 0; i < maxPlayer; i++)
        {
            // spawnCenter 회전 (i * angle)만큼
            spawnCenter.eulerAngles = new Vector3(0, i * angle, 0);
            // spawnCenter 앞방향으로 2만큼 떨어진 위치 구하자
            spawnPos[i] = spawnCenter.position + spawnCenter.forward * 2;
            //// 큐브 하나 생성
            //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //// 생성된 큐브를 위에서 구한 위치에 놓자
            //cube.transform.position = spawnPos[i];
        }
    }
}
