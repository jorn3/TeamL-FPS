using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisible : MonoBehaviour
{   
    public bool playerIsVisble;
    private int n;
    // Start is called before the first frame update
    void Start()
    {
        n=0;
        playerIsVisble = false;
    }

    // Update is called once per frame
    void Update()
    {
        n++;
        if(n>7){
            playerIsVisble = false;  
            Debug.Log("invisible"); 
        }
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.name == "streetlight"){
            playerIsVisble = true;
            n=0;
            Debug.Log("works");
        }
    }
}
