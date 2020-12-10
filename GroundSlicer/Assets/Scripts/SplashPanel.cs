using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashPanel : MonoBehaviour
{
    [SerializeField] GameObject player;

    public void GoButton()
    {
        PlayerPrefs.SetInt("FirstOpen", 1);
        player.SetActive(true);
        gameObject.SetActive(false);
    }
}
