using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float CountDownTime;
    private float GameTime;
    private float timer = 0;
    public Image Clock;
    public Text GameCountTimeText;

    // Start is called before the first frame update
    void Start()
    {
        GameTime = CountDownTime;
    }

    // Update is called once per frame
    void Update()
    {
 
        Clock.fillAmount = GameTime / CountDownTime;
        int M = (int)(GameTime / 60);
        float S = GameTime % 60;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0;
            GameTime--;
            GameCountTimeText.text = M + ":" + string.Format("{0:00}", S);
            if (S < 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

}
