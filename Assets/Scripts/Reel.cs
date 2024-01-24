using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;


public class Reel : MonoBehaviour
{
    public Symbol ReelSymbols;
    //[SerializeField]
    //StateManagement stateManagement;
    public int[] ReelArray = new int[100];
    public float bounceTime = 0.2f;
    private float currentSpeed = 15.0f;
    public GameObject[] ReelSlot;
    private int counter = 0;
    bool startFlag = false;
    float bounceBackTime = 0.8f;
    public int easeinVal = 0;
    private List<Vector3> transPos;
    private List<Vector3> targetPos;
    private List<Vector3> targetScale;
    private Vector3 scaleChange;
    private Coroutine refStartSpin;
    private int stopPoint;
    private int myEasing;
    private bool revDirection = false;
    //private float ReelDuration = 4.0f;

    private float spinStartTime;
    private List<bool> myStop = new List<bool>();

    [SerializeField]
    private float spinTime = 3f;

    [SerializeField]
    private float startTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //stateManagement = GetComponent<StateManagement>();
        transPos = new List<Vector3>();
        targetPos = new List<Vector3>();
        targetScale = new List<Vector3>();
        scaleChange = new Vector3(0.001f, 0.001f, 0.0f);
        for (int j = 0; j < ReelArray.Length; j++)
        {
            ReelArray[j] = UnityEngine.Random.Range(0, 8);
        }
        transPos.Clear();
        targetPos.Clear();
        targetScale.Clear();
        myStop.Clear();
        for (int j = 0; j < 9; j++)
        {
            myStop.Add(false);
            int pointIndex = j - 3;
            if (pointIndex < 0)
            {
                pointIndex = ReelArray.Length + pointIndex;
            }
            pointIndex = pointIndex % ReelArray.Length;
            int tempIndex = 0;
            if (ReelArray.Length == 0)
            {
                tempIndex = 0;
            }
            else if (ReelArray.Length >= 9)
            {
                tempIndex = ReelArray[pointIndex];
            }
            else if (ReelArray.Length > 0 && ReelArray.Length < 9)
            {
                if (j >= ReelArray.Length)
                {
                    tempIndex = ReelArray[ReelArray.Length - 1];
                }
                else
                {
                    tempIndex = ReelArray[pointIndex];
                }
            }
            else
            {
                tempIndex = 0;
            }
            Sprite sprite = ReelSymbols.Symbols[tempIndex];
            ReelSlot[j].GetComponent<SpriteRenderer>().sprite = sprite;

            transPos.Add(ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition);
            targetPos.Add(ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition);
            targetScale.Add(ReelSlot[j].GetComponent<SpriteRenderer>().transform.localScale);
        }
    }
    float EvaluateBounce(float t)
    {
        switch (myEasing)
        {
            case 0:
                return EaseInBack(t);
            case 1:
                return EaseInBounce(t);
            case 2:
                return EaseInOutBack(t);
            case 3:
                return EaseOutBack(t);
            default:
                return EaseInBack(t);
        }
    }


    float EaseInBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return c3 * t * t * t - c1 * t * t;
    }

    float EaseInOutBack(float t)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;
        return t < 0.5
          ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
          : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }

    float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
    }

    float EaseInBounce(float t)
    {
        return 1 - EaseOutBounce(1 - t);
    }

    float EaseOutBounce(float t)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (t < 1 / d1)
            return n1 * t * t;
        else if (t < 2 / d1)
            return n1 * (t -= 1.5f / d1) * t + 0.75f;
        else if (t < 2.5 / d1)
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;
        else
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
    }

    public void SetReelDuration(float duration)
    {
        //ReelDuration = duration;
    }
    private void SetDynamicReel()
    {
        myStop.Clear();
        for (int j = 0; j < 9; j++)
        {
            myStop.Add(false);
            int pointIndex = j - 3;
            if (pointIndex < 0)
            {
                pointIndex = ReelArray.Length + pointIndex;
            }
            pointIndex = pointIndex % ReelArray.Length;
            int tempIndex = 0;
            if (ReelArray.Length == 0)
            {
                tempIndex = 0;
            }
            else if (ReelArray.Length >= 9)
            {
                tempIndex = ReelArray[pointIndex];
            }
            else if (ReelArray.Length > 0 && ReelArray.Length < 9)
            {
                if (j >= ReelArray.Length)
                {
                    tempIndex = ReelArray[ReelArray.Length - 1];
                }
                else
                {
                    tempIndex = ReelArray[pointIndex];
                }
            }
            else
            {
                tempIndex = 0;
            }
            Sprite sprite = ReelSymbols.Symbols[tempIndex];
            ReelSlot[j].GetComponent<SpriteRenderer>().sprite = sprite;
        }

    }
    private IEnumerator StartSpin()
    {
        startFlag = true;
        currentSpeed = 15.0f;
        float bounceHeight = 0.2f;
        float waitTime = 0;
        bounceTime = 0.2f;

        //SetDynamicReel();
        targetPos.Clear();
        for (int i = 0; i < ReelSlot.Length; i++)
        {
            targetPos.Add(ReelSlot[i].GetComponent<SpriteRenderer>().transform.localPosition);
        }
        while (waitTime <= bounceTime)
        {
            //valBet = waitTime / bounceTime;
            for (int i = 0; i < ReelSlot.Length; i++)
            {
                var moveTo = targetPos[i].y + (bounceHeight * EvaluateBounce(waitTime / bounceTime));
                Vector2 newPos = new Vector2(ReelSlot[i].transform.localPosition.x, moveTo);
                ReelSlot[i].transform.localPosition = newPos;



                /*

                float posNext = Mathf.Lerp(targetPos[i].y, targetPos[i].y + bounceHeight, valBet);
                ReelSlot[i].transform.localPosition = new Vector2(ReelSlot[i].transform.localPosition.x, posNext);//   Translate(0f, bounceHeight * waitTime / bounceTime * Time.deltaTime, 0f);
                */
            }
            yield return new WaitForEndOfFrame();
            waitTime += Time.deltaTime;
        }


        float spentTime = 0f;
        float finalTime = 0f;
        yield return new WaitForSeconds(startTime);
        finalTime = spinTime - bounceTime;// - bounceBackTime;
        // Loop Spin
        while (spentTime <= finalTime && startFlag == true)
        {
            for (int j = 0; j < 9; j++)
            {
                // set final symbols

                if ((spentTime + ((8 * 0.3240625) / currentSpeed)) > finalTime)
                {

                }
                ReelSlot[j].GetComponent<SpriteRenderer>().transform.Translate(currentSpeed * Time.deltaTime * Vector3.down);
                if (ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition.y < -1.3)
                {
                    float difference = ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition.y + 1.3f;
                    float finalPos = 1.5f + difference;
                    counter++;
                    if (counter == ReelArray.Length)
                    {
                        counter = 0;
                    }
                    ReelSlot[j].GetComponent<SpriteRenderer>().sprite = ReelSymbols.Symbols[ReelArray[ReelArray.Length - 1 - counter]];
                    ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition = new Vector2(0.03f, finalPos);
                }
            }
            spentTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (startFlag == true)
        {
            StartCoroutine(SetFinalValuesAndPositions());
        }
    }

    public void SetEasing(int easing)
    {
        myEasing = easing;
    }
    public IEnumerator BounceReelsInitial()
    {
        float waitTime = 6;
        while (--waitTime != 0)
        {
            for (int i = 0; i < ReelSlot.Length; i++)
            {

                ReelSlot[i].transform.Translate(0f, (20f) * Time.deltaTime, 0f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator BounceReelsEnd()
    {
        float waitTime = 6;
        while (--waitTime != 0)
        {
            for (int i = 0; i < ReelSlot.Length; i++)
            {
                yield return new WaitForEndOfFrame();
                ReelSlot[i].transform.Translate(0f, (-20f) * Time.deltaTime, 0f);
            }
        }
    }
    private IEnumerator SetFinalValuesAndPositions()
    {
        SetFinalPositions();
        SetFinalValues();
        currentSpeed = 5.0f;

        while ((ReelSlot[5].GetComponent<SpriteRenderer>().transform.localPosition.y > -1.3) && (startFlag == true))
        {
            for (int j = 0; j < 9; j++)
            {
                ReelSlot[j].GetComponent<SpriteRenderer>().transform.Translate(currentSpeed * Time.deltaTime * Vector3.down);
            }
            yield return new WaitForEndOfFrame();
        }

        //        stateManagement.SetStopReel();
        StateManagement.Instance.SetStopReel();
        startFlag = false;
    }
    public void SetStopPoint(int point)
    {
        stopPoint = point;
        if (stopPoint < 0)
        {
            stopPoint = ReelArray.Length + stopPoint;
        }
        stopPoint = stopPoint % ReelArray.Length;

    }
    private void SetSpriteValue(int index)
    {
        ReelSlot[index].GetComponent<SpriteRenderer>().sprite = ReelSymbols.Symbols[ReelArray[stopPoint]];
        stopPoint++;
        stopPoint = stopPoint % ReelArray.Length;
    }
    private void SetFinalValues()
    {
        for (int j = 0; j < 9; j++)
        {
            ReelSlot[j].GetComponent<SpriteRenderer>().sprite = ReelSymbols.Symbols[ReelArray[stopPoint]];
            stopPoint++;
            stopPoint = stopPoint % ReelArray.Length;
        }
    }

    private void SetFinalPositions()
    {
        for (int j = 0; j < 9; j++)
        {
            ReelSlot[j].GetComponent<SpriteRenderer>().transform.localPosition = transPos[j];
        }
    }
    private IEnumerator StartAccelaration()
    {
        currentSpeed = -0.009f;
        yield return new WaitForSeconds(1);
        //ChangeState(States.STATE_SLOW);
    }

    private IEnumerator StartSlow()
    {
        yield return null;
    }

    private IEnumerator StartStop()
    {
        yield return null;
    }

    private IEnumerator StartIdle()
    {
        currentSpeed = 0.0f;
        yield return null;
    }
    /*
    private void ChangeState(States cState)
    {
        myState = cState;
    }*/

    public void StartReelSpin()
    {
        refStartSpin = StartCoroutine(StartSpin());
    }
    public bool isReelSpinning()
    {
        return startFlag;
    }
    public void StopReelSpin()
    {
        startFlag = false;
        stopPoint = stopPoint - 3;
        if (stopPoint < 0)
        {
            stopPoint = ReelArray.Length + stopPoint;
        }
        stopPoint = stopPoint % ReelArray.Length;
        SetFinalValues();
        SetFinalPositions();

        //StopCoroutine(refStartSpin);
    }
    public void PlayWinScaling(int index)
    {
        StartCoroutine(PlayWinScalingRoutine(index));
    }
    private IEnumerator PlayWinScalingRoutine(int index)
    {
        while (startFlag == false)
        {
            if (revDirection == false)
            {
                if (ReelSlot[index].GetComponent<SpriteRenderer>().transform.localScale.y < targetScale[index].y + 0.2f)
                {
                    ReelSlot[index].GetComponent<SpriteRenderer>().transform.localScale += scaleChange;
                }
                else
                {
                    revDirection = true;
                }
            }

            if (revDirection == true)
            {
                if (ReelSlot[index].GetComponent<SpriteRenderer>().transform.localScale.y > targetScale[index].y - 0.1f)
                {
                    ReelSlot[index].GetComponent<SpriteRenderer>().transform.localScale -= scaleChange;
                }
                else
                {
                    revDirection = false;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        ReelSlot[index].GetComponent<SpriteRenderer>().transform.localScale = targetScale[index];
    }
    // Update is called once per frame
    void Update()
    {

    }
}
