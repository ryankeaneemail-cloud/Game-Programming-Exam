using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using SimpleJSON;

[System.Serializable]
public class PlantCount
{
    public int carrotCount, cabbageCount, pumpkinCount;
}

public class GameManager : MonoBehaviour
{
    public GameObject welcomeUI;

    public static GameManager instance;

    public PlantCount plantCount = new PlantCount();

    public TextMeshProUGUI carrotText, cabbageText, pumpkinText;

    private string filePath = Application.streamingAssetsPath + "/PlantCount.json";

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isPaused = true;
        Time.timeScale = 0;
        LoadJsonValue();
    }

    // Update is called once per frame
    void Update()
    {
        ShowPlantAmount();

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    void LoadJsonValue()
    {
        string contents = File.ReadAllText(filePath);
        plantCount =  JsonUtility.FromJson<PlantCount>(contents);
    }

    void ShowPlantAmount()
    {
        pumpkinText.text = plantCount.pumpkinCount.ToString();
        carrotText.text = plantCount.carrotCount.ToString();
        cabbageText.text = plantCount.cabbageCount.ToString();
    }

    public void AddCropAmount(CropName name)
    {
        if (name == CropName.Carrot)
        {
            plantCount.carrotCount++;
        }
        else if (name == CropName.Cabbage)
        {
            plantCount.cabbageCount++;
        }
        else if (name == CropName.Pumpkin)
        {
            plantCount.pumpkinCount++;
        }

        string json = JsonUtility.ToJson(plantCount);
        File.WriteAllText(filePath, json);
    }

    public void PlayGame()
    {
        welcomeUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    public void PauseGame() 
    {
        welcomeUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
}
