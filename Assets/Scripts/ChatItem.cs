using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatItem : MonoBehaviour
{
    // Text
    TMP_Text chatText;


    private void Awake()
    {
        // Text 컴포넌트 가져오기
        chatText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetText(string s)
    {
        // 텍스트 갱신
        chatText.text = s;

        // 사이즈 조절 코르틴 실행
        StartCoroutine(UpdateSize());
    }

    IEnumerator UpdateSize()
    {
        yield return null;

        // 텍스트의 내용에 맞춰서 크기를 조절
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);
    }
}
