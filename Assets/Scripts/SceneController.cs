using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void LoadFreeGame()
    {
        SceneManager.LoadScene("FreeGame");
    }
    public void LoadChangeMode()
    {
        SceneManager.LoadScene("StartScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
