using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

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

    // Animator
    Animator anim;

    // A D 키 입력 받을 변수
    float h;
    // W S 키 입력 받을 변수
    float v;


    // 서버에서 넘어오는 위치값
    Vector3 receivePos;
    // 서버에서 넘어오는 회전값
    Quaternion receiveRot;
    //보정 속력
    public float lerpSpeed = 50;

    // LookPos
    public Transform lookPos;

    // 닉네임 UI
    public TMP_Text nickName;


    void Start()
    {
        if (photonView.IsMine)
        {
            // 마우스 잠그기
            Cursor.lockState = CursorLockMode.Locked;
        }

        // 캐릭터 컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
        // 내 것일 때만 카메라를 활성화하자
        cam.SetActive(photonView.IsMine);
        // Animator 가져오기
        anim = GetComponentInChildren<Animator>();

        // 닉네임 UI 에 해당 캐릭터의 주인의 닉네임 설정
        nickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        // 내 것일 때만 컨트롤 하자!
        if (photonView.IsMine)
        {
            // 마우스의 lockMode 가 None 이면 (마우스 포인터가 활성화 되어 있다면)
            if (Cursor.lockState == CursorLockMode.None)
                return;

            // 키보드 WASD입력
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

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
        //나의 Player가 아니라면
        else
        {
            // 위치 보정
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * lerpSpeed);
            // 회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, Time.deltaTime * lerpSpeed);
        }

        // anim을 이용해서 h, v 값을 전달
        anim.SetFloat("DirH", h);
        anim.SetFloat("DirV", v);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만약에 내가 데이터를 보낼 수 있는 상태라면 (내 것이라면)
        if (stream.IsWriting)
        {
            // 나의 위치값을 보낸다.
            stream.SendNext(transform.position);
            // 나의 회전값을 보낸다
            stream.SendNext(transform.rotation);
            // 나의 h 값
            stream.SendNext(h);
            // 나의 v 값
            stream.SendNext(v);
            // LookPos의 위치값을 보낸다
            stream.SendNext(lookPos.position);
        }
        // 데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if (stream.IsReading)
        {
            // 위치값을 받자.
            receivePos = (Vector3)stream.ReceiveNext();
            // 회전값을 받자.
            receiveRot = (Quaternion)stream.ReceiveNext();
            // 서버에서 전달되는 h 값 받자
            h = (float)stream.ReceiveNext();
            // 서버에서 전달되는 v 값 받자
            v = (float)stream.ReceiveNext();
            // LookPos의 위치값을 받자
            lookPos.position = (Vector3)stream.ReceiveNext();
        }
    }
}
