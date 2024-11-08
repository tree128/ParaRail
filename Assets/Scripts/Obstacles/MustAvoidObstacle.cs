using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 積み荷も破壊し避けなくてはいけない障害物
/// </summary>
public class MustAvoidObstacle : ObstacleBase
{
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField, Header("障害物のX最初の位置は〇")] private Vector3 startPos;
    [SerializeField, Header("障害物のX最後の位置は〇")] private Vector3 endPos;
    [SerializeField, Header("【障害物のX衝突位置は〇％〜】")] protected float collisionStartTiming;
    [SerializeField, Header("【障害物のX衝突位置は〜〇％】")] protected float collisionEndTiming;
    [SerializeField] private int sortOrder_afterPasage;

    protected override void Start()
    {
        //nearSprite = sprite_near;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // 当たり判定発生
        if (collisionStartTiming <= transform.localScale.x && transform.localScale.x <= collisionEndTiming)
        {
            myCollider.enabled = true;
        }
        else if (myCollider.enabled)
        {
            myCollider.enabled = false;
            myRenderer.sortingOrder = sortOrder_afterPasage;
        }

        // 位置調整
        if (isMove)
        {
            var rate = (transform.localScale.x - minSize) / (maxSize - minSize);
            //transform.position = Vector3.Lerp(startPos, endPos, rate);
            transform.position = Vector3.Lerp(startPos, endPos, rate) * (1 + Time.deltaTime);
        }
    }

    public override void Init()
    {
        base.Init();
        transform.position = startPos;
        myCollider.enabled = false;
    }
}
