using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameMenuManager : MonoBehaviour
{
    public void GoGame()
    {
        SceneManager.LoadScene(1);
    }
}
