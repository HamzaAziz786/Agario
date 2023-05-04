using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManag : MonoBehaviour
{
    public static MenuManag instance;

    public List<Image> CharactersImages;
    public Image CharacterImg;

    private void Awake()
    {
        instance = this;
    }
    public void SetImageOnClick(int imageNumber)
    {
        CharacterImg.sprite = CharactersImages[imageNumber].sprite;
    }
}
