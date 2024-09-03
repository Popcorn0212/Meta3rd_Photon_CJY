using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    // Input Field
    public TMP_InputField inputChat;

    // ChatItem Prefab
    public GameObject chatItemFactory;

    // Content 의 Transform
    public Transform trContent;


    void Start()
    {
        // inputChat 의 내용이 변경될 때 호출되는 함수 등록
        inputChat.onValueChanged.AddListener(OnValueChanged);
        // inputChat 엔터를 쳤을 때 호출되는 함수 등록
        inputChat.onSubmit.AddListener(OnSummit);
        // inputChat 포커싱을 잃을 때 호출되는 함수 등록
        inputChat.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        
    }

    void OnSummit(string s)
    {
        // ChatItem 하나 만들자 (부모를 ChatView 의 Content 로 하자)
        GameObject go = Instantiate(chatItemFactory, trContent);
        // ChatItem 컴포넌트 가져오자.
        ChatItem chatItem = go.GetComponent<ChatItem>();
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(s);

        inputChat.text = "";
    }

    void OnValueChanged(string s)
    {
        //print("변경 중 : " + s);
    }

    void OnEndEdit(string s)
    {
        //print("작성 끝 : " + s);
    }
}
