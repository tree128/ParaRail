using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ςׂ݉�j�󂷂��Q��
/// </summary>
public class NormalObstacle : ObstacleBase
{
    [SerializeField] private Sprite farSprite;
    [SerializeField] private Sprite nearSprite;
    [SerializeField] private BoxCollider2D myCollider;
    [SerializeField, Header("�y��Q����X�Փˈʒu�́Z���`�z")] protected float collisionStartTiming;
    [SerializeField, Header("�y��Q����X�Փˈʒu�́`�Z���z")] protected float collisionEndTiming;
    
    [SerializeField] protected float collisionStartTiming_Ver2;
    [SerializeField] protected float collisionEndTiming_Ver2;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // �����蔻�蔭��
        //if (collisionStartTiming <= transform.localScale.x && transform.localScale.x <= collisionEndTiming)
        if (collisionEndTiming_Ver2 <= transform.position.z && transform.position.z <= collisionStartTiming_Ver2)
        {
            if (!myCollider.enabled)
            {
                myCollider.enabled = true;
            }
        }
        else if (myCollider.enabled)
        {
            myCollider.enabled = false;
            //myRenderer.sortingOrder = sortOrder_afterPasage;
        }

        if (elapsedTime >= time_keepSpeed)
        {
            // ���i�X�v���C�g����ߌi�X�v���C�g�֐؂�ւ�
            if (myRenderer.sprite != nearSprite)
            {
                myRenderer.sprite = nearSprite;
            }
        }
    }

    public override void Init()
    {
        myRenderer.sprite = farSprite;
        myCollider.enabled = false;
        base.Init();
    }
}
