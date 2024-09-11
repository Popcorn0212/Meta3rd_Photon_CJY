using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    // InputNickName
    public TMP_InputField inputNickName;
    // btn_Connect
    public Button btn_Connect;


    void Start()
    {
        // inputNickName 의 내용이 변경될 때 호출되는 함수
        inputNickName.onValueChanged.AddListener(OnValueChanged);
    }

    void Update()
    {
        
    }

    void OnValueChanged(string s)
    {
        btn_Connect.interactable = s.Length > 0;

        //// 만약에 s 의 길이가 0 보다 크면
        //if (s.Length > 0)
        //{
        //    // 접속 버튼 활성화
        //    btn_Connect.interactable = true;
        //}
        //// 그렇지 않다면 (s 의 길이가 0)
        //else
        //{
        //    // 접속 버튼 비활성화
        //    btn_Connect.interactable = false;
        //}
    }

    public void OnClickConnect()
    {
        // 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속 성공하면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // 닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;
        // 로비씬으로 전환
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
