using System.Collections;
using System.Collections.Generic;
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
    public Image CharacterImage;
    public Text Title;
    public Text Name;
    public List<GameObject> stars;
    public Text description;
}
public class MenuManag : MonoBehaviour
{
   
    public static MenuManag instance;
    public List<CharacterMainStruct> charactermain;
    public List<SelectedCharacterDetail> selectedChrDetail;
    

   
    
    private void Awake()
    {
        instance = this;
    }
    public void SetImageOnClick(int imageNumber)
    {
        
    }
}
