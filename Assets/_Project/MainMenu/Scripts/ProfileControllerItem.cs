using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileControllerItem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image icon;
    [SerializeField] private GameObject locked;
    [SerializeField] private Button button;

    private ProfileController profileController;
    private int index;
    private bool owned;

    private void Start()
    {
        button.onClick.AddListener(OnSelectClick);
    }

    public void SetProfileController(ProfileController profileController)
    {
        this.profileController = profileController;
    }

    private void OnSelectClick()
    {
        if(owned)
            SelectPlayer();
        else
            UnlockPlayer();
    }

    public void Setup(Sprite player, int index, bool owned)
    {
        this.owned = owned;
        locked.SetActive(!owned);
        icon.sprite = player;
        this.index = index;
    }

    private void SelectPlayer()
    {
        profileController.SelectPlayer(index);
    }

    private void UnlockPlayer()
    {
        MessageBoxController.instance.ShowSellMessage("Comprar", "Você deseja comprar este dinossauro?", icon.sprite, "50",
            () =>
            {
                profileController.UnlockPlayer(index);
            },
            null, "Sim", "Não");
    }
}
