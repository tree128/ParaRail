using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    public int Step;
    private bool CountStart = false;
    private int NowCount = 0;
    private int TotalPoint;

    public GamePlay game;

    public GameObject resultText;
    public GameObject plus;
    public GameObject minus;
    public GameObject chara;
    public GameObject line;
    public GameObject total;

    public TextMeshProUGUI plusText;
    public TextMeshProUGUI minusText;
    public TextMeshProUGUI charaText;
    public TextMeshProUGUI totalText;

    Animator anim;
    AudioSource SE;
    public AudioClip SESound;

    // Start is called before the first frame update
    void Start()
    {
        Step = 0;
        anim = GetComponent<Animator>();
        SE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.GameFinish) {
            switch (Step) {
                case 0:
                    resultText.SetActive(false);
                    plus.SetActive(false);
                    minus.SetActive(false);
                    chara.SetActive(false);
                    line.SetActive(false);
                    total.SetActive(false);
                    break;
                case 1:
                    anim.Play("ResultTextOn");
                    resultText.SetActive(true);
                    break;
                case 2:
                    anim.Play("PlusOn");
                    plus.SetActive(true);
                    break;
                case 3:
                    plusText.text = FontManager.DotNumberFont(game.TotalPlusPoint.ToString());
                    anim.Play("CharaOn");
                    chara.SetActive(true);
                    break;
                case 4:
                    if (game.Clear) {
                        charaText.text = FontManager.DotNumberFont(game.ClearPoint.ToString());
                    } else {
                        charaText.text = FontManager.DotNumberFont("0");
                    }
                    anim.Play("MinusOn");
                    minus.SetActive(true);
                    break;
                case 5:
                    minusText.text = FontManager.DotNumberFont(game.TotalMinusPoint.ToString());
                    anim.Play("LineOn");
                    line.SetActive(true);
                    break;
                case 6:
                    anim.Play("TotalOn");
                    total.SetActive(true);
                    break;
                case 7:
                    if (game.Clear) {
                        TotalPoint = (game.TotalPlusPoint * game.ClearPoint) - game.TotalMinusPoint;
                        if (TotalPoint == 0) {
                            totalText.text = FontManager.DotNumberFont("0");
                        } else {
                            totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                        }
                    } else {
                        TotalPoint = game.TotalPlusPoint - game.TotalMinusPoint;
                        if (TotalPoint == 0) {
                            totalText.text = FontManager.DotNumberFont("0");
                        } else {
                            totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                        }
                    }

                    Invoke("BackHome", 1f);
                    Step = 8;
                    break;

                case 9:
                    if (Input.GetKeyDown(KeyCode.Space)) {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    }
                    break;
            }

            if (Input.anyKeyDown) {
                if (Step < 7) {
                    Step += 1;
                    CountStart = false;
                    if (Step == 3 || Step == 4 || Step == 5 || Step == 7) {
                        SE.PlayOneShot(SESound);
                    }
                }
            }

            anim.SetInteger("Step", Step);

            if (CountStart) {
                switch (Step) {
                    case 2:
                        if (NowCount < game.TotalPlusPoint) {
                            NowCount += 1;
                            plusText.text = FontManager.DotNumberFont(NowCount.ToString());
                            if (NowCount >= game.TotalPlusPoint) {
                                plusText.text = FontManager.DotNumberFont(game.TotalPlusPoint.ToString());
                                SE.PlayOneShot(SESound);
                                Step += 1;
                                CountStart = false;
                            }
                        }
                        if (game.TotalPlusPoint == 0) {
                            plusText.text = FontManager.DotNumberFont("0");
                            SE.PlayOneShot(SESound);
                            Step += 1;
                            CountStart = false;
                        }
                        break;
                    case 4:
                        if (NowCount < game.TotalMinusPoint) {
                            NowCount += 1;
                            minusText.text = FontManager.DotNumberFont(NowCount.ToString());
                            if (NowCount >= game.TotalMinusPoint) {
                                minusText.text = FontManager.DotNumberFont(game.TotalMinusPoint.ToString());
                                SE.PlayOneShot(SESound);
                                Step += 1;
                                CountStart = false;
                            }
                        }
                        if (game.TotalMinusPoint == 0) {
                            minusText.text = FontManager.DotNumberFont("0");
                            SE.PlayOneShot(SESound);
                            Step += 1;
                            CountStart = false;
                        }
                        break;
                    case 6:
                        if (game.Clear) {
                            TotalPoint = (game.TotalPlusPoint * game.ClearPoint) - game.TotalMinusPoint;
                            if (TotalPoint > 0) {
                                if (NowCount < TotalPoint) {
                                    NowCount += 1;
                                    totalText.text = FontManager.DotNumberFont(NowCount.ToString());
                                    if (NowCount >= TotalPoint) {
                                        totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                                        SE.PlayOneShot(SESound);
                                        Step += 1;
                                        CountStart = false;
                                    }
                                }
                            } else if(TotalPoint < 0) {
                                if (NowCount > TotalPoint) {
                                    NowCount -= 1;
                                    totalText.text = FontManager.DotNumberFont(NowCount.ToString());
                                    if (NowCount <= TotalPoint) {
                                        totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                                        SE.PlayOneShot(SESound);
                                        Step += 1;
                                        CountStart = false;
                                    }
                                }
                            } else {
                                totalText.text = FontManager.DotNumberFont("0");
                                SE.PlayOneShot(SESound);
                                Step += 1;
                                CountStart = false;
                            }
                            
                        } else {
                            TotalPoint = game.TotalPlusPoint - game.TotalMinusPoint;
                            if (TotalPoint > 0) {
                                if (NowCount < TotalPoint) {
                                    NowCount += 1;
                                    totalText.text = FontManager.DotNumberFont(NowCount.ToString());
                                    if (NowCount >= TotalPoint) {
                                        totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                                        SE.PlayOneShot(SESound);
                                        Step += 1;
                                        CountStart = false;
                                    }
                                }
                            } else if (TotalPoint < 0) {
                                if (NowCount > TotalPoint) {
                                    NowCount -= 1;
                                    totalText.text = FontManager.DotNumberFont(NowCount.ToString());
                                    if (NowCount <= TotalPoint) {
                                        totalText.text = FontManager.DotNumberFont(TotalPoint.ToString());
                                        SE.PlayOneShot(SESound);
                                        Step += 1;
                                        CountStart = false;
                                    }
                                }
                            } else {
                                totalText.text = FontManager.DotNumberFont("0");
                                SE.PlayOneShot(SESound);
                                Step += 1;
                                CountStart = false;
                            }
                        }
                        break;
                }
            }
        }
    }

    void Next() {
        Step += 1;
    }
    
    void Count() {
        switch (Step) {
            case 2:
                CountStart = true;
                plusText.text = FontManager.DotNumberFont("0");
                NowCount = 0;
                break;
            case 4:
                CountStart = true;
                minusText.text = FontManager.DotNumberFont("0");
                NowCount = 0;
                break;
            case 3:
                if (game.Clear) {
                    charaText.text = FontManager.DotNumberFont(game.ClearPoint.ToString());
                } else {
                    charaText.text = FontManager.DotNumberFont("0");
                }
                SE.PlayOneShot(SESound);
                Step += 1;
                break;
            case 6:
                CountStart = true;
                totalText.text = FontManager.DotNumberFont("0");
                NowCount = 0;
                break;
        }
    }

    void BackHome() {
        anim.Play("ResultFinish");
        Step = 9;
    }
}
