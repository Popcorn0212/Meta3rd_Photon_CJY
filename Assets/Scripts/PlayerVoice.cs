using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoice : MonoBehaviour
{
    // Photon Voice View
    public PhotonVoiceView pvv;
    // Image Speaker
    public GameObject imgSpeaker;
    // Image SpeachBubble
    public GameObject imgSpeachBubble;


    void Start()
    {
        
    }

    void Update()
    {
        // 만약에 내가 말하고 있다면
        // imgSpeachBubble을 활성화
        // 그렇지 않으면 비활성화
        imgSpeachBubble.SetActive(pvv.IsRecording);

        // 만약에 말하는 소리가 들린다면
        // imgSpeaker를 활성화
        // 그렇지 않으면 비활성화
        imgSpeaker.SetActive(pvv.IsSpeaking);
    }
}
