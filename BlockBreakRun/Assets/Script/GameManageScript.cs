using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManageScript : MonoBehaviour {
    public float timer;
    public float timelimit;
    public Text timertext;
    public GameObject panel;
    public bool isGameRunnig;

	// Use this for initialization
	void Start () {
        timer = 0.0f;
        timelimit = 120.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (isGameRunnig)
        {
            timer += Time.deltaTime;
            timelimit -= Time.deltaTime;
            if (timelimit < 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            timertext.text = ((int)timelimit).ToString();
        }
	}

    //menuのスクリプト
    public void ClickPoseButton()
    {
        if (isGameRunnig)
        {
            panel.SetActive(true);
            isGameRunnig = false;
        }
    }

    public void menu_Title()
    {
        SceneManager.LoadScene("Title");
    }
    public void menu_Continue()
    {
        panel.SetActive(false);
        isGameRunnig = true;
    }
}
