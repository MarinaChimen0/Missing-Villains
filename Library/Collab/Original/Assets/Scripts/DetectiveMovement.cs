using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveMovement : MonoBehaviour {

    public float moveSpeed;
    bool facingRight = true;
    public GameObject cam;
    public Animator anim;
    // Use this for initialization
    void Start () {
        cam = GameObject.Find("Main Camera");
        //Make camera child of the police to follow him
        cam.transform.SetParent(this.transform);

        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
 
    }

    //Move character
    void Move ()
    {
        //Keep policeman straight
        //transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        float move = Input.GetAxis("Horizontal");
        if (move == 0)
        {
            move = Input.GetAxis("Vertical");
        }
        anim.SetFloat("speed", Mathf.Abs(move));

        //Go to the rigth
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetInteger("orientation", 1);
            transform.Translate(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
        }
        //Go to the left
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetInteger("orientation", 2);
            transform.Translate(new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
        }
        //Go down
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetInteger("orientation", 3);
            transform.Translate(new Vector3(0, -moveSpeed, 0) * Time.deltaTime);
        }
        //Go up
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("orientation", 4);
            transform.Translate(new Vector3(0, moveSpeed, 0) * Time.deltaTime);
        }
        if (Input.GetKey(""))
        {
            anim.SetInteger("orientation", 0);
        }
    } 
}
