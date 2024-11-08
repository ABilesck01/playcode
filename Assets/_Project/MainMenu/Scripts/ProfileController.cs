using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image[] currentPlayerIcon;
    [SerializeField] private TextMeshProUGUI txtUserName;
    [SerializeField] private TextMeshProUGUI txtEmail;
    [Space]
    [SerializeField] private Button btnLogout;
    [SerializeField] private Button btnCancel;
    [SerializeField] private Button btnSave;
    [SerializeField] private Button btnBack;
    [Space]
    [SerializeField] private Sprite[] playerSprites;
    [Space]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemContainer;
    int currentPlayer = 0;

    private void Awake()
    {
        btnLogout.onClick.AddListener(Logout);
        btnCancel.onClick.AddListener(Cancel);
        btnSave.onClick.AddListener(Save);
    }

    private void Start()
    {
        UpdateGrid();
        txtUserName.text = PersistentGameData.usuario.Nome;
        txtEmail.text = PersistentGameData.usuario.Email;
    }

    private void UpdateGrid()
    {
        foreach (Transform item in itemContainer)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < playerSprites.Length; i++) 
        {
            var newItem = Instantiate(itemPrefab, itemContainer);
            ProfileControllerItem profileControllerItem = newItem.GetComponent<ProfileControllerItem>();
            profileControllerItem.Setup(playerSprites[i], i, PersistentGameData.usuario.Avatares.Contains(i));
            profileControllerItem.SetProfileController(this);

        }
    }

    private void UpdatePlayerIcon()
    {
        for (int i = 0; i < currentPlayerIcon.Length; i++)
        {
            currentPlayerIcon[i].sprite = playerSprites[currentPlayer];
        }
    }

    public void SelectPlayer(int index)
    {
        currentPlayer = index;
        UpdatePlayerIcon();
    }

    private void Logout()
    {
        PersistentGameData.Logout();
        SceneManager.LoadScene("Login");
    }

    private void Save()
    {
        ApiController.instance.SendRequest<int>(RequestType.POST, $"{PersistentGameData.usuario.ID}/set-avatar",
            success =>
            {
                btnBack.onClick.Invoke();
            },
            error =>
            {
                MessageBoxController.instance.ShowMessage("Erro", error);
            });
    }

    private void Cancel()
    { 
        btnBack.onClick.Invoke(); 
    }

}
