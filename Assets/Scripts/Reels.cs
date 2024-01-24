using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reels : MonoBehaviour
{
    public Symbol ReelSymbols;
    public GameObject reelsPrefab;
    public GameObject reelsContainer;
    private int[] ReelArray = new int[100];
    private List<GameObject> prefabList;
    private float yPos = -0.31f;
    private GameObject obj;
    // Start is called before the first frame update
    void Start()
    {

        
        for (int j = 0; j < ReelArray.Length; j++)
        {
            ReelArray[j] = Random.Range(0, 8);
        }
        //reelsPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path: "ImagesSymbol/Symbol" + ReelArray[i]);
        //reelsPrefab.GetComponent<SpriteRenderer>().sprite = ReelSymbols.Symbols[ReelArray[5]];

        
        for (var i = 0; i <= 5; i++)
        {
            obj = Instantiate(reelsPrefab, transform);
            
            obj.transform.localPosition = new Vector3(-0.095f, yPos, 0);
            obj.transform.localScale = new Vector3(0.15f, 0.035f, 0);
            
            yPos += 0.13f;
            obj.GetComponent<SpriteRenderer>().sprite = ReelSymbols.Symbols[ReelArray[i]];
         
            obj.SetActive(true);
            //prefabList.Add(obj);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        //for (int j = 0; j < prefabList.Count; j++)
        {
            //prefabList[j].GetComponent<SpriteRenderer>().transform.Translate(Vector3.up * 2.0f * Time.deltaTime);
            //Reel1[j].GetComponent<SpriteRenderer>().transform.Translate(velocity);
            /*if (mySprite[j].GetComponent<SpriteRenderer>().transform.localPosition.y > 190)
            {
                mySprite[j].GetComponent<SpriteRenderer>().transform.localPosition = new Vector2(-177.0f, -200.01f);
            }*/
        }
    }
}
