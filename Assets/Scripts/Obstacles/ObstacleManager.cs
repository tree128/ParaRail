using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// áŠQ•¨‚ğ¶¬‚·‚é
/// </summary>
public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance;
    [SerializeField] private NormalObstacle centerPrefab;
    [SerializeField] private NormalObstacle leftPrefab;
    [SerializeField] private NormalObstacle rightPrefab;
    [SerializeField] private NormalObstacle avoidPrefab;
    [SerializeField] private Tunnel tunnelPrefab;
    private float elapsedTime_center = 0f;
    private float elapsedTime_left = 0f;
    private float elapsedTime_right = 0f;
    private float centerCT = 0f;
    private float leftCT = 0f;
    private float rightCT = 0f;
    [SerializeField, Header("yáŠQ•¨‚ÌX’†‰›ƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz")] private float centerCT_From;
    [SerializeField, Header("yáŠQ•¨‚ÌX’†‰›ƒN[ƒ‹ƒ^ƒCƒ€•Ï“®‚ÍZ•bz")] private float centerCT_To;
    [SerializeField, Header("yáŠQ•¨‚ÌX¶ƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz")] private float leftCT_From;
    [SerializeField, Header("yáŠQ•¨‚ÌX¶ƒN[ƒ‹ƒ^ƒCƒ€•Ï“®‚ÍZ•bz")] private float leftCT_To;
    [SerializeField, Header("yáŠQ•¨‚ÌX‰EƒN[ƒ‹ƒ^ƒCƒ€‚ÍZ•bz")] private float rightCT_From;
    [SerializeField, Header("yáŠQ•¨‚ÌX‰EƒN[ƒ‹ƒ^ƒCƒ€•Ï“®‚ÍZ•bz")] private float rightCT_To;
    public bool CanMoveStart = false;
    public GameObject inTunnelView;
    [SerializeField] private GamePlay game;
    private bool stop = false;
    private ObjectPool<NormalObstacle> centerPool;
    private ObjectPool<NormalObstacle> leftPool;
    private ObjectPool<NormalObstacle> rightPool;
    private ObjectPool<NormalObstacle> avoidPool;
    private ObjectPool<Tunnel> tunnelPool;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        centerPool = PoolInit(centerPrefab);
        leftPool = PoolInit(leftPrefab);
        rightPool = PoolInit(rightPrefab);
        avoidPool = PoolInit(avoidPrefab);
        tunnelPool = PoolInit(tunnelPrefab);

        centerCT = Random.Range(centerCT_From, centerCT_To);
        leftCT = Random.Range(leftCT_From, leftCT_To);
        rightCT = Random.Range(rightCT_From, rightCT_To);
    }

    private ObjectPool<T> PoolInit<T>(T prefab) where T : ObstacleBase
    {
        ObjectPool<T> pool =   new ObjectPool<T>(
            createFunc: () => OnCreate(prefab),
            actionOnGet: (obj) => OnGet(obj),
            actionOnRelease: (obj) => OnRelease(obj),
            actionOnDestroy: (obj) => Pool_OnDestroy(obj),
            collectionCheck: true,
            defaultCapacity: 2,
            maxSize: 3
            );
        var a = pool.Get();
        if (prefab.name != "Tunnel")
        {
            var b = pool.Get();
            pool.Release(b);
        }
        pool.Release(a);
        return pool;
    }

    private T OnCreate<T>(T target) where T : ObstacleBase
    {
        return Instantiate(target, transform);
    }

    private void OnGet<T>(T target) where T : ObstacleBase
    {
        target.Init();
        target.gameObject.SetActive(true);
    }

    private void OnRelease<T>(T target) where T : ObstacleBase
    {
        target.gameObject.SetActive(false);
    }

    private void Pool_OnDestroy<T>(T target) where T : ObstacleBase
    {
        Destroy(target.gameObject);
        target = null;
    }
    // Update is called once per frame
    void Update()
    {
        if(game.GameFinish && !stop)
        {
            StopObstacles();
        }

        if (CanMoveStart)
        {
            elapsedTime_center += Time.deltaTime;
            elapsedTime_left += Time.deltaTime;
            elapsedTime_right += Time.deltaTime;

            if (elapsedTime_center >= centerCT)
            {
                CenterMoveStart();
                //AdjustDrawingOrders();
            }
            if (elapsedTime_left >= leftCT)
            {
                LeftMoveStart();
                //AdjustDrawingOrders();
            }
            if (elapsedTime_right >= rightCT)
            {
                RightMoveStart();
                //AdjustDrawingOrders();
            }
        }
    }

    private void CenterMoveStart()
    {
        var num = Random.Range(0, 5);
        if(num != 0)
        {
            // Ï‚İ‰×‚ğ”j‰ó‚·‚éáŠQ•¨iƒTƒ{ƒeƒ“j
            /*
            var center = Instantiate(centerPrefab, transform);
            //var index = Random.Range(0, farSpriteArray.Length);
            //center.Init(farSpriteArray[index], nearSpriteArray[index]);
            center.Init();
            */
            centerPool.Get();
            centerCT = Random.Range(centerCT_From, centerCT_To);
            elapsedTime_center = 0;
        }
        else
        {
            // Ï‚İ‰×‚Í”j‰ó‚µ‚È‚¢‚ª”ğ‚¯‚È‚­‚Ä‚Í‚¢‚¯‚È‚¢áŠQ•¨iƒgƒ“ƒlƒ‹j
            /*
            var tunnel = Instantiate(tunnelPrefab, transform);
            tunnel.Init();
            */
            tunnelPool.Get();
            CanMoveStart = false;
        }
    }

    private void LeftMoveStart()
    {
        var num = Random.Range(0, 2);
        if (num == 0)
        {
            // Ï‚İ‰×‚ğ”j‰ó‚·‚éáŠQ•¨iƒTƒ{ƒeƒ“j
            /*
            var left = Instantiate(leftPrefab, transform);
            //var index = Random.Range(0, farSpriteArray.Length);
            //left.Init(farSpriteArray[index], nearSpriteArray[index]);
            left.Init();
            */
            leftPool.Get();
        }
        else
        {
            // Ï‚İ‰×‚à”j‰ó‚µ”ğ‚¯‚È‚­‚Ä‚Í‚¢‚¯‚È‚¢áŠQ•¨iƒNƒŒ[ƒ“j
            /*
            var avoid = Instantiate(avoidPrefab, transform);
            avoid.Init();
            */
            avoidPool.Get();
        }

        leftCT = Random.Range(leftCT_From, leftCT_To);
        elapsedTime_left = 0;
    }

    private void RightMoveStart()
    {
        // Ï‚İ‰×‚ğ”j‰ó‚·‚éáŠQ•¨iƒTƒ{ƒeƒ“j
        /*
        var right = Instantiate(rightPrefab, transform);
        //var index = Random.Range(0, farSpriteArray.Length);
        //right.Init(farSpriteArray[index], nearSpriteArray[index]);
        right.Init();
        */
        rightPool.Get();
        rightCT = Random.Range(rightCT_From, rightCT_To);
        elapsedTime_right = 0;
    }

    public void ReleaseToPool<T>(T type) where T : ObstacleBase
    {
        if(type.TryGetComponent(out Tunnel tunnel))
        {
            tunnelPool.Release(tunnel);
        }
        else
        {
            NormalObstacle normal = type.GetComponent<NormalObstacle>();
            if (type.gameObject.name.Contains("Center"))
            {
                centerPool.Release(normal);
            }
            else if (type.gameObject.name.Contains("Left"))
            {
                leftPool.Release(normal);
            }
            else if (type.gameObject.name.Contains("Right"))
            {
                rightPool.Release(normal);
            }
            else
            {
                avoidPool.Release(normal);
            }
        }
    }

    public void TunnelViewSet(bool b)
    {
        inTunnelView.SetActive(b);
    }

    private void StopObstacles()
    {
        stop = true;
        CanMoveStart = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ObstacleBase>().MoveStop();
        }
    }

    private void AdjustDrawingOrders()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ObstacleBase>().AdjustDrawingOrder();
        }
    }
}
