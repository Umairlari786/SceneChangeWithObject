using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImage : MonoBehaviour
{
    public GameObject[] mySprite;
    bool startFlag = false;
    private int[] ReelArray = new int[100];
    private float currentSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        for(int j=0; j<ReelArray.Length; j++)
        {
            ReelArray[j] = Random.Range(0,5);
        }
        for(int i = 0; i < mySprite.Length; i++)
        {
            mySprite[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path: "ImagesSymbol/Symbol" + ReelArray[i]);
            //mySprite[i].transform.localScale = new Vector3(75.0f,60.0f,0.0f);
        }
    }
    private IEnumerator StartSpin()
    {
        //velocity = new Vector3(0.0f, 0.004f, 0.0f);
        currentSpeed = 2.0f;
        yield return new WaitForSeconds(0.5f);
        /*
        currentSpeed = 2.8f;
        //velocity = new Vector3(0.0f, 0.008f, 0.0f);
        yield return new WaitForSeconds(0.5f);

        currentSpeed = 10.0f;
        //velocity = new Vector3(0.0f, 0.015f, 0.0f);
        yield return new WaitForSeconds(1.5f);

        currentSpeed = 2.0f;
        //velocity = new Vector3(0.0f, 0.004f, 0.0f);
        yield return new WaitForSeconds(1.0f);

        currentSpeed = 10.0f;
        //velocity = new Vector3(0.0f, 0.0f, 0.0f);
        //yield return new WaitForSeconds(0.5f);
        //ChangeState(States.STATE_ACCELERATION);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ChangeState(States.STATE_START_SPIN);
            startFlag = true;
            StartCoroutine(StartSpin());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ChangeState(States.STATE_STOP);
            startFlag = false;
        }
        if (startFlag == true)
        {
            for (int j = 0; j < mySprite.Length; j++)
            {
                mySprite[j].GetComponent<SpriteRenderer>().transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
                //Reel1[j].GetComponent<SpriteRenderer>().transform.Translate(velocity);
                if (mySprite[j].GetComponent<SpriteRenderer>().transform.localPosition.y > 190)
                {
                    mySprite[j].GetComponent<SpriteRenderer>().transform.localPosition = new Vector2(-177.0f, -200.01f);
                }
            }
        }
    }
}
