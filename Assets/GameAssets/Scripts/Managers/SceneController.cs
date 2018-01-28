using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    
    
    // Use this for initialization
    void Start()
    {

    }

    public void Play() {
        SceneManager.LoadScene("character_selection");
    }

    public void Credits() {
        SceneManager.LoadScene("credits");
    }

    public void Configuration() {
        SceneManager.LoadScene("configuration");
    }

    public void Back() {
        SceneManager.LoadScene("menu");
    }
}
