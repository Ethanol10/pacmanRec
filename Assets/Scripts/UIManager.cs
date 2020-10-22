using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Canvas menuHUD;
    
    public Text score1;
    public Text timer1;
    public Text score2;
    public Text timer2;
    // Start is called before the first frame update
    void Start()
    {
        score1 = menuHUD.transform.GetChild(5).GetComponent<Text>();
        timer1 = menuHUD.transform.GetChild(6).GetComponent<Text>();
        score2 = menuHUD.transform.GetChild(7).GetComponent<Text>();
        timer2 = menuHUD.transform.GetChild(8).GetComponent<Text>();

        float playerTime1 = PlayerPrefs.GetFloat("timer1", 9999999.0f);
        TimeSpan time = TimeSpan.FromSeconds(playerTime1);
        string str = time.ToString(@"mm\:ss\:ff");
        score1.text = "HI SCORE: " + PlayerPrefs.GetInt("score1", 0);
        timer1.text = "BEST TIME: " + str;

        float playerTime2 = PlayerPrefs.GetFloat("timer2", 9999999.0f);
        TimeSpan time2 = TimeSpan.FromSeconds(playerTime2);
        string str2 = time2.ToString(@"mm\:ss\:ff");
        score2.text = "HI SCORE: " + PlayerPrefs.GetInt("score2", 0);
        timer2.text = "BEST TIME: " + str2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFirstLevel() {
        DontDestroyOnLoad(this);
        SceneManager.LoadSceneAsync(1);
    }

    public void CloseGame(){
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void deletePrefs(){
        PlayerPrefs.DeleteAll();
    }
}
