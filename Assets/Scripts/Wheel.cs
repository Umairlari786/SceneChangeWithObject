using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
struct WinData
{
    public int angle;
    public int value;
}
public class Wheel : MonoBehaviour
{
    public RectTransform armMovement;
    public int sector = 0;
    private const int minInHours = 60;
    private int currentTime = 0;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI winVal;
    private List<WinData> data;
    public AnimationCurve myCurve;
    private Vector2 initialPosition = Vector2.zero;
    float angleVal = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        WinData tempData;
        data = new List<WinData>();
        initialPosition = armMovement.transform.position;
        tempData.angle = 0;
        tempData.value = 2000;
        data.Add(tempData);
        tempData.angle = 30;
        tempData.value = 300;
        data.Add(tempData);
        tempData.angle = 60;
        tempData.value = 500;
        data.Add(tempData);
        tempData.angle = 90;
        tempData.value = 300;
        data.Add(tempData);
        tempData.angle = 120;
        tempData.value = 1000;
        data.Add(tempData);
        tempData.angle = 150;
        tempData.value = 500;
        data.Add(tempData);
        tempData.angle = 180;
        tempData.value = 800;
        data.Add(tempData);
        tempData.angle = 210;
        tempData.value = 600;
        data.Add(tempData);
        tempData.angle = 240;
        tempData.value = 500;
        data.Add(tempData);
        tempData.angle = 270;
        tempData.value = 200;
        data.Add(tempData);
        tempData.angle = 300;
        tempData.value = 100;
        data.Add(tempData);
        tempData.angle = 330;
        tempData.value = 300;
        data.Add(tempData);
    }
    public void StartWheel()
    {
        winText.enabled = false;
        winVal.enabled = false;
        armMovement.transform.SetPositionAndRotation(initialPosition, Quaternion.identity);
        StartCoroutine(WheelMove());
    }

    private IEnumerator WheelMove()
    {


        //TestWheelMove();
        //yield break;

        float time = 0.0f;

        //int rotation = Random.Range(1, 3);
        //int sector = Random.Range(1, 13);
        //int sectorAngle = Random.Range(0, 30);
        //int finalVal = 360 * 2 + 30 * 3 + 12;
        float speed = Random.Range(450.0f, 800.0f);
        //float valCurve = myCurve.Evaluate(-speed * Time.deltaTime);

        while (time <= 0.5f)
        {
            armMovement.transform.Rotate(0, 0, -myCurve.Evaluate(time));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        time = 0.0f;
        while (time <= 2.15f)
        {
            armMovement.transform.Rotate(0, 0, -speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }

        time = 0.0f;
        while (time <= 0.35f)
        {
            armMovement.transform.Rotate(0, 0, myCurve.Evaluate(time));
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
        }
        angleVal = armMovement.eulerAngles.z;
        CheckWin(angleVal);
    }

    void TestWheelMove()
    {
        StartCoroutine(TWheelMove());
    }
    private IEnumerator TWheelMove()
    {
        float time = 0.0f;
        //armMovement.transform.Rotate(0, 0, 330 );
        // yield return new WaitForSeconds(3);
        while (time <= 1.0f)
        {
            armMovement.transform.Rotate(0, 0, 360 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            // yield return new WaitForEndOfFrame();
        }
    }
    void CheckWin(float angleVal)
    {
        float declaredWinValue = 0.0f;
        angleVal = 360 - angleVal;
        for (int i = 0; i < 12; i++)
        {
            if ((angleVal >= data[i].angle) && (angleVal <= (data[i].angle + 30)))
            {
                declaredWinValue = data[i].value;
            }
        }
        winText.enabled = true;
        winVal.enabled = true;
        winVal.SetText(declaredWinValue.ToString());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
