using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#region 1st CharacterSelection

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

#endregion

#region SubCharacteDetail
[System.Serializable]
public struct SubCharacterSelect
{

    public string Title;
    public string Name;
    public int Stars;
    public string description;
}
[System.Serializable]
public struct SubCharacterDetail
{
    public TMP_Text SelectedCharactername;
    public TMP_Text SelectedTitle;
    public TMP_Text SelectedName;
    public List<GameObject> Selectedstars;
    public TMP_Text Selecteddescription;
    public List<TMP_Text> SubCharactersubName;
    
} 
#endregion
public class MenuManag : MonoBehaviour
{

    public static MenuManag instance;


    #region CharacterSelection
    public List<CharacterMainStruct> charactermain;
    public List<SelectedCharacterDetail> selectedChrDetail;
    #endregion

    #region SubCharacterSelection
    public List<SubCharacterSelect> subCharacterSelect;
    public List<SubCharacterDetail> subCharacterDetail; 
    #endregion



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

    public void SubCharacterSelection(int SubCharacterNumber)
    {
        subCharacterDetail[0].SelectedCharactername.text = "Char # " + SubCharacterNumber;
        subCharacterDetail[0].SelectedTitle.text = subCharacterSelect[SubCharacterNumber].Title;
        subCharacterDetail[0].SelectedName.text = subCharacterSelect[SubCharacterNumber].Name;
        subCharacterDetail[0].Selecteddescription.text = subCharacterSelect[SubCharacterNumber].description;
        for (int i = 0; i < subCharacterDetail[0].SubCharactersubName.Count; i++)
        {
            subCharacterDetail[0].SubCharactersubName[i].text = "Char # " + SubCharacterNumber;
        }
        for (int i = 0; i < 5; i++)
        {
            subCharacterDetail[0].Selectedstars[i].SetActive(false);
        }
        for (int i = 0; i < subCharacterSelect[SubCharacterNumber].Stars; i++)
        {
            subCharacterDetail[0].Selectedstars[i].SetActive(true);
        }
    }
}
