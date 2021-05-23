using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public Text m_Scoreboard;
    public static int m_Score;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Text>();
    }


    // Update is called once per frame
    void Update()
    {
        m_Scoreboard.text = "Score:" + m_Score;
    }
}
