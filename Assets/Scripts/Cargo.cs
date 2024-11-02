using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour {
    public int CargoType; // 1 = ThrowCargo, 2 = Bird, 3 = TNT
    private int BirdType;

    public float[] CargoSpeed;

    private bool Throw = false;
    private bool Dump = false;

    public Sprite[] sprites;
    public SpriteRenderer Skin;

    public Sprite[] BirdParts;
    public Sprite[] CargoParts;

    PlayerMove player;
    GamePlay game;
    Animator anim;
    public Transform SkinPos;

    AudioSource SE;
    public AudioClip ThrowSE;
    public AudioClip TNTFailSE;
    public AudioClip CargoFailSE;
    public AudioClip BirdFailSE1;
    public AudioClip BirdFailSE2;

    public BoxCollider2D myCollider;

    void Start()
    {
        anim = GetComponent<Animator>();

        player = GameObject.Find("Chara1").GetComponent<PlayerMove>();
        game = GameObject.Find("GameManager").GetComponent<GamePlay>();
        SE = GetComponent<AudioSource>();

        CargoType = Random.Range(1, 4);

        if(CargoType != 2) {
            Skin.sprite = sprites[CargoType - 1];
        } else {
            BirdType = Random.Range(1, 4);
            Skin.sprite = BirdParts[BirdType - 1];
            Vector3 pos = SkinPos.localScale;
            pos.x = -1;
            SkinPos.localScale = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Dump && !Throw) {
            switch (player.Status) {
                case "Wait":
                    Skin.enabled = true;
                    anim.Play("Idle");
                    anim.SetInteger("Throw", 0);
                    anim.SetBool("Dump", false);
                    break;

                case "ThrowWait":
                    Skin.enabled = true;
                    anim.Play("Ready");
                    anim.SetInteger("Throw", 0);
                    anim.SetBool("Dump", false);
                    break;

                case "Throw":
                    anim.speed = CargoSpeed[CargoType - 1];
                    if (CargoType == 1) {
                        anim.SetInteger("Throw", 1);
                    }
                    if (CargoType == 2) {
                        anim.SetInteger("Throw", 2);
                        Skin.sprite = BirdParts[(BirdType - 1) + 3];
                    }
                    if (CargoType == 3) {
                        anim.SetInteger("Throw", 1);
                        game.TotalMinusPoint += game.MinusPoint;
                    }
                    myCollider.enabled = true;
                    Throw = true;
                    break;

                case "Squat":
                    Skin.enabled = false;
                    break;

                case "Dump":
                    Skin.enabled = true;
                    SE.PlayOneShot(ThrowSE);
                    anim.speed = 1;
                    anim.SetBool("Dump", true);
                    Dump = true;
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Obstacle")) {
            Debug.Log("Hit");
            switch (CargoType) {
                case 1:
                    anim.Play("HitCargo");
                    SE.PlayOneShot(CargoFailSE);
                    Skin.sortingOrder = 2;
                    Skin.sprite = CargoParts[0];
                    game.TotalMinusPoint += game.MinusPoint;
                    Invoke("Destroy", 0.5f);
                    break;
                case 2:
                    anim.Play("HitBird");
                    SE.PlayOneShot(BirdFailSE1);
                    Skin.sortingOrder = 2;
                    Skin.sprite = BirdParts[(BirdType - 1) + 6];
                    game.TotalMinusPoint += game.MinusPoint;
                    Invoke("Destroy", 1f);
                    break;
                case 3:
                    Skin.sortingOrder = 2;
                    Skin.sprite = CargoParts[1];
                    SE.PlayOneShot(TNTFailSE);
                    myCollider.enabled = false;
                    anim.enabled = false;
                    Invoke("Destroy", TNTFailSE.length * 0.5f);
                    break;
            }
        }
    }

    void CargoArrival() {
        switch (CargoType) {
            case 1:
                Debug.Log("í èÌî†Å@ìûíÖ");
                GameObject.FindWithTag("Catcher").GetComponent<Catcher>().Catch();
                game.TotalPlusPoint += game.PlusPoint;
                Destroy(this.gameObject);
                break;
            case 2:
                Debug.Log("íπÅ@ìûíÖ");
                GameObject.FindWithTag("Catcher").GetComponent<Catcher>().Catch();
                game.TotalPlusPoint += game.PlusPoint;
                Destroy(this.gameObject);
                break;
            case 3:
                Skin.sprite = CargoParts[1];
                SE.PlayOneShot(TNTFailSE);
                player.isMove = false;
                game.Clear = false;
                game.Step = 3;
                break;
        }
    }

    void CargoDump() {
        switch (CargoType) {
            case 1:
                Debug.Log("í èÌî†Å@îpä¸");
                game.TotalMinusPoint += game.MinusPoint;
                Destroy(this.gameObject);
                break;
            case 2:
                Debug.Log("íπÅ@îpä¸");
                game.TotalMinusPoint += game.MinusPoint;
                Destroy(this.gameObject);
                break;
            case 3:
                Debug.Log("TNTÅ@îpä¸");
                game.TotalPlusPoint += game.PlusPoint;
                Destroy(this.gameObject);
                break;
        }
    }

    void ThrowNow() {
        SE.PlayOneShot(ThrowSE);
    }

    void HitBirdSE() {
        SE.PlayOneShot(BirdFailSE2);
    }

    private void Destroy() {
        Destroy(this.gameObject);
    }
}
