using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    // 내용 담는 Text
    public Text roomInfo;
    // 잠금 표시 Image
    public GameObject img_lock;
    // 방 이름
    string realRoomName;
    // map index
    int mapIndex;


    // 클릭 되었을 때 호출되는 함수를 가지고 있는 변수
    public Action<string, int> onChangeRoomName;


    public void SetContent(string roomName, int curPlayer, int maxPlayer)
    {
        // roomName을 전역변수에 담아놓자
        realRoomName = roomName;

        // 정보 입력
        roomInfo.text = roomName + " ( " + curPlayer + " / " + maxPlayer + " )";
    }

    public void SetLockMode(bool isLock)
    {
        img_lock.SetActive(isLock);
    }

    public void SetMapIndex(int index)
    {
        mapIndex = index;
        // 추가적으로 mapIndex에 따른 이미지 출력
    }

    public void OnClick()
    {
        // 만약에 onChangeRoomName 의 함수가 들어있다면
        if (onChangeRoomName != null)
        {
            // 헤당 함수 실행
            onChangeRoomName(realRoomName, mapIndex);
        }

        //// 1. InputRoomName 게임오브젝트 찾자.
        //GameObject go = GameObject.Find("InputRoomName");
        //// 2. 찾은 게임오브젝트에서 Tmp_InputField 컴포넌트 가져오자
        //TMP_InputField inputField = go.GetComponent<TMP_InputField>();
        //// 3. 가져온 컴포넌트를 이용해서 내용 변경
        //inputField.text = realRoomName;
    }
}
