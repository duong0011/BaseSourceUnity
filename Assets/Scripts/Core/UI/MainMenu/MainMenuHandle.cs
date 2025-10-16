using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandle : MonoBehaviour
{ 
    public void LoadExp()
    {
        MainManager.Instance.LoadExp();
    }
    public void LoadTutorial()
    {
        MainManager.Instance.LoadTutorial();
    }
    public void Exit()
    {
        MainManager.Instance.Exit();
    }
}
