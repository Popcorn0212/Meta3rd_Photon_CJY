using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun, IPunObservable 
{
    // 이동속도
    float speed = 5;

    // 캐릭터 컨트롤러
    CharacterController cc;

    // 중력
    float gravity = -9.8f;
    // y 속력
    float yVelocity;
    // 점프 초기 속력
    public float jumpPower = 3;
    // 카메라
    public GameObject cam;

    // 서버에서 넘어오는 위치값
    Vector3 receivePos;
    // 서버에서 넘어오는 회전값
    Quaternion receiveRot;
    //보정 속력
    public float lerpSpeed = 50;

    void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
        // 내 것일 때만 카메라를 활성화하자
        cam.SetActive(photonView.IsMine);
    }

    void Update()
    {
        // 내 것일 때만 컨트롤 하자!
        if (photonView.IsMine)
        {
            // 키보드 WASD입력
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // 방향 지정
            Vector3 dir = new Vector3(h, 0, v);
            dir.Normalize();

            // 방향을 자신을 기준으로 dir 변경
            dir = transform.TransformDirection(dir);

            // 만약, 땅에 닿아 있으면 yVelocity 를 0으로 초기화
            if (cc.isGrounded)
            {
                yVelocity = 0;
            }

            // 만약, space를 누를 시
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // yVelocity 를 jumpPower 로 설정
                yVelocity = jumpPower;
            }

            // yVelocity 값을 중력에 의해서 변경
            yVelocity += gravity * Time.deltaTime;

            #region 물리적이지 않은 점프
            // dir.y 에 yVelocity 값을 세팅
            dir.y = yVelocity;

            // 캐릭터 컨트롤러를 이용한 이동
            cc.Move(dir * speed * Time.deltaTime);
            #endregion

            #region 물리적인 점프
            //dir = dir * speed;
            //dir.y = yVelocity;
            //cc.Move(dir * Time.deltaTime);
            #endregion
        }
        // 나의 Player가 아니라면
        else
        {
            // 위치 보정
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * lerpSpeed);
            // 회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * lerpSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만약에 내가 데이터를 보낼 수 있는 상태라면 (내 것이라면)
        if(stream.IsWriting)
        {
            // 나의 위치값을 보낸다.
            stream.SendNext(transform.position);
            // 나의 회전값을 보낸다
            stream.SendNext(transform.rotation);
        }
        // 데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if(stream.IsReading)
        {
            // 위치값을 받자.
            transform.position = (Vector3)stream.ReceiveNext();
            // 회전값을 받자.
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
