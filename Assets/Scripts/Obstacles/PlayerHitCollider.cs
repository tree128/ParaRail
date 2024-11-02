using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 「積み荷も破壊し避けなくてはいけない障害物」のプレイヤーと衝突する部分
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
