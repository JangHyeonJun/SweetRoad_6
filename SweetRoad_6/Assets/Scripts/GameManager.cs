using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Board board;

    public static GameManager instance;
    public int move_num = 30;
    public Text move_text;
    
    public int mission_num = 5;
    public Image mission_image;
    public Text mission_text;

    public int score_num = 0;
    public Text score_text;

    public Image result_ui;
    public Text result_text;
    public Text result_score_text;

    public Button close_button;
    public Button setting_button;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        if (instance == null)
            instance = this;
        board = GameObject.Find("Board").GetComponent<Board>();
        mission_image.GetComponent<Image>().sprite = board.munchkin_sprite;
        result_ui.gameObject.SetActive(false);
        result_text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        move_text.text = move_num.ToString();
        mission_text.text = mission_num.ToString();
        score_text.text = score_num.ToString();
        result_score_text.text = score_text.text;

        if (mission_num <= 0)
            StartCoroutine(MissionClear());
        else if (move_num <= 0)
            StartCoroutine(MissionFail());
    }

    public void RestartGame()
    {
        Application.LoadLevel(1);
    }

    public void QuitLevel()
    {
        Application.LoadLevel(0);
    }

    public void OpenSetting()
    {
        Time.timeScale = 0.0f;
        close_button.gameObject.SetActive(true);
        result_ui.gameObject.SetActive(true);
        setting_button.gameObject.SetActive(false);
    }

    public void CloseSetting()
    {
        close_button.gameObject.SetActive(false);
        result_ui.gameObject.SetActive(false);
        setting_button.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }

    IEnumerator MissionClear()
    {
        yield return new WaitForSeconds(1.0f);
        result_ui.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        result_text.text = "CLEAR";
    }

    IEnumerator MissionFail()
    {
        yield return new WaitForSeconds(1.0f);
        result_ui.gameObject.SetActive(true);
        Time.timeScale = 0.0f;
        result_text.text = "FAIL";
    }
}
