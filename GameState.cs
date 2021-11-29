using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{

    private bool restartKey = false;
    private bool restartGame = false;
    private bool Win = false;
    private bool lose = false;
    public YouWinScreen WinMessage;
    public YouWinScreen LoseMessage;

    public void GameWon(){
        Debug.Log("Game Complete");
        Win = true;
        Restart();
        if (lose == false){
            WinMessage.DisplayScreen();
            FindObjectOfType<AudioManager>().Play("YouWin");
        }
    }

    public void GameOver(){
        Debug.Log("Game Over");
        lose = true;
        Restart();
        if (Win == false){
            LoseMessage.DisplayScreen();
        }
    }

    private void Restart(){
        restartGame = true;
    }

    void Update(){
        if (Input.GetKeyDown("r")){
            restartKey = true;
        }
        else{
            restartKey = false;
        }

        if ((restartKey == true) && (restartGame == true)){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Awake(){
        restartKey = false;
        restartGame = false;
    }
}
