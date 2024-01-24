using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinManagement : MonoBehaviour
{
    public StateManagement stateManagement;
    private bool winFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        winFlag = false;
        stateManagement = GetComponent<StateManagement>();
    }
    public void PlayWin()
    {
        winFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (stateManagement.GetState() == GameState.WIN && winFlag == false)
        {
            winFlag = true;
            PlayWin();
        }

    }
}
