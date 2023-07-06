using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class AnimalPool : MonoBehaviour
{
    [SerializeField] private int _maxScore;
    [SerializeField] private List<AnimalRandom> animalRandoms;
    [SerializeField] private int scoreCount;


    private void Awake()
    {
       animalRandoms =  GetAnimalRandomList(_maxScore, 5, 5, 4);
    }

    public List<AnimalRandom> GetAnimalRandomList(int maxScore, int minChicken, int minPig, int minHegdhog)
    {   
        
        List<AnimalRandom> listResult = new List<AnimalRandom>();
        AnimalRandom chicken = new AnimalRandom("chicken", 1);
        AnimalRandom pig = new AnimalRandom("pig", 2);
        AnimalRandom hegdhog = new AnimalRandom("hegdhog", 3);
        
        listResult.Add(chicken);
        listResult.Add(pig);
        listResult.Add(hegdhog);
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < listResult.Count; j++)
            {
                scoreCount += listResult[i].score;
                listResult[i].amount ++;

                //validate
#if UNITY_EDITOR
                if(scoreCount > maxScore) {
                    EditorUtility.DisplayDialog("OOP!","Total score greater than " + maxScore,"Ok");
                    throw new System.Exception("total score greater than " + maxScore);
                }

                if(scoreCount == 0 && maxScore > 0) {
                    EditorUtility.DisplayDialog("OOP!","Score count is zero","Ok");
                    throw new System.Exception("score count is zero");
                }
#endif
                ////
                
            }
        }

        while(scoreCount < maxScore)
        {
            int randomIndex = Random.Range(0, listResult.Count);
            while(listResult[randomIndex].score + scoreCount > maxScore)
            {
                randomIndex = Random.Range(0, listResult.Count);
            }
            listResult[randomIndex].amount++;
            scoreCount += listResult[randomIndex].score;
        }


        return listResult;
    }

}

public enum AnimalType {
    Chicken,
    Pig,
    Hedgehog
}

[System.Serializable]
public class AnimalRandom
{
    public string name;
    public AnimalType type;
    public int amount;
    [HideInInspector] public int score;

    public AnimalRandom(string name, int score) {
        this.name = name;
        this.score = score;
    }
}

