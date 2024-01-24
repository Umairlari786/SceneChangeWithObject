using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public GameObject StartGameObject;
    public GameObject MainGameObject;
    public GameObject FreeGameObject;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void LoadMainGame()
    {
        StartGameObject.SetActive(false);
        MainGameObject.SetActive(true);
        FreeGameObject.SetActive(false);
    }
    public void LoadFreeGame()
    {
        StartGameObject.SetActive(false);
        MainGameObject.SetActive(false);
        FreeGameObject.SetActive(true);
    }
    public void LoadChangeMode()
    {
        StartGameObject.SetActive(true);
        MainGameObject.SetActive(false);
        FreeGameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
