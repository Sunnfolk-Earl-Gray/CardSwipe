
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardScript : MonoBehaviour 
{

    [SerializeField]private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    public int currentScore;
    public static LeaderboardScript instance;

    private void Awake() {
        //PlayerPrefs.DeleteAll();
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        
        if (entryContainer != null)
        {
            entryTemplate = entryContainer.Find("EntryTemplate");
            entryTemplate.gameObject.SetActive(false);
        }

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
    /*
        if (highscores == null) {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            AddHighscoreEntry(1000000, "CMK");
            AddHighscoreEntry(897621, "JOE");
            AddHighscoreEntry(872931, "DAV");
            AddHighscoreEntry(785123, "CAT");
            AddHighscoreEntry(542024, "MAX");
            AddHighscoreEntry(68245, "AAA");
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }
        */
    if (entryContainer == null || highscores == null) return;
        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }

        }
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        highscoreEntryTransformList = new List<Transform>();
        if (entryContainer != null || entryTemplate != null)
        {
            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
            {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        if (transformList.Count + 1 > 10)  return;
        Debug.Log("Creating highscore entry transform");
        float templateHeight = 10f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        Debug.Log(transformList);
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.GetComponent<ScoreEntryScript>().EntryRank.GetComponent<TMP_Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.GetComponent<ScoreEntryScript>().EntryScore.GetComponent<TMP_Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.GetComponent<ScoreEntryScript>().EntryName.GetComponent<TMP_Text>().text = name;
        
        

        transformList.Add(entryTransform);
    }

    public void saveScore()
    {
        AddHighscoreEntry(currentScore, GenerateRandomString(3));
    }
    private void AddHighscoreEntry(int score, string name) {
        Debug.Log("Adding highscore entry to table");
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        public string name;
    }

    private string GenerateRandomString(int length)
    {
        string allowedChars = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        List<string> allowedCharsList = allowedChars.Split(',').ToList();
        string returnString = allowedCharsList[Random.Range(0, allowedCharsList.Count)];
        for (int i = 0; i < length-1; i++) returnString += allowedCharsList[Random.Range(0, allowedCharsList.Count)];
        return returnString;
    }
}
