using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        Application.LoadLevel(1);
    }
}
