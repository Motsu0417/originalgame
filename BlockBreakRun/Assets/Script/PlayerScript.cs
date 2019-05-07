using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    public int speed = 10;
    public Camera maincamera;
    public float tics;
    public long timer;

	// Use this for initialization
	void Start () {
        tics = 0.0f;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //姿勢強制
        gameObject.transform.rotation *= new Quaternion(0, 0, 0, 0);
        //進行系
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.position = new Vector3(10,1,gameObject.transform.position.z + 1 * Time.deltaTime *speed);
        }

        //秒数管理 
        tics += Time.deltaTime;
        if (tics > 1)
        {
            timer++;
            tics--;
        }

        //あるく動き
	}
}
