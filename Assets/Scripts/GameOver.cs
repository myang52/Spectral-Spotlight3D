using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
 public void Retry() {

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);


 }





    public void QuitGame(){
            Debug.Log ("You've quit.");
            Application.Quit();
    }
}
