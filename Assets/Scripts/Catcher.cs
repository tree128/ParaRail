using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMove player;

    // Update is called once per frame
    void Update()
    {
        if (!player.IsPlaying && animator.enabled)
        {
            animator.enabled = false;
        }

        if(player.IsPlaying && !animator.enabled)
        {
            animator.enabled = true;
        }
    }

    public void Catch()
    {
        animator.SetTrigger("Catch");
    }
}
