using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {
    public float dragSpeed = 2f;
    public float RotateSpeed = 20000f;
    
    private Vector3 dragOrigin;
    


    // Use this for initialization
    void Start() {
        
        
    }

    // Update is called once per frame
    void Update() {
       

       

        Move_camera();
        Zoom_camera();
        //Rotate_camera();



        
    }
    void Move_camera() {
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin.x = Input.mousePosition.x;
            return;
        }
        if (!Input.GetMouseButton(0)) { return; }
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);
        transform.Translate(move, Space.World);
    }
    void Rotate_camera() {
        var cam = GetComponent<Camera>();
        Vector3 middle = cam.ViewportToWorldPoint(new Vector3(0.5f , cam.nearClipPlane , 0.5f));
        Vector3 rot = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        if (Input.GetMouseButtonDown(1))
        {
            //fucked up code
            dragOrigin = middle;
            //goede code
            //dragOrigin.y = Input.mousePosition.y;
            return;
        }
        if (!Input.GetMouseButton(1)) { return; }
        Vector3 move = new Vector3(0f, rot.y * RotateSpeed, 0f );
        transform.Rotate(-move , Space.World);
        //transform.Translate(move, Space.World);
    }
   
    void Zoom_camera() {
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        float Scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Scroll > 0 && transform.position.y > 20)
        {
            Vector3 zoom = new Vector3(pos.x, pos.y - 10, pos.z);
            transform.Translate(zoom, Space.World);
        }
        else if (Scroll < 0 && transform.position.y < 120)
        {
            
            Vector3 zoom = new Vector3(pos.x, pos.y + 10, pos.z);
            transform.Translate(zoom, Space.World);
        }
    }     
}

