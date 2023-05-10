using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct CharacterMainStruct
{
    public Sprite CharacterImage;
    public string Title;
    public string Name;
    public int Stars;
    public string description;
}
[System.Serializable]
public struct SelectedCharacterDetail
{
    public Image SelectedCharacterImage;
    public TMP_Text SelectedTitle;
    public TMP_Text SelectedName;
    public List<GameObject> Selectedstars;
    public TMP_Text Selecteddescription;
}
public class MenuManag : MonoBehaviour
{
   
    public static MenuManag instance;
    public List<CharacterMainStruct> charactermain;
    public List<SelectedCharacterDetail>selectedChrDetail;
    

   
    
    private void Awake()
    {
        instance = this;
    }
    public void SelectCharacter(int CurrentSelectedCharacter)
    {
        selectedChrDetail[0].SelectedCharacterImage.sprite = charactermain[CurrentSelectedCharacter].CharacterImage;
        selectedChrDetail[0].SelectedTitle.text = charactermain[CurrentSelectedCharacter].Title;
        selectedChrDetail[0].SelectedName.text = charactermain[CurrentSelectedCharacter].Name;
        selectedChrDetail[0].Selecteddescription.text = charactermain[CurrentSelectedCharacter].description;
        for (int i = 0; i < 5; i++)
        {
            selectedChrDetail[0].Selectedstars[i].SetActive(false);
        }
        for (int i = 0; i < charactermain[CurrentSelectedCharacter].Stars; i++)
        {
            selectedChrDetail[0].Selectedstars[i].SetActive(true);
        }
        Debug.Log("Stars" + charactermain[CurrentSelectedCharacter].Stars);
    }
}
