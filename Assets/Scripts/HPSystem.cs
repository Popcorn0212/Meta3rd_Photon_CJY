using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviourPun
{
    // 최대 HP
    public float maxHP;
    // 현재 HP
    float currHP;
    // HP Bar Image
    public Image hpBar;
    // HP 가 0 이 되었을 때 호출되는 함수를 담을 변수
    public Action onDie;


    void Start()
    {
        InitHP();
    }

    void Update()
    {
        
    }

    public void InitHP()
    {
        // 현재 HP를 최대 HP로 설정
        currHP = maxHP;
    }

    public void UpdateHP(float value)
    {
        photonView.RPC(nameof(RpcUpdateHp), RpcTarget.All, value);
    }

    // HP 갱신 함수
    [PunRPC]
    public void RpcUpdateHp(float value)
    {
        // 현재 HP를 value만큼 더하자.
        currHP += value;

        // HP Bar Image 갱신
        if (hpBar != null)
        {
            hpBar.fillAmount = currHP / maxHP;
        }

        // 만약에 현재 HP가 0보다 작거나 같으면
        if(currHP <= 0)
        {
            print(gameObject.name + "의 HP가 0 입니다.");

            if(onDie != null)
            {
                onDie();
            }
            else
            {
                Destroy(gameObject);
            }

            //if(gameObject.layer == LayerMask.NameToLayer("Player"))
            //{
            //    // 플레이어 죽음 처리
            //    PlayerFire pf = GetComponentInParent<PlayerFire>();
            //    pf.Ondie();
            //}
            //else if(gameObject.layer == LayerMask.NameToLayer("ObstacleCube"))
            //{
            //    // 큐브 죽음 처리
            //    ObstacleCube cube = GetComponent<ObstacleCube>();
            //    cube.Ondie();
            //}
            //else if(gameObject.layer == LayerMask.NameToLayer("Enemy"))
            //{
            //    // 적 죽음 처리
            //}
        }
    }
}
