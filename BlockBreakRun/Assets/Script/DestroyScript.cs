using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyScript : MonoBehaviour {
    public float reachableDistance = 30.0f;
    public Camera playerCamera;
    public GameObject toolhand;
    public GameObject dmgtxtManager;
    public GameObject GameManager;
    public Text damageText,text;
    private Vector3 touchdown, touchup;
    private int hosei;
    private string FlickDirection;
    public AudioClip[] BlockClips = new AudioClip[6];

    Animation anim,dmgtxtanim;
    Ray ray;
    RaycastHit hitInfo;

	// Use this for initialization
	void Start () {
        hosei = 100;
        anim = toolhand.gameObject.GetComponent<Animation>();
        dmgtxtanim = damageText.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = Input.mousePosition.ToString();
        if (GameManager.GetComponent<GameManageScript>().isGameRunnig)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchdown = Input.mousePosition;
                Debug.Log(touchdown.ToString());
            }
            if (Input.GetMouseButtonUp(0))
            {
                touchup = Input.mousePosition;
                Debug.Log(touchup.ToString());
                Doattack();
            }
            if (Input.GetKey(KeyCode.B))
            {
                ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                //rayが当たっている物と当たっているかの判定
                if (Physics.Raycast(ray, out hitInfo, reachableDistance) && hitInfo.transform.tag == "Breakable")
                {
                    //text.text = FlickDirection;
                    //ポイ
                    Destroy(hitInfo.transform.gameObject);
                }
            }
        }
    }


    private void Doattack()
    {
        Debug.Log("Doattack()が呼ばれました");
        //rayの取得
        ray = playerCamera.ScreenPointToRay(touchdown);
        Debug.Log("rayの決定");
        //rayが当たっている物と当たっているかの判定
        if (!(Physics.Raycast(ray, out hitInfo, reachableDistance))) { return; }
        if (hitInfo.transform.tag == "Breakable")
        {

            Debug.Log("ray照射:true");
            //text.text = Input.mousePosition.ToString();
            if (isFlick()) //タップがフリックなら
            {
                //text.text = FlickDirection;
                //DamageBlockメソッドへポイ
                DamageBlock(GetComponent<PlayerScript>().HandingTool, hitInfo.transform.gameObject);
                //アニメーションの切り替え
                anim.Stop();
                anim.Play();
            }

        }

    }

    public void DamageBlock(GameObject tool,GameObject Block)
    {
        int damage = tool.GetComponent<ToolScript>().attackPower; //toolの攻撃力の取得
        if (FlickDirection == Block.GetComponent<BlockScript>().weekdirec) damage *= 2;
        if (tool.GetComponent<ToolScript>().tool == Block.GetComponent<BlockScript>().aptitude) damage *= 5; //toolとブロックの適正チェック
        Block.GetComponent<BlockScript>().blockHp -= damage;//ブロックのhpを削る
        changeDamageText(damage);//ダメージ表示メソッドへ
        if (Block.GetComponent<BlockScript>().blockHp < 0)
        {
            PlaySound(Block, "destroy");
            Destroy(Block);//削り切ったらデストロイ
        }
        else
        {
            PlaySound(Block, "attack");
        }
        if (tool.name != "Hand")
        {
            tool.GetComponent<ToolScript>().dmgDurability -= Block.GetComponent<BlockScript>().hardend / (damage / 10 + 1); //toolの耐久値減少
        }
    }

    private void PlaySound(GameObject block, string v)
    {
        string toolName = block.GetComponent<BlockScript>().itemId;
        int playNumber;
        switch (toolName)
        {
            case "block_stone":
                playNumber = 0;
                break;
            case "block_iron":
                playNumber = 2;
                break;
            case "block_wood":
                playNumber = 4;
                break;
            default: return;
        }
        if (v.Equals("destroy"))
        {
            playNumber++;
        }
        GetComponent<AudioSource>().PlayOneShot(BlockClips[playNumber]);
    }

    void changeDamageText(int damage)      //ダメージ表示
    {
        damageText.text = damage.ToString();
        dmgtxtManager.transform.position = touchdown;
        dmgtxtanim.Stop();
        dmgtxtanim.Play();
    }

    //フリック処理
    bool isFlick()
    {
        if (touchdown.y > 1500) return false;
        FlickDirection = GetDirection();
        if (FlickDirection == "tap" || FlickDirection == "") return false;
        return true;
    }

    private string GetDirection()
    {
        float di_X = touchup.x - touchdown.x,
              di_Y = touchup.y - touchdown.y;
        string direction = "";
        if (Mathf.Abs(di_X) > Mathf.Abs(di_Y))
        {
            if (hosei < di_X) direction = "right"; //→
            else if (-1 * hosei > di_X) direction = "left"; //←
        }
        else if (Mathf.Abs(di_X) < Mathf.Abs(di_Y))
        {
            if (hosei < di_Y ) direction = "up"; //↑
            else if (-1 * hosei > di_Y) direction = "down"; //↓
        }
        else direction = "tap";
        Debug.Log("Direction : " + direction);
        return direction;
    }

}
