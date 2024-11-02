using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public GamePlay game;

    public RectTransform ClockHand;

    private float TimerSecond;
    private int TimerStep;

    AudioSource TrainAudio;

    // Start is called before the first frame update
    void Start()
    {
        TrainAudio = GetComponent<AudioSource>();
        TimerStep = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.GameStart) {
            if (game.Step == 0) {
                TimerSecond = (360 / game.MaxTime);
            }

            if (game.NowTime < game.MaxTime) {
                ClockHand.Rotate(0, 0, TimerSecond * Time.deltaTime);
            } else {
                ClockHand.Rotate(0, 0, 0);
            }
        }

        switch (TimerStep) {
            case 0:
                if(game.NowTime >= game.MaxTime / 4) {
                    TrainAudio.Play();
                    TrainAudio.pitch = 0.8f;
                    TrainAudio.volume = 0.5f;
                    TimerStep = 1;
                }
                break;

            case 1:
                if (game.NowTime >= (game.MaxTime / 4) * 2) {
                    TrainAudio.Play();
                    TrainAudio.pitch = 0.9f;
                    TrainAudio.volume = 0.75f;
                    TimerStep = 2;
                }
                break;

            case 2:
                if (game.NowTime >= (game.MaxTime / 4) * 3) {
                    TrainAudio.Play();
                    TrainAudio.pitch = 1;
                    TrainAudio.volume = 1;
                    TimerStep = 3;
                }
                break;
        }
    }
}
