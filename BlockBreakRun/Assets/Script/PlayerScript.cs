using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    public int speed = 10;
    public Camera maincamera;
    public GameObject[] tools = new GameObject[4];
    public GameObject HandingTool;
    public Slider runGage;
    private CharacterController controller;
    private Vector3 moveVec;

	// Use this for initialization
	void Start () {
        HandingTool = tools[3];
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        //姿勢強制
        gameObject.transform.rotation *= new Quaternion(0, 0, 0, 0);
        //前進
        moveVec.z = speed;

        //画面左runゲージ
        //104,504
        runGage.value = (gameObject.transform.position.z - 104) / 400;

        //ジャンプ処理
        if (controller.isGrounded) { if (Input.GetKeyDown(KeyCode.Space)) { moveVec.y = 15 - 10 * Time.deltaTime; } }

        //ベクターのコミット
        controller.Move(moveVec * Time.deltaTime);
	}

   public void setToolChange(int num) { HandingTool = tools[num]; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Goal"))
        {
            SceneManager.LoadScene("Goal");
        }

    }
}
