using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    
    // Use this for initialization
    void Start()
    {

    }

    public void Play() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("gameplay");
    }

    public void Credits() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("credits");
    }
}
