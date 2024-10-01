using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExploreGamesViewItem : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtAuthor;
    public Button playButton;

    private int levelID;

    public void Setup(string name, string author, int levelID)
    {
        playButton.onClick.AddListener(PlayLevel);

        txtName.text = name;
        txtAuthor.text = author;
        this.levelID = levelID;
    }

    public void PlayLevel()
    {
        if (PersistentGameData.level == null)
            PersistentGameData.level = new LevelDTO();

        PersistentGameData.level.id = levelID;
        SceneManager.LoadScene("PlayGame");
    }
}
