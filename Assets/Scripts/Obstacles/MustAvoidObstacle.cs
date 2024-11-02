using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ςׂ݉��j�󂵔����Ȃ��Ă͂����Ȃ���Q��
/// </summary>
public class MustAvoidObstacle : ObstacleBase
{
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField, Header("��Q����X�ŏ��̈ʒu�́Z")] private Vector3 startPos;
    [SerializeField, Header("��Q����X�Ō�̈ʒu�́Z")] private Vector3 endPos;
    [SerializeField, Header("�y��Q����X�Փˈʒu�́Z���`�z")] protected float collisionStartTiming;
    [SerializeField, Header("�y��Q����X�Փˈʒu�́`�Z���z")] protected float collisionEndTiming;
    [SerializeField] private int sortOrder_afterPasage;

    protected override void Start()
    {
        //nearSprite = sprite_near;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // �����蔻�蔭��
        if (collisionStartTiming <= transform.localScale.x && transform.localScale.x <= collisionEndTiming)
        {
            myCollider.enabled = true;
        }
        else if (myCollider.enabled)
        {
            myCollider.enabled = false;
            myRenderer.sortingOrder = sortOrder_afterPasage;
        }

        // �ʒu����
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
