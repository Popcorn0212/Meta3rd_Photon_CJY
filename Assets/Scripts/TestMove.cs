using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


// ���������� ��Ȯ�ϰ� 5��ŭ �̵��ϰ� �ʹ�.
public class TestMove : MonoBehaviour
{
    // �̵��Ÿ�
    float moveDist = 0;
    // ������ �� �ִ���
    bool isMove = true;

    void Start()
    {
        
        
    }

    void Update()
    {
        if (isMove)
        {
            // 1. ���������� �����̱�
            transform.position += Vector3.right * 5 * Time.deltaTime;
            // �̵��Ÿ� ����
            moveDist += 5 * Time.deltaTime;
            // 2. ���࿡ ������ �Ÿ��� 5 ���� Ŀ����
            if (moveDist >= 5)
            {
                // 3. ���߱�
                isMove = false;
                // 4. ���� �� �̵��Ÿ� ��ŭ �������� �̵���Ű��
                float overDist = moveDist - 5;
                transform.position += Vector3.left * overDist;
            }
        }
    }
}
