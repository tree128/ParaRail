using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// è·äQï®ÇÃäÓíÍÉNÉâÉX
/// </summary>
public class ObstacleBase: MonoBehaviour
{
    [SerializeField] protected SpriteRenderer myRenderer;
    [SerializeField, Header("Åyè·äQï®ÇÃXç≈è¨ï\é¶ÇÕÅZÅìÅz")] protected float minSize;
    [SerializeField, Header("è·äQï®ÇÃXç≈ëÂï\é¶ÇÕÅZÅì")] protected float maxSize;
    [SerializeField, Header("è·äQï®ÇÃXç≈èâÇÃà íuÇÕÅZ")] protected Vector3 startPos;
    [SerializeField, Header("è·äQï®ÇÃXç≈å„ÇÃà íuÇÕÅZ")] protected Vector3 endPos;
    [SerializeField, Header("Åyè·äQï®ÇÃXèâë¨ìxÇÕÅZÅìÅz"), Range(1.0001f, 1.1f)] protected float firstSpeed;
    [SerializeField, Header("Åyè·äQï®ÇÃXâ¡ë¨äJénÇÕÅZïbÅz")] protected float time_keepSpeed;
    [SerializeField, Header("Åyè·äQï®ÇÃXâ¡ë¨ìxÇÕÅZÅìÅz"), Range(0.0001f, 0.1f)] protected float acceleration;
    protected float elapsedTime;
    protected float appliedSpeed;
    protected bool isMove = false;
    //[SerializeField] protected int sortOrder_afterPasage = 11;

    [SerializeField] protected Vector3 startPos_Ver2;
    [SerializeField] protected Vector3 endPos_Ver2;
    [SerializeField] protected float firstSpeed_Ver2;
    [SerializeField] protected float acceleration_Ver2;
    protected Vector3 moveVector;
    protected Vector3 normalizedMoveVector;
    protected Vector3 currntPos;
    protected float rate = 0f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        moveVector = endPos_Ver2 - startPos_Ver2;
        normalizedMoveVector = moveVector.normalized;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if(transform.localScale.x >= maxSize)
        if(transform.position.z <= endPos_Ver2.z)
        {
            //Destroy(gameObject);
            ObstacleManager.Instance.ReleaseToPool(this);
        }

        // à⁄ìÆ(ÉTÉCÉYïœçX)
        /*if (isMove)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= time_keepSpeed)
            {
                // â¡ë¨
                appliedSpeed += acceleration * (elapsedTime - time_keepSpeed);
            }
            transform.localScale *= appliedSpeed * (1+ Time.deltaTime);

            var rate = (transform.localScale.x - minSize) / (maxSize - minSize);
            var zPos = transform.position.z;
            transform.position = Vector3.Lerp(startPos, endPos, rate) * (1 + Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        }*/

        // à⁄ìÆ(Zç¿ïWïœçX)
        if (isMove)
        {
            elapsedTime += Time.deltaTime;
            if (time_keepSpeed <= elapsedTime)
            {
                appliedSpeed += acceleration_Ver2 * Time.deltaTime;
            }
            transform.position += normalizedMoveVector * appliedSpeed * Time.deltaTime;
            /*
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= time_keepSpeed)
            {
                // â¡ë¨
                appliedSpeed += acceleration_Ver2 * (elapsedTime - time_keepSpeed);
            }
            currntPos -= normalizedMoveVector * appliedSpeed * (1 + Time.deltaTime);
            rate = (currntPos - startPos_Ver2).magnitude / moveVector.magnitude * (1 + Time.deltaTime);

            transform.position = Vector3.Lerp(startPos_Ver2, endPos_Ver2, rate);
            */
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.CompareTag("Cargo"))
        {
            Debug.Log("Hit:Cargo");
            collision.GetComponentInParent<Animator>().enabled = false;
            Destroy(collision.gameObject.transform.parent.gameObject);
        }*/
    }

    public virtual void Init()
    {
        //transform.localScale *= minSize;
        transform.position = startPos_Ver2;
        currntPos = startPos_Ver2;
        myRenderer.enabled = true;
        //appliedSpeed = firstSpeed;
        appliedSpeed = firstSpeed_Ver2;
        elapsedTime = 0f;
        isMove = true;
    }

    public virtual void MoveStop()
    {
        isMove = false;
    }

    public virtual void AdjustDrawingOrder()
    {
        int order = transform.GetSiblingIndex();
        if (transform.position.z != order)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, order);
        }
    }
}
