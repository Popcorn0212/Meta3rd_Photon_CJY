using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float moveSpeed = 30;

    // Rigidbody
    Rigidbody rb;

    // 충돌 효과 Prefab
    public GameObject exploFactory;


    void Start()
    {
        // 내것 일때만
        if (photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * moveSpeed;
        }
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        // 내것일때만
        if (photonView.IsMine)
        {
            // 부딛힌 지점을 향해서 Raycast 하자
            Ray ray = new Ray(Camera.main.transform.position, transform.position - Camera.main.transform.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            // 폭발효과를 만들자
            photonView.RPC(nameof(CreateExplo), RpcTarget.All, transform.position, hit.normal);

            // 나를 파괴하자
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void CreateExplo(Vector3 position, Vector3 normal)
    {
        // 폭발효과 생성
        GameObject explo = Instantiate(exploFactory);
        // 생성된 효과를 나의 위치에 위치시키자
        explo.transform.position = position;
        // 생성된 효과의 앞방향을 부딪힌 지점의 normal로 바꾸자
        explo.transform.forward = normal;
    }
}
