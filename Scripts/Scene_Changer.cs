using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Changer : MonoBehaviour
{
    public string scenceToLoad ;
    public Animator fadeAnim;
    public float fadeTime = .5f;



    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Player")
        {
            fadeAnim.Play("FadeToWhite");
            StartCoroutine(DelayFade());
        }
    }

    IEnumerator DelayFade() 
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scenceToLoad);
    }
}
