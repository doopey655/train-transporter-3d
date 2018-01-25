using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    bool activate;
    public Button railsButton;
    public Button signalsButton;
    public GameObject railsUI;
    public GameObject signalsUI;
    // Use this for initialization
    void Start()
    {
        railsUI.SetActive(false);
        signalsUI.SetActive(false);

        Button Rails = railsButton.GetComponent<Button>();
        Button Signals = signalsButton.GetComponent<Button>();

        Rails.onClick.AddListener(railsUIToggle);
        Signals.onClick.AddListener(signalsUIToggle);
    }
    void railsUIToggle() {
        if (railsUI.activeSelf == false)
        {
            railsUI.SetActive(true);
            
        }
        else
        {
            railsUI.SetActive(false);
        }

        signalsUI.SetActive(false);
    }
    void signalsUIToggle()
    {
        if (signalsUI.activeSelf == false)
        {
            signalsUI.SetActive(true);
        }
        else
        {
            signalsUI.SetActive(false);
        }

        railsUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //railsButton.onClick.AddListener(railsUIToggle);
       // signalsButton.onClick.AddListener(signalsUIToggle);
    }
}
