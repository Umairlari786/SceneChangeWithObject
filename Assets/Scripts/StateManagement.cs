using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    IDLE,
    START,
    RUNNING,
    STOP,
    WIN
}

public class StateManagement : MonoBehaviour
{
    public static StateManagement Instance { get; private set; }
    private GameState currentState;
    private List<bool> ReelState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.IDLE;
        ReelState = new List<bool>();
    }

    public void ChangeState(GameState state)
    {
        if (currentState != state)
        {
            currentState = state;
        }
    }

    public GameState GetState()
    {
        return currentState;
    }

    public void SetStopReel()
    {
        ReelState.Add(true);
        if (ReelState.Count == 5)
        {
            ChangeState(GameState.STOP);
            ReelState.Clear();
            ChangeState(GameState.WIN);
        }

    }

    public void ClearStopReel()
    {
        ReelState.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
