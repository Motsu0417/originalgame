using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {
    public float timer;
    public GameObject player;
    public GameObject maincamera;
    public GameObject Gamemanager;
    public Animation CntAnimation;
    public int timecount;
    public Text countText;
    public GameObject text;
    bool sound = true;

	// Use this for initialization
	void Start () {
        timer = 0f;
        timecount = 0;
        player.SetActive(false);
        maincamera.SetActive(false);
        Gamemanager.GetComponent<GameManageScript>().isGameRunnig = false;
        gameObject.GetComponent<Animation>().Play();
        CntAnimation = countText.GetComponent<Animation>();
        countText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > 1)
        {
            if (timecount > 2)
            {
                countText.text = (6 - timecount) + "";
                CntAnimation.Stop();
                CntAnimation.Play();
                if (sound)
                {
                    text.GetComponent<AudioSource>().Play();
                    sound = false;
                }
            }
            timecount++;
            timer--;
        }
        if (timecount == 7)
        {
            player.SetActive(true);
            maincamera.SetActive(true);
            gameObject.SetActive(false);
            Gamemanager.GetComponent<GameManageScript>().isGameRunnig = true;
            countText.text = "";
        }
	}
}
