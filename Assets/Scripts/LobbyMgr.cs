using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class LobbyMgr : MonoBehaviourPunCallbacks
{
    // InputRoomName
    public TMP_InputField inputRoomName;
    // InputMaxPlayer
    public TMP_InputField inputMaxPlayer;
    // InoutPassword
    public TMP_InputField inputPassword;
    // CreateButton
    public Button btn_Create;
    // Join Button
    public Button btn_Join;

    // 전체 방에 대한 정보
    Dictionary<string, RoomInfo> allRoomInfo = new Dictionary<string, RoomInfo>();


    void Start()
    {
        // 로비 진입
        PhotonNetwork.JoinLobby();

        // inputRoomName 의 내용이 변경될 때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnValueChangedRoomName);
        // inputMaxPlayer 의 내용이 변경될 때 호출되는 함수 등록
        inputMaxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);

        #region Dictionary 사용 예제
        //Dictionary<int, string> dic = new Dictionary<int, string>();
        ////dic.Add(12, "값1");

        //// value 에 대한 값을 추가/ 수정
        //dic[12] = "값1";
        //dic[12] = "안녕하세요";
        //dic[13] = "값2";

        //print(dic[12]); // 안녕하세요
        //print(dic[13]); // 값2

        //// 삭제
        //dic.Remove(12);
        //print(dic[12]); // 오류
        #endregion
        #region HashTable 사용 예제

        object number = this;

        Hashtable hash = new Hashtable();
        hash[12] = "값1";         // key : int, value : string
        hash["나이"] = 27;        // key : string, value : int
        hash["미성년자"] = false; // key : string, value : bool

        string str = (string)hash[12];
        int age = (int)hash["나이"];
        bool isMinor = (bool)hash["미성년자"];

        #endregion
    }

    void Update()
    {
        
    }

    // 방 이름과 최대 인원의 값이 있을 때만 Create 버튼 활성화
    // 방 이름에 값이 있으면 Join 버튼 활성화

    // Join & Create 버튼을 활성화 / 비활성화
    void OnValueChangedRoomName(string roomName)
    {
        // Join 버튼 활성 / 비활성
        btn_Join.interactable = roomName.Length > 0;

        // Create 버튼 활성화 / 비활성
        btn_Create.interactable = roomName.Length > 0 && inputMaxPlayer.text.Length > 0;
    }

    public void CreateRoom()
    {
        // 방 옵션 설정
        RoomOptions option = new RoomOptions();
        // 최대 인원 설정
        option.MaxPlayers = int.Parse(inputMaxPlayer.text);
        // 커스텀 정보 설정
        ExitGames.Client.Photon.Hashtable customInfo = new ExitGames.Client.Photon.Hashtable();
        // 방 제목 만
        customInfo["room_name"] = inputRoomName.text;
        // 잠금 모드인지
        bool isLock = false;
        if (inputPassword.text.Length > 0) isLock = true;
        customInfo["lock_mode"] = isLock;
        // 선택된 map (0, 1, 2)
        int map = Random.Range(0, 3);
        customInfo["map"] = map;

        option.CustomRoomProperties = customInfo;

        // 커스텀 정보를 Lobby 에서 사용할 수 있게 설정
        // 로비에서 알아야 할 커스텀 정보의 key 값들[ 배열 ]
        string[] customKeys = { "room_name", "lock_mode", "map" };
        option.CustomRoomPropertiesForLobby = customKeys;

        // 방 옵션을 기반으로 방을 생성
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, option);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패 : " + message);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방 입장 실패 : " + message);
    }

    // Create 버튼을 활성화 / 비활성화
    void OnValueChangedMaxPlayer(string maxPlayer)
    {
        // Create 버튼 활성 / 비활성
        btn_Create.interactable = maxPlayer.Length > 0 && inputRoomName.text.Length > 0;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공!");
    }

    // 로비에 있을 때 방에 대한 정보들이 변경되면 호출되는 함수
    // roomList = 전체 방목록 X, 변경된 방 정보
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // 방 목록 UI 를 전체 삭제
        RemoveRoomList();

        // 전체 방 정보를 갱신
        UpdateRoomList(roomList);

        // allRoomInfo 를 기반으로 방 목록 UI 를 만들자
        CreateRoomList();

        //for (int i = 0; i < roomList.Count; i++)
        //{
        //    print(roomList[i].Name + ", " + roomList[i].PlayerCount + ", " + roomList[i].RemovedFromList);
        //}
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            // allRoomInfo 의 roomList 에서 [i] 번째 정보가 있니? (roomList[i] 의 방 이름이 key값으로 존재하니?)
            if (allRoomInfo.ContainsKey(roomList[i].Name))
            {
                // allRoomInfo 수정 or 삭제
                // 삭제 된 방이니?
                if (roomList[i].RemovedFromList == true)
                {
                    allRoomInfo.Remove(roomList[i].Name);
                }
                // 수정
                else
                {
                    allRoomInfo[roomList[i].Name] = roomList[i];
                }
            }
            else
            {
                // allRoomInfo 추가
                //allRoomInfo.Add(roomList[i].Name, roomList[i]);
                allRoomInfo[roomList[i].Name] = roomList[i];
            }
        }
    }

    // roomItem 의 Prefab
    public GameObject roomItemFactory;
    // ScrollView 의 Content Transform
    public RectTransform trContent;
    void CreateRoomList()
    {
        foreach(RoomInfo info in allRoomInfo.Values)
        {
            // roomItem prefab 을 이용해서 roomItem 을 만든다.
            GameObject go = Instantiate(roomItemFactory, trContent);
            // 만들어진 roomItem 의 내용을 변경
            // RoomItem 컴포넌트 가져오자
            RoomItem roomItem = go.GetComponent<RoomItem>();

            // 커스텀 정보 중 방 이름 가져오자.
            string roomName = (string)info.CustomProperties["room_name"];
            // 커스텀 정보 중 잠금 모드 가져오자.
            bool isLock = (bool)info.CustomProperties["lock_mode"];
            // 커스텀 정보 중 Map 종류 가져오자
            int mapIdx = (int)info.CustomProperties["map"];

            // 가져온 컴포넌트의 정보를 입력하자
            // ex) 방 이름 ( 5 / 10 )
            roomItem.SetContent(roomName, info.PlayerCount, info.MaxPlayers);
            // 잠금 모드 표현
            roomItem.SetLockMode(isLock);
            // roomItem 에 mapIdx 전달
            roomItem.SetMapIndex(mapIdx);

            // roomItem이 클릭 되었을 때 호출되는 함수 등록
            roomItem.onChangeRoomName = OnChangeRoomNameField;
        }
    }

    void RemoveRoomList()
    {
        // trContent 에 있는 자식을 모두 삭제
        for(int i = 0; i < trContent.childCount; i++)
        {
            Destroy(trContent.GetChild(i).gameObject);
        }
    }

    // Map Image 담을 변수
    public Image imgMap;
    // Map Sprite 들 담을 변수
    public Sprite[] mapThumbnails;
    void OnChangeRoomNameField(string roomName, int mapIdx)
    {
        // 방 이름 설정
        inputRoomName.text = roomName;

        // mapIdx 에 따라서 맵 이미지 보이게 하자
        imgMap.sprite = mapThumbnails[mapIdx];
    }
}
