using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public GameObject Goal;
    public Transform SpawnPoint;
    public GameObject Player;
    public GameObject RestartButton;
    public GameObject Princess;
    public GameObject Win;
    public GameObject Door;
    public Sprite CloseDoor;
    public Sprite OpenDoor;

    static public Text ClickCountText;
    static public Text BestCountText;

    static public int ClickCount = 0;
    static public int BestPerformance = 1000000;

    private void Awake()
    {
        ClickCountText = GameObject.Find("ClickCount").gameObject.GetComponent<Text>();
        BestCountText = GameObject.Find("BestCount").gameObject.GetComponent<Text>();
    }

    static public void UpdateClickCount()
    {
        ClickCount++;
        GameManager.ClickCountText.text = "Click Count : " + ClickCount.ToString();
    }

    static public void UpdateBestCount()
    {
        GameManager.BestCountText.text = "Best Cout : " + BestPerformance.ToString();
    }

    public void Restart()
    {
        Goal.SetActive(false);
        Door.GetComponent<SpriteRenderer>().sprite = CloseDoor;
        Win.SetActive(false);

        // Respawn player 
        Player.GetComponent<PlayerControl>().Reset();

        // Reset princess
        Princess.GetComponent<Princess>().Reset();

        // Reset score
        ClickCount = -1;
        UpdateClickCount();
        RestartButton.SetActive(false);
    }
}
