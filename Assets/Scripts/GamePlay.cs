using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlay : MonoBehaviour
{
    public bool GameStart;
    public bool GameFinish;
    public int Step { get; set; } = -1;
    public bool Tonnel;

    public bool PlayerJump;
    public bool Clear { get; set; }
    public bool JumpTextNow { get; set; }

    public int MaxCargo;
    public int NowCargo;

    public bool NextCargo { get; set; }

    [Space(20)]

    public int TotalPoint;
    public int TotalPlusPoint;
    public int TotalMinusPoint;

    public int PlusPoint;
    public int MinusPoint;

    public int ClearPoint;

    [Space(20)]

    public float MaxTime;
    public float NowTime;

    public GameObject Cargo;
    public TextMeshProUGUI PointCount;
    public TextMeshProUGUI CargoCount;
    public GameObject Result;
    public GameObject JumpText;

    AudioSource TrainAudio;
    public AudioClip TrainMove;
    public GameObject WindSE;
    public AudioSource BGM;
    public AudioClip BGMSound;


    // Start is called before the first frame update
    void Start()
    {
        TrainAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart) {
            switch (Step) {
                case 0:
                    Instantiate(Cargo, new Vector3(-6.1f, -2.15f, 0.0f), Quaternion.identity);
                    NextCargo = false;
                    NowCargo = 0;
                    TotalPoint = 0;
                    TotalPlusPoint = 0;
                    TotalMinusPoint = 0;
                    PlayerJump = false;
                    Clear = false;
                    JumpTextNow = false;
                    Tonnel = false;
                    WindSE.SetActive(true);
                    BGM.volume = 0.2f;
                    BGM.Play();
                    Step = 1;
                    break;

                case 1:
                    if (NowCargo < MaxCargo) {
                        if (NextCargo) {
                            NowCargo += 1;
                            if (NowCargo != MaxCargo) {
                                Instantiate(Cargo, new Vector3(-6.1f, -2.15f, 0.0f), Quaternion.identity);
                            }
                            NextCargo = false;
                        }
                    } else {
                        Step = 2;
                        JumpTextNow = true;
                    }
                    break;

                case 2:
                    PlayerJump = true;
                    if(BGM.volume > 0) {
                        BGM.volume -= 0.1f * Time.deltaTime;
                        if(BGM.volume == 0) {
                            BGM.volume = 0;
                            BGM.Stop();
                        }
                    }
                    break;

                case 3:
                    GameFinish = true;
                    GameStart = false;
                    BGM.Stop();
                    WindSE.SetActive(false);
                    Step = 4;
                    Invoke("ResultOn", 0.5f);
                    break;
            }

            NowTime = Mathf.Clamp(NowTime, 0, MaxTime);
            NowTime += 1 * Time.deltaTime;

            if(NowTime >= MaxTime) {
                Step = 3;
            }
        }
        if (GameFinish) {
            if(TrainAudio.volume > 0) {
                TrainAudio.volume -= (TrainAudio.volume / 10);
                if(TrainAudio.volume <= 0) {
                    TrainAudio.volume = 0;
                }
            }
        }

        if (JumpTextNow) {
            JumpText.SetActive(true);
        } else {
            JumpText.SetActive(false);
        }


        TotalPoint = TotalPlusPoint;
        PointCount.text = FontManager.DotNumberFont(TotalPoint.ToString());
        CargoCount.text = FontManager.DotNumberFont((MaxCargo - NowCargo).ToString());

        if (TrainAudio.time >= TrainMove.length - 0.2f) {
            TrainAudio.time = 0.2f;
            TrainAudio.Play();
        }

        if (BGM.time >= BGMSound.length - 3f) {
            BGM.time = 0f;
            BGM.Play();
        }
    }

    void ResultOn() {
        Result.SetActive(true);
    }
}
