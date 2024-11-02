using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �u�ςׂ݉��j�󂵔����Ȃ��Ă͂����Ȃ���Q���v�̃v���C���[�ƏՓ˂��镔��
/// </summary>
public class PlayerHitCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Input.GetKey(KeyCode.S))
        {
            collision.GetComponent<Animator>().Play("JumpFail");
        }
    }
}
