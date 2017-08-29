using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /*
       Player Mechanism

       Se encargara de gestionar los estados de animacion del personaje. 
       Y de los metodos en caso de que el jugador sea tocado por el enemigo.
    */

    // UI

    public SpriteRenderer playerRender;
    public ParticleSystem dust;

    // vidas del jugador
    public GameObject heart_1GO;
    public GameObject heart_2GO;

    public SpriteRenderer heart_1Render;
    public SpriteRenderer heart_2Render;

    public Sprite[] hearts_1;
    public Sprite[] hearts_2;

    public Animator anim;

    // Audio

    private AudioSource audioPlayer;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointsClip;

    // Script

    public EnemyGeneratorController enemyGeneratorScript;

    // GameObject

    public GameObject game;

    // Normales

    int x = 0;
    int y = 0;
    private float startY;

    void Start()
    {

        anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;

    }


    void Update()
    {
        //Nos permitira que el judador no pueda saltar una vez muerto
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        //Este tocando el suelo
        bool isGrounend = transform.position.y == startY;
        // Adejunta las 3 condiciones anteriores en una sola
        bool jumpCheck = gamePlaying && userAction && isGrounend;

        if (jumpCheck)
        {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy"){
			InjuredPlayer();
        }else if(collision.gameObject.tag == "Point"){
            PlayerPoints();
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Ya no me esta tocando");
            playerRender.color = Color.white;

        }


    }


    public void UpdateState(string state = null)
    {

        if (state != null)
        {
            anim.Play(state);
        }

    }




    private void InjuredPlayer()
    {
            
            Debug.Log("Me toco");
            playerRender.color = Color.red;
            ArrayPadlock(x);
    }


    private void LifeVerication(int check)
    {
        // De que el el jugador haya perdido su primer corazon
        if (check == 2)
        {
            
            heart_1GO.SetActive(false);
            heart_2Render.sprite = hearts_2[y];
            y++;
            FinalCheck(y);
        }

    }

    private void FinalCheck(int check)
    {
        if (check == 3)
        {
            // Que el jugador pierda su ultmia vida
            heart_2GO.SetActive(false);
			game.GetComponent<AudioSource>().Stop();
			UpdateState("PlayerDie");
			audioPlayer.clip = dieClip;
			audioPlayer.Play();
			game.GetComponent<GameController>().gameState = GameState.Ended;
			enemyGeneratorScript.SendMessage("StopGenerator", true);
            game.SendMessage("ResertTimeScale", 0.5f);
            DustStop();
        }
    }

    private void ArrayPadlock(int check) { 

        // Esto se encarga de que el arreglo, no se salga de su indice.
        if(check < 2) { 
            heart_1Render.sprite = hearts_1[check];
            x++;
        }

        LifeVerication(x);
        
    }

    private void GameReady() {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    private void DustPlay() {
        dust.Play();
    }

    private void DustStop() {
        dust.Stop();
    }

   private void PlayerPoints() {
        
        game.SendMessage("IncreasPoints");
        audioPlayer.clip = pointsClip;
        audioPlayer.Play();
    }





}




            