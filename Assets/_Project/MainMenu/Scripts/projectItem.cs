using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class projectItem : MonoBehaviour
{
    public TextMeshProUGUI txtProjectName;
    public Button buttonEdit;

    private LevelDTO level;

    private void BtnEditClick()
    {
        PersistentGameData.level = level;
        SceneManager.LoadScene("CreateGame");
    }

    public void SetLevelDTO(LevelDTO level)
    {
        this.level = level;
        txtProjectName.text = level.nome;

        buttonEdit.onClick.AddListener(BtnEditClick);
    }
}
