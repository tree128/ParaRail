using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField] private Image OPImage;
    [SerializeField] private Sprite OPSprite2;
    [SerializeField] private Sprite OPSprite3;
    [SerializeField] private Sprite OPSprite4;
    [SerializeField] private PlayerMove player;
    [SerializeField, Range(0.01f, 0.5f)] private float waitTime;
    [SerializeField] Image darkenImage;
    [SerializeField] private GamePlay game;

    private void Start()
    {
        OPImage.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (OPImage.enabled && Input.anyKeyDown)
        {
            if (OPImage.sprite == OPSprite2)
            {
                OPImage.sprite = OPSprite3;
            }
            else if (OPImage.sprite == OPSprite3)
            {
                OPImage.sprite = OPSprite4;
            }
            else if (OPImage.sprite == OPSprite4)
            {
                game.GameStart = true;
                ObstacleManager.Instance.gameObject.SetActive(true);
                ObstacleManager.Instance.CanMoveStart = true;
                Invoke("Wait", waitTime);
                OPImage.enabled = false;
            }
            else
            {
                OPImage.sprite = OPSprite2;
            }
        }

        if(ObstacleManager.Instance.inTunnelView.activeInHierarchy && !darkenImage.enabled)
        {
            darkenImage.enabled = true;
        }
        if (!ObstacleManager.Instance.inTunnelView.activeInHierarchy && darkenImage.enabled)
        {
            darkenImage.enabled = false;
        }
    }

    private void Wait()
    {
        player.IsPlaying = true;
        game.Step = 0;
    }
}
