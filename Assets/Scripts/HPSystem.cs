using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    // 최대 HP
    public float maxHP;
    // 현재 HP
    float currHP;
    // HP Bar Image
    public Image hpBar;


    void Start()
    {
        // 현재 HP를 최대 HP로 설정
        currHP = maxHP;
    }

    void Update()
    {
        
    }


    // HP 갱신 함수
    public void UpdateHp(float value)
    {
        // 현재 HP를 value만큼 더하자.
        currHP += value;

        // HP Bar Image 갱신
        hpBar.fillAmount = currHP / maxHP;

        // 만약에 현재 HP가 0보다 작거나 같으면
        if(currHP <= 0)
        {

        }
    }
}
