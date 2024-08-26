using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMove : MonoBehaviour
{
    // 이동속도
    float speed = 5;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxisRaw("Horizontal");
        float mz = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(mx, 0, mz);
        dir.Normalize();

        dir = transform.TransformDirection(dir);

        transform.position += dir * speed * Time.deltaTime;
    }
}
