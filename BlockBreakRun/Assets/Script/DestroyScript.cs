using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyScript : MonoBehaviour {
    public float reachableDistance = 5.0f;
    public Camera playerCamera;
    public GameObject tool;
    public Animation anim;
    public Text damageText;

    Ray ray;
    RaycastHit hitInfo;

	// Use this for initialization
	void Start () {
        anim = tool.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        bool isRayhit = Physics.Raycast(ray, out hitInfo, reachableDistance);
        if (isRayhit && Input.GetMouseButtonDown(0) && hitInfo.transform.tag == "Breakable")
        {
            anim.Stop();
            DamageBlock(tool , hitInfo.transform.gameObject);
            damageText.transform.position += new Vector3(0f,Input.mousePosition.y,0f);
            anim.Play();
            damageText.gameObject.transform.position = Input.mousePosition;
        }
    }

    public void DamageBlock(GameObject tool,GameObject Block)
    {
        int damage = 1;
        if (tool.GetComponent<ToolScript>().tool == Block.GetComponent<BlockScript>().aptitude)
        {
            damage *= 2;
        }
        Block.GetComponent<BlockScript>().blockHp -= damage;
        if (Block.GetComponent<BlockScript>().blockHp < 0)
        {
            Destroy(Block);
        }
        tool.GetComponent<ToolScript>().dmgDurability -= Block.GetComponent<BlockScript>().hardend / damage;
    }
}
