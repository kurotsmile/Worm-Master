using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Animal_Manager : MonoBehaviour
{
    public GameObject Cow_prefab;
    public GameObject Cow_beef_prefab;
    private float timer_create = 0f;

    private float boardDown, boardTop, boardLeft, boardRight;
    private float boardOffset;
    private Vector3 randomPosition;

    public void On_Load()
    {
        boardDown = -GameObject.FindGameObjectWithTag(Tags.BoundsTag).GetComponent<BoxCollider2D>().size.y / 2.0f;
        boardTop = -boardDown;
        boardLeft = -GameObject.FindGameObjectWithTag(Tags.BoundsTag).GetComponent<BoxCollider2D>().size.x / 2.0f;
        boardRight = -boardLeft;
        boardOffset = 2.0f;
    }

    private void Update()
    {
        this.timer_create += 1f * Time.deltaTime;
        if (this.timer_create > 15f)
        {
            this.create_Animal();
            this.timer_create = 0;
        }
    }

    private void create_Animal()
    {
        float minX = boardLeft + boardOffset,maxX = boardRight - boardOffset,minY = boardDown + boardOffset,maxY = boardTop - boardOffset;
        this.randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0.0f);
        this.GetComponent<GameManager>().create_effect(this.randomPosition, 4);
        delay_function(1f, create_cow);
    }

    private void create_cow()
    {
        GameObject cow_item;
        if(Random.Range(0,2)==1)
            cow_item = Instantiate(this.Cow_beef_prefab, this.randomPosition, Quaternion.identity);
        else
            cow_item = Instantiate(this.Cow_prefab, this.randomPosition, Quaternion.identity);
        cow_item.transform.SetParent(this.transform);
        cow_item.transform.localScale = new Vector3(cow_item.transform.localScale.x, cow_item.transform.localScale.y, cow_item.transform.localScale.z);
    }

    public void reset()
    {
        foreach(Transform tr in this.transform)
        {
            if(tr.gameObject.tag== Tags.Animal)
            {
                Destroy(tr.gameObject);
            }
        }
        this.timer_create = 0f;
    }

    public void delay_function(float timer, UnityAction act_func)
    {
        StartCoroutine(act_delay_function(timer, act_func));
    }

    private IEnumerator act_delay_function(float timer, UnityAction act_func)
    {
        yield return new WaitForSeconds(timer);
        act_func();
    }
}
