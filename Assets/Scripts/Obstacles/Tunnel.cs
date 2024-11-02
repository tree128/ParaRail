using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 積み荷は破壊しないが避けなくてはいけない障害物
/// </summary>
public class Tunnel : ObstacleBase
{
    [SerializeField] private Sprite entranceSprite;
    [SerializeField] private Sprite exitSprite;
    [SerializeField, Header("トンネルが最前表示になるX大きさは〇")] private float orderChangeTiming;
    [SerializeField, Header("トンネルに入っているX時間は〇")] private float tunnelTime;
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
                // トンネルに入った
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
                // トンネルから出た
                ObstacleManager.Instance.CanMoveStart = true;
                game.Tonnel = false;
                //Destroy(gameObject);
                ObstacleManager.Instance.ReleaseToPool(this);
            }
        }

        // トンネル内
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

        // 移動(サイズ変更)
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

        // 移動(Z座標変更)
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
                // 加速
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
        // プレイヤー達より手前に表示
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
