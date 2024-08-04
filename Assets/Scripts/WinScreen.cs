using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    // Start is called before the first frame update
 public void Retry() {

    SceneManager.LoadScene("Game");


 }





    public void ToMenu(){
     SceneManager.LoadScene("Menu");
    }
}
