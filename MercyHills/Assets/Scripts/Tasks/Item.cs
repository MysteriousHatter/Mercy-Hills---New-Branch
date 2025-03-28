using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public float distance;

    //Game objects referencing the on screen text objects (button/description) and the door itself.
    public GameObject itemActiveDescription;
    public GameObject itemDescription;
    public GameObject item;
    public GameObject innerCross;
    //public AudioSource doorOpening;

    public Tasks tasks;
    public bool check;
    public int taskNum;
    public int dayNum;


    //When Player is close to an item
    private Controller player;
    float objStoppingDistance = 4f;
    [SerializeField] private Light fadingLight;
    [SerializeField] private float defaultIntensity = 4f;

    private void Start()
    {
        player = FindObjectOfType<Controller>();
        fadingLight.intensity = defaultIntensity;
    }

    void Update()
    {
        //Setting distance variable to the distance from the target in the RayCasting script
        distance = RayCasting.distance;
        float distanceToObject = Vector3.Distance(player.transform.position, this.transform.position);
        if (distanceToObject <= objStoppingDistance)
        {
            fadingLight.intensity -= 0.5f;
        }
        else if (distanceToObject >= objStoppingDistance)
        {
            if (fadingLight.intensity >= defaultIntensity)
            {
                return;
            }
            else
            {
                fadingLight.intensity += 0.5f;
            }
        }

    }

    //Enable on screen text when looking at object & perform actions on button press
    void OnMouseOver()
    {
        //If you are within 2.5 units of the door, display the button and its description by setting it to true
        if (distance < 3.5 && check)
        {
            itemActiveDescription.SetActive(true);
            //itemDescription.SetActive(true);
            innerCross.SetActive(true);
        }
        else if(distance < 3.5 && !check)
        {
            //itemActiveDescription.SetActive(false);
            itemDescription.SetActive(true);
            innerCross.SetActive(false);
            
        }
        

        //If correct button is pressed, continue to next if
        if (Input.GetButtonDown("XButton"))
        {
            /*If you are within 2.5 units of the door and have pressed button assigned above
            disable on screen text, disable the door box collider (can cause issues), open the door, and make a sound if wanted*/
            if (distance < 3.5 && check)
            {
                this.GetComponent<BoxCollider>().enabled = false;


                itemActiveDescription.SetActive(false);
                itemDescription.SetActive(false);
                item.SetActive(false);
                innerCross.SetActive(false);

                Debug.Log("Item " + taskNum);
                //doorOpening.Play();

                //Tasks check
                if (dayNum == 1)
                    tasks.day1Tasks(taskNum);
                else
                    tasks.day2Tasks(taskNum);
            }
        }
    }

    //Disable on screen text when looking away
    void OnMouseExit()
    {
        itemActiveDescription.SetActive(false);
        itemDescription.SetActive(false);
        innerCross.SetActive(false);
    }

    public void test22()
    {

    }
}
