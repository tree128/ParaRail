using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ςׂ͔݉j�󂵂Ȃ��������Ȃ��Ă͂����Ȃ���Q��
/// </summary>
public class Tunnel : ObstacleBase
{
    [SerializeField] private Sprite entranceSprite;
    [SerializeField] private Sprite exitSprite;
    [SerializeField, Header("�g���l�����őO�\���ɂȂ�X�傫���́Z")] private float orderChangeTiming;
    [SerializeField, Header("�g���l���ɓ����Ă���X���Ԃ́Z")] private float tunnelTime;
    private bool isGameOver = false;
    [SerializeField] private int defaultOrder;
    private GamePlay game;
    [SerializeField] private Vector3 fromScale;
    [SerializeField] private Vector3 toScale_entrance;
    [SerializeField] private Vector3 toScale_exit;

    protected override void Start()
    {
        base.Start();
        game = GameObject.Find("GameManager").GetComponent<GamePlay>();
    }
    protected override void Update()
    {
        //if (transform.localScale.x >= maxSize)
        if (transform.position.z <= endPos_Ver2.z)
        {
            ObstacleManager.Instance.TunnelViewSet(!game.Tonnel);
            if (!game.Tonnel)
            {
                // �g���l���ɓ�����
                isMove = false;
                //transform.localScale *= minSize / transform.localScale.x;
                transform.position = startPos_Ver2;
                transform.localScale = fromScale;
                currntPos = startPos_Ver2;
                myRenderer.enabled = false;
                elapsedTime = 0f;
                myRenderer.sortingOrder = defaultOrder;
                game.Tonnel = true;
            }
            else
            {
                // �g���l������o��
                ObstacleManager.Instance.CanMoveStart = true;
                game.Tonnel = false;
                //Destroy(gameObject);
                ObstacleManager.Instance.ReleaseToPool(this);
            }
        }

        // �g���l����
        if(game.Tonnel && !isMove && !isGameOver)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= tunnelTime)
            {
                myRenderer.sprite = exitSprite;
                myRenderer.enabled = true;
                elapsedTime = 0f;
                //appliedSpeed = firstSpeed;
                appliedSpeed = firstSpeed_Ver2;
                isMove = true;
            }
        }

        // �ړ�(�T�C�Y�ύX)
        /*if (isMove)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= time_keepSpeed)
            {
                appliedSpeed += acceleration * (elapsedTime - time_keepSpeed);
            }
            transform.localScale *= appliedSpeed * (1 + Time.deltaTime);

            var rate = (transform.localScale.x - minSize) / (maxSize - minSize);
            var zPos = transform.position.z;
            transform.position = Vector3.Lerp(startPos, endPos, rate) * (1 + Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        }*/

        // �ړ�(Z���W�ύX)
        if (isMove)
        {
            elapsedTime += Time.deltaTime;
            if (time_keepSpeed <= elapsedTime)
            {
                appliedSpeed += acceleration_Ver2 * Time.deltaTime;
            }
            transform.position += normalizedMoveVector * appliedSpeed * Time.deltaTime;
            rate = (transform.position - startPos_Ver2).magnitude / (endPos_Ver2 - startPos_Ver2).magnitude;
            if (myRenderer.sprite == entranceSprite)
            {
                transform.localScale = Vector3.Lerp(fromScale, toScale_entrance, rate);
            }
            else
            {
                transform.localScale = Vector3.Lerp(fromScale, toScale_exit, rate);
            }
            /*
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= time_keepSpeed)
            {
                // ����
                appliedSpeed += acceleration_Ver2 * (elapsedTime - time_keepSpeed);
            }
            currntPos += normalizedMoveVector * appliedSpeed;
            rate = (currntPos - startPos_Ver2).magnitude / moveVector.magnitude;
            transform.position = Vector3.Lerp(startPos_Ver2, endPos_Ver2, rate) * (1 + Time.deltaTime);
            if(myRenderer.sprite == entranceSprite)
            {
                transform.localScale = Vector3.Lerp(fromScale, toScale_entrance, rate) * (1 + Time.deltaTime);
            }
            else
            {
                transform.localScale = Vector3.Lerp(fromScale, toScale_exit, rate) * (1 + Time.deltaTime);
            }
            */
        }

        /*
        // �v���C���[�B����O�ɕ\��
        if (orderChangeTiming <= transform.localScale.x && !game.Tonnel)
        {
            myRenderer.sortingOrder = sortOrder_afterPasage;
        }
        */
    }

    public override void Init()
    {
        myRenderer.sprite = entranceSprite;
        transform.localScale = fromScale;
        base.Init();
    }


    public override void MoveStop()
    {
        base.MoveStop();
        isGameOver = true;
    }
}
