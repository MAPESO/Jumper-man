using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready };


public class GameController : MonoBehaviour {

	/* 
	   Mechanism of video game
	   
       Se encarga de getionar el estado global del juego, como de producir el efecto parallax.
     */

	// Scripts

	public EnemyGeneratorController enemyGeneratorScript;

	//UI

	public RawImage platform;
	public RawImage bg;
    public Text pointsText;
    public Text recordText;

    // GameObject

    public GameObject uiScore;
	public GameObject uiIdleGO;
    public GameObject heartGO;
    public GameObject heart2_GO;
    public GameObject player;


	// Normal data type

	public float parallaxSpeed = 0.05f;
    public GameState gameState = GameState.Idle;
    private int points = 0;

    // Aumento de dificultad 

    public float scaleTime = 6f;
    public float scaleInc = .25f; 

    // Audio

    private AudioSource musicPlayer;


    void Start () {

		heartGO.SetActive(false);
        heart2_GO.SetActive(false);
        uiScore.SetActive(false);
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
	}


    void ParallaxEffect() {
        
		float finalSpeed = parallaxSpeed * Time.deltaTime; //para que sea una velocidad constante para cualquier ordenador.

        bg.uvRect = new Rect( /* posicion "x" */ bg.uvRect.x + (finalSpeed), 0f, 1f, 1f);
        platform.uvRect = new Rect( /* posicion "x" */ platform.uvRect.x + (finalSpeed * 4), 0f, 1f, 1f);

	}


	void Update () {

        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);

        // Para que el juego comience el estado debe de ser "parado", posterior a eso le daremos click o precionaremos para que comience.

        if (gameState == GameState.Idle && userAction) {

			gameState = GameState.Playing;

            musicPlayer.Play();

			heartGO.SetActive(true);
            heart2_GO.SetActive(true);
			uiIdleGO.SetActive(false);
            uiScore.SetActive(true);

            player.SendMessage("UpdateState", "PlayerRun");

            player.SendMessage("DustPlay");

            enemyGeneratorScript.SendMessage("StartGenerator");

            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);

            // Juego activado
        }else if(gameState == GameState.Playing) {

			ParallaxEffect();

            // Preparando para reiniciar
        }else if(gameState == GameState.Ready) {

            if(userAction) {
                RestartGame();
            }
            
        }


	}

    public void RestartGame() {

        ResertTimeScale();

        SceneManager.LoadScene("Game");   
    }

    void GameTimeScale() {
        //timeScale por defecto su valor es de 1
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incrementado: " + Time.timeScale.ToString());
    }

    public void ResertTimeScale(float newTimeScale = 1f) {
        CancelInvoke("GameTimeScale");

        Time.timeScale = newTimeScale;

        Debug.Log("ritmo restablecido: " + Time.timeScale.ToString());
    }

    public void IncreasPoints() {
        points++;
        pointsText.text = points.ToString();
        // Record points
        if(points >= GetMaxScore()) {
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore() {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints) {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }

   
}
