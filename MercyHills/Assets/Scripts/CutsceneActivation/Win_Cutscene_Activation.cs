using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Cutscene_Activation : MonoBehaviour
{
    public GameObject winCutscene;


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player" && Input.GetButtonDown("XButton"))
    //    {

    //        winCutscene.SetActive(true);
    //    }
    //}
    public void EnableCutscene()
    {
        winCutscene.SetActive(true);
        StartCoroutine(startCutscene());
    }

    IEnumerator startCutscene()
    {
        yield return new WaitForSeconds(62.5f);
    }
}
