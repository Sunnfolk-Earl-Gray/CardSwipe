using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{
   private Score highScore;

   private int currentScore;

   private class Score
   {
      public string ScoreIndex;
      public int scoreInt;
   }
   
  private List<Score> Scores = new List<Score>();

   private void Awake()
   {
      _loadScores();
      for (int i = 10; i >= 0; i--)
      {
         Scores.Add(new Score() { ScoreIndex = i.ToString(), scoreInt = 0 });
      }

      foreach (Score score in Scores)
      {
         Debug.Log(score.ScoreIndex + "," + score.scoreInt);
      }
   }

   private void OnDisable()
   {
      _saveScore();
   }

   private void _loadScores()
   {
      foreach (Score score in Scores)
      {
         score.scoreInt = PlayerPrefs.GetInt(score.ScoreIndex);
      }
   }
   
   
   private void _saveScore()
   {
      PlayerPrefs.DeleteAll();
      foreach (Score score in Scores)
      {
         if (currentScore >= score.scoreInt) score.ScoreIndex = (Int32.Parse(score.ScoreIndex) + 1).ToString();
         if (currentScore < score.scoreInt) PlayerPrefs.SetInt(score.ScoreIndex, score.scoreInt);
      }
   }
}
