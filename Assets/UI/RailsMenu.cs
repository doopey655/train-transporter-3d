using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RailsMenu : MonoBehaviour {

    private float depth = 0.0f;
    private bool activate;
    private Vector3 cameraPos;
    private Vector3 followMe;
    private Vector3 spawnPos;
    //public Button railsButton;
    //public GameObject railsUI;

    //button classes here
    public Button straightRails;
    public Button curvedRails;
    public Button deleteRails;

    //gameObjects here
    public GameObject spawnStraightRail;
    
    public GameObject spawnRightRail;
    private GameObject SpawnCube;
    // Use this for initialization
    void Start () {
        //Initialize buttons here
        Button Straight = straightRails.GetComponent<Button>();
        Button Curved = curvedRails.GetComponent<Button>();
        Button Delete = deleteRails.GetComponent<Button>();

        //Game Objects here
        SpawnCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        //Rails listerners here
        Straight.onClick.AddListener(StraightRailsToggle);
        Curved.onClick.AddListener(CurvedRailsToggle);
        Delete.onClick.AddListener(DeleteRailsToggle);
    }
    void StraightRailsToggle()
    {
        
        if (activate == false)
        {
            activate = true;
            Debug.Log(activate);
            
        }
        
    }
    void CurvedRailsToggle()
    {

    }
    void DeleteRailsToggle()
    {

    }
    void placeRail()
    {
       
    }
    // Update is called once per frame
    void Update () {
        
        if (activate == true) {
            var mousePos = Input.mousePosition;
            mousePos.z = 300f;
            
            
            cameraPos = Camera.main.ScreenToWorldPoint(mousePos);
            spawnPos = new Vector3(cameraPos.x + 0f, 0f, cameraPos.z + 500f);
            spawnStraightRail.transform.position = spawnPos;

            //transform.position = followMe;
            if (Input.GetMouseButtonDown(1))
            { 
                Instantiate(spawnStraightRail, spawnPos, Quaternion.identity);
                spawnStraightRail.transform.position = new Vector3(0, 0, 0);
                activate = false;
                Debug.Log("rightclick");
                Debug.Log(activate);


            }
        }
    }
}
