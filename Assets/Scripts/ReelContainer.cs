using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BounceEasing
{
    EaseInBack,
    EaseInBounce,
    EaseInOutBack,
    EaseOutBack,
    EaseOutBounce,
}
public class ReelContainer : MonoBehaviour
{
    public Reel[] reels;
    public BounceEasing easing;
    public LineRenderer lineRenderer;
    public int[] stopPoints;
    public int lineWin;
    public int winKind;
    private List<Vector3> points;
    private int privateWinLine;
    private int privateWinKind;
    private bool winFlag = false;

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.positionCount = 0;
        points = new List<Vector3>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            lineRenderer.enabled = false;
            if (!CheckReelSpinning())
            {
                if (lineWin > 2 || lineWin < 0)
                {
                    lineWin = 0;
                }
                if (winKind > 4 || winKind < 0)
                {
                    winKind = 3;
                }
                privateWinLine = lineWin;
                StateManagement.Instance.ChangeState(GameState.START);
                winFlag = false;
                StartReels();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckReelSpinning())
            {
                privateWinLine = lineWin + 3;
                //lineRenderer.positionCount = winKind;

                if (privateWinLine > 5 || privateWinLine < 3)
                {
                    privateWinLine = 3;
                }
                if (winKind > 4 || winKind < 0)
                {
                    winKind = 3;
                }

                winFlag = false;
                StateManagement.Instance.ChangeState(GameState.STOP);
                StateManagement.Instance.ClearStopReel();
                StopReels();
                StateManagement.Instance.ChangeState(GameState.WIN);
            }
        }
        if (StateManagement.Instance.GetState() == GameState.WIN && winFlag == false)
        {
            winFlag = true;
            CheckWinDisplay();
        }
    }

    private void CheckWinDisplay()
    {
        DrawPath();
        for (int i = 0; i < winKind; i++)
        {
            reels[i].PlayWinScaling(privateWinLine);
        }

    }

    private void DrawPath()
    {
        lineRenderer.enabled = true;
        points.Clear();
        lineRenderer.positionCount = winKind;
        Vector3 point = Vector3.zero;
        for (int i = 0; i < winKind; i++)
        {
            point = reels[i].ReelSlot[privateWinLine].transform.position;
            lineRenderer.SetPosition(i, point);
            points.Add(point);
        }
    }
    private bool CheckReelSpinning()
    {
        bool retVal = false;
        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i].isReelSpinning())
            {
                retVal = true;
                break;
            }
        }
        return retVal;
    }
    private void StartReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StartReelSpin();
            reels[i].SetEasing((int)easing);
            reels[i].SetStopPoint(stopPoints[i]);
        }
    }

    private void StopReels()
    {
        for (int i = 0; i < reels.Length; i++)
        {
            reels[i].StopReelSpin();
        }
    }
}
