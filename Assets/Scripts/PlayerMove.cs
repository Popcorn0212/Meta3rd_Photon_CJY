using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMove : MonoBehaviour
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


    void Start()
    {
        // 캐릭터 컨트롤러 컴포넌트 가져오기
        cc = GetComponent<CharacterController>();
    }

    void Update()
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
        if(cc.isGrounded)
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
}
