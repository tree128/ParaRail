using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject Cargo;
    public GameObject BB;

    public string Status = "Wait";
    public bool isMove;

    public float ThrowRecast;
    public float NowRecast = 0;

    Animator anim;
    public GamePlay game;

    AudioSource SE;
    public AudioClip GameOverSE;
    public AudioClip JumpFinSE;

    public bool IsPlaying = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        SE = GetComponent<AudioSource>();
        BB.SetActive(false);
    }

    void Update() {
        if (!IsPlaying) return;
        if (isMove) {
            switch (Status) {
                case "Wait":
                    if (!game.PlayerJump) {
                        if (Input.GetKey(KeyCode.D) && NowRecast <= 0) {
                            Status = "ThrowWait";
                            anim.SetInteger("Status", 1);
                        }
                        if (Input.GetKey(KeyCode.S)) {
                            Status = "Squat";
                            anim.SetInteger("Status", 3);
                        }
                        if (Input.GetKeyDown(KeyCode.A) && NowRecast <= 0) {
                            Status = "Dump";
                            anim.SetInteger("Status", 4);
                            NowRecast = ThrowRecast;
                        }
                    } else {
                        if (Input.GetKey(KeyCode.D)) {
                            Status = "JumpWait";
                            anim.SetInteger("Jump", 1);
                        }
                        if (Input.GetKey(KeyCode.S)) {
                            Status = "JumpSquat";
                            anim.SetInteger("Jump", 3);
                        }
                        anim.Play("Idle");
                    }
                    break;

                case "ThrowWait":
                    if (Input.GetKeyUp(KeyCode.D) && NowRecast <= 0) {
                        Status = "Throw";
                        anim.SetInteger("Status", 2);
                        NowRecast = ThrowRecast;
                    }
                    if (Input.GetKeyDown(KeyCode.S)) {
                        Status = "Squat";
                        anim.SetInteger("Status", 3);
                    }
                    if (Input.GetKeyDown(KeyCode.A) && NowRecast <= 0) {
                        Status = "Dump";
                        anim.SetInteger("Status", 4);
                        NowRecast = ThrowRecast;
                    }
                    break;

                case "Throw":
                    if (Input.GetKey(KeyCode.S)) {
                        Status = "Squat";
                        anim.SetInteger("Status", 3);
                    } else {
                        anim.SetInteger("Status", 0);
                        Status = "Wait";
                    }
                    break;

                case "Squat":
                    if (!game.PlayerJump) {
                        if (Input.GetKeyUp(KeyCode.S)) {
                            if (Input.GetKey(KeyCode.D)) {
                                Status = "ThrowWait";
                                anim.SetInteger("Status", 1);
                            } else {
                                Status = "Wait";
                                anim.SetInteger("Status", 0);
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.A)) {
                            Status = "Dump";
                            anim.SetInteger("Status", 4);
                            NowRecast = ThrowRecast;
                        }
                    } else {
                        if (Input.GetKeyUp(KeyCode.S)) {
                            if (Input.GetKey(KeyCode.D)) {
                                Status = "JumpWait";
                            } else {
                                Status = "Wait";
                            }
                        }
                    }
                    break;

                case "Dump":
                    if (!game.PlayerJump) {
                        if (Input.GetKey(KeyCode.S)) {
                            Status = "Squat";
                            anim.SetInteger("Status", 3);
                        } else if (Input.GetKey(KeyCode.D) && NowRecast <= 0) {
                            Status = "ThrowWait";
                            anim.SetInteger("Status", 1);
                        } else {
                            Status = "Wait";
                            anim.SetInteger("Status", 0);
                        }
                    } else {
                        if (Input.GetKey(KeyCode.S)) {
                            Status = "JumpSquat";
                        } else if (Input.GetKey(KeyCode.D)) {
                            Status = "JumpWait";
                        } else {
                            Status = "Wait";
                        }
                    }
                    break;


                case "JumpWait":
                    if (Input.GetKeyUp(KeyCode.D)) {
                        Status = "Jump";
                        anim.SetInteger("Jump", 2);
                    }
                    if (Input.GetKey(KeyCode.S)) {
                        Status = "JumpSquat";
                        anim.SetInteger("Jump", 3);
                    }
                    if (Input.GetKey(KeyCode.A)) {
                        Status = "Wait";
                        anim.SetInteger("Jump", 0);
                    }
                    break;

                case "Jump":
                    game.JumpTextNow = false;
                    break;

                case "JumpSquat":
                    if (Input.GetKeyUp(KeyCode.S)) {
                        if (Input.GetKey(KeyCode.D)) {
                            Status = "JumpWait";
                            anim.SetInteger("Jump", 1);
                        } else {
                            Status = "Wait";
                            anim.SetInteger("Jump", 0);
                        }
                    }
                    break;
            }
        }

        if(NowRecast > 0) {
            NowRecast -= 1 * Time.deltaTime;
            if(NowRecast <= 0) {
                NowRecast = 0;
                game.NextCargo = true;
            }
        }

        if (!isMove) {
            anim.speed = 0f;
        }

        if (game.Tonnel)
        {
            if (Status != "Squat" && Status != "JumpSquat")
            {
                anim.Play("JumpFail");
                /*
                BB.SetActive(true);
                Time.timeScale = 1f;
                GameOver();
                game.Tonnel = false;
                */
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Obstacle")) {
            if (Status == "Jump") {
                anim.Play("JumpFail");
                Time.timeScale = 1f;
            }
        }
    }

    void TimeStart() {
        Time.timeScale = 0.1f;
    }
    void TimeFinish() {
        Time.timeScale = 1f;
    }

    void Clear() {
        SE.PlayOneShot(JumpFinSE);
        game.Clear = true;
        game.Step = 3;
    }

    void GameOver() {
        game.Clear = false;
        //SE.PlayOneShot(GameOverSE);
        isMove = false;
        game.Step = 3;
        if (game.Tonnel)
        {
            BB.SetActive(true);
            Time.timeScale = 1f;
            game.Tonnel = false;
        }
    }

    public void Play_GameOverSE()
    {
        SE.PlayOneShot(GameOverSE);
    }
}
