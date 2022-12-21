using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class NoteObject : MonoBehaviour
{

    public static bool canBePressed;

    public KeyCode KeyToPress;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;
    
    public string arduinoOutput = "111111111111"; 
    public int Uinput = 0;

    public static NoteObject ist;

    // Start is called before the first frame update
    void Start()
    {
        ist = this;
        canBePressed = false;
        Uinput = 13;
        arduinoOutput = "111111111111";
    }

    public void run(){
        if (!GameManager.instance.sp.IsOpen) {
            GameManager.instance.sp.Open();
        } else { 
            if(GameManager.instance.inp == 6){
                KeyToPress = KeyCode.LeftArrow;
                Debug.Log(KeyToPress);
            }
            else if(GameManager.instance.inp == 7){
                KeyToPress = KeyCode.RightArrow;
                Debug.Log(KeyToPress);
            }
            else if(GameManager.instance.inp == 8){
                KeyToPress = KeyCode.RightArrow;
                Debug.Log(KeyToPress);
            }
            else if(GameManager.instance.inp == 9){
                KeyToPress = KeyCode.LeftArrow;
                Debug.Log(KeyToPress);
            }

            if(NoteObject.canBePressed){
                gameObject.SetActive(false);
                
                Debug.Log(Mathf.Abs(transform.position.y));

                if(Mathf.Abs(transform.position.y) > 1){
                    Debug.Log("m2");
                    canBePressed = false;
                    GameManager.instance.NoteMissed();
                    Instantiate(missEffect, transform.position, missEffect.transform.rotation);
                }
                else if(Mathf.Abs(transform.position.y) > 0.7){
                    Debug.Log("Hit");
                    canBePressed = false;
                    GameManager.instance.NormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
                else if(Mathf.Abs(transform.position.y) > 0.6){
                    Debug.Log("Good");
                    canBePressed = false;
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else{
                   Debug.Log("Perfect");
                   canBePressed = false;
                   GameManager.instance.PerfectHit();
                   Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        canBePressed = false;
        if(other.tag == "Activator"){
            Debug.Log("in");
            canBePressed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyToPress)){
            if(canBePressed){
                gameObject.SetActive(false);
                
                if(Mathf.Abs(transform.position.y) > 1){
                    Debug.Log("mmmmmm");
                }
                else if(Mathf.Abs(transform.position.y) > 0.7){
                    Debug.Log("Hit aaaaaa");
                }
                else if(Mathf.Abs(transform.position.y) > 0.6){
                    Debug.Log("Good aaaaa");
                }
                else{
                   Debug.Log("Perfect aaaaa");
                }
            }
        }
    }

    

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Activator"){
            run();
            if(canBePressed){
                GameManager.instance.NoteMissed();
                Instantiate(missEffect, transform.position, missEffect.transform.rotation);
            }
            canBePressed = false;
        }
    }
}
