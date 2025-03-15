using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskList : MonoBehaviour
{
    Animator animText;

    public Text tasksText;


    public bool check = true;
    [SerializeField] private PauseMenu pause;

    public float timer = 2f;




    // Start is called before the first frame update
    void Start()
    {

        animText = GetComponent<Animator>();
        check = false;
        pause = FindObjectOfType<PauseMenu>();
    }

    private void Update()
    {
        ActivatingUIThroughBumper();


        //timer -= Time.deltaTime;
        //if (Input.GetButtonDown("LeftBumper"))
        //{
        //    check = true;
        //    animText.SetBool("check", false);
        //}



    }

    private void ActivatingUIThroughBumper()
    {
        if (Input.GetButtonDown("RightBumper") && check == false)
        {
            check = true;
            animText.SetBool("check", true);
        }
        else if ((Input.GetButtonDown("RightBumper") && check == true))
        {
            check = false;
            animText.SetBool("check", false);
        }
    }

    public void setString(string text)
    {
        tasksText.text = text;     
    }
}
