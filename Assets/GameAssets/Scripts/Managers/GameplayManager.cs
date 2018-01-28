using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {

    public float timeToChangeRule;
    public List<Player> players = new List<Player>();
    public List<Infectable> infectables = new List<Infectable>();

    [Header("UI")]
    [SerializeField] Text P1Score;
    [SerializeField] Text P2Score;
    [SerializeField] Text P3Score;
    [SerializeField] Text P4Score;
    [SerializeField] Image[] Rules;

    public static GameplayManager Instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    public int AddPlayer(Player player)
    {
        int count = players.Count;

        if (count >= 4)
            return -1;

        players.Add(player);

        return count;
    }

    void Start()
    {
        UpdatePoints();
        InvokeRepeating("ChangeRules", 0, 15);
    }

    public void Update()
    {
    }

    public void ChangeInfectableOwner(Player playerOrigin, Player playerDest) {
        for (int i = 0; i < infectables.Count; i++) {
            if(infectables[i].infectedBy != null && infectables[i].infectedBy.GetComponent<Player>().playerIndex == playerOrigin.playerIndex)
            {
                infectables[i].Infect(playerDest);
            }
        } 
    }

    public void UpdatePoints()
    {
        P1Score.color = GameManager.Instance.GetPlayerColor(1);
        P1Score.rectTransform.Find("Dominations").GetComponent<Text>().text = "Dominations: " + players[0].dominations;
        P1Score.rectTransform.Find("Dominations/Score").GetComponent<Text>().text = "Score: " + players[0].points;

        P2Score.color = GameManager.Instance.GetPlayerColor(2);
        P2Score.rectTransform.Find("Dominations").GetComponent<Text>().text = "Dominations: " + players[1].dominations;
        P2Score.rectTransform.Find("Dominations/Score").GetComponent<Text>().text = "Score: " + players[1].points;

        P3Score.color = GameManager.Instance.GetPlayerColor(3);
        P3Score.rectTransform.Find("Dominations").GetComponent<Text>().text = "Dominations: " + players[2].dominations;
        P3Score.rectTransform.Find("Dominations/Score").GetComponent<Text>().text = "Score: " + players[2].points;

        P4Score.color = GameManager.Instance.GetPlayerColor(4);
        P4Score.rectTransform.Find("Dominations").GetComponent<Text>().text = "Dominations: " + players[3].dominations;
        P4Score.rectTransform.Find("Dominations/Score").GetComponent<Text>().text = "Score: " + players[3].points;
    }

    void ChangeRules()
    {
        RuleManager.Instance.NextRules();
        for (int i = 0; i < 4; i++) {
            Rules[(i + 1) % 4].color = GameManager.Instance.GetPlayerColor(RuleManager.Instance.GetRule(i));
        }
        AudioManager.Instance.ChangedRule();
    }
}
