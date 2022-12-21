using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class GameManager : MonoBehaviour
{

    public AudioSource theMusic;

    public bool startPlaying = false;

    public BeatScroller theBS;

    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierTresholds;

    public Text scoreText;
    public Text multiText;

    public float totalNotes;
    public float normalHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public int left;
    public int up;
    public int down;
    public int right;

    public bool rt = true;
    public string ret = "1699999";

    public GameObject resultsScreen;
    public Text percentHitText, normalsText, goodsText, perfectsText, missesText, rankText, finalScoreText;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    public SerialPort sp = new SerialPort("/dev/cu.usbmodem11301", 9600);//每個人的port感覺不一樣 11401你的
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        resultsScreen.SetActive(false);
        sp.Open();
        sp.ReadTimeout = 1;
        scoreText.text = "Score: 0";
        currentMultiplier = 1;
        rt = true;
        totalNotes = FindObjectsOfType<NoteObject>().Length;
        ret = "23";
    }
    public int inp = 1;
    
    // Update is called once per frame
    void Update()
    {
        
        try {
            ret = sp.ReadLine();
            //Debug.Log("read");
            inp = int.Parse(ret);
            Debug.Log(inp);
        } catch (System.Exception) {}

        if(!startPlaying&&rt){
            if(Input.anyKeyDown){
                Debug.Log("send");
                sp.WriteLine("1");

            }
            else if(inp == 0){
                startPlaying = true;
                rt = false;
                theBS.hasStarted = true;
                theMusic.Play();
                Debug.Log("receive");
            }
            
            else{
                try {
                    ret = sp.ReadLine();
                    inp = int.Parse(ret);
                    Debug.Log(inp);
                } catch (System.Exception) {}           
                if(inp == 0){
                Debug.Log("Work");
            }
            }
        }
        else{
            try {
                ret = sp.ReadLine();
                inp = int.Parse(ret);
            } catch (System.Exception) {}

            
            
            if(!theMusic.isPlaying && !resultsScreen.activeInHierarchy){
                resultsScreen.SetActive(true);

                normalsText.text = "" + left;
                goodsText.text = up.ToString();
                perfectsText.text = down.ToString();
                missesText.text = "" + right;

                float totalHit = normalHits + goodHits + perfectHits;
                float percentHit = (totalHit/totalNotes) * 100f;

                percentHitText.text = percentHit.ToString("F1") + "%";

                string rankVal = "Oh";

                if(percentHit > 10){
                    rankVal = "D";
                    if(percentHit > 50){
                        rankVal = "C";
                        if(percentHit > 70){
                            rankVal = "B";
                            if(percentHit > 85){
                                rankVal = "A";
                                if(percentHit > 90){
                                    rankVal = "A+";
                                    if(percentHit > 95){
                                        rankVal = "SS";
                                    }
                                }
                            }
                        }
                    }
                }

                rankText.text = rankVal;

                finalScoreText.text = currentScore.ToString();
            }
        }
    }

    public void NoteHit(){
        Debug.Log("Hit On Time");

        if(currentMultiplier - 1 <multiplierTresholds.Length){
            multiplierTracker++;

            if(multiplierTresholds[currentMultiplier - 1] <= multiplierTracker){
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;
        
        scoreText.text = "Score: " + "400";
    }

    public void NormalHit(){
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 8){
            right++;
        }
        if(NoteObject.ist.KeyToPress== KeyCode.LeftArrow && inp == 6){
            left++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 7){
            up++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.LeftArrow && inp == 9){
            down++;
        }
        
        normalHits++;
    }

    public void GoodHit(){
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 8){
            right++;
        }
        if(NoteObject.ist.KeyToPress== KeyCode.LeftArrow && inp == 6){
            left++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 7){
            up++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.LeftArrow && inp == 9){
            down++;
        }
        goodHits++;
    }

    public void PerfectHit(){
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 8){
            right++;
        }
        if(NoteObject.ist.KeyToPress== KeyCode.LeftArrow && inp == 6){
            left++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.RightArrow && inp == 7){
            up++;
        }
        if(NoteObject.ist.KeyToPress == KeyCode.LeftArrow && inp == 9){
            down++;
        }
        perfectHits++;
    }

    public void NoteMissed(){
        Debug.Log("Missed Note");

        currentMultiplier = 1;
        multiplierTracker = 0;

        missedHits++;

        multiText.text = "Multiplier: x" + currentMultiplier;

    }
}
