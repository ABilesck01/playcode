using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour
{
    public GameObject messageBoxPanel; // Painel que ser� ativado/desativado
    public TextMeshProUGUI titleText; // Texto que exibir� a mensagem
    public TextMeshProUGUI messageText; // Texto que exibir� a mensagem
    public Button okButton; // Bot�o de OK
    public Button cancelButton; // Bot�o de Cancelar (opcional)
    public GameObject sellInfo;
    public UnityEngine.UI.Image sellIcon;
    public TextMeshProUGUI txtPrice;
    public TextMeshProUGUI txtOkButton; // Texto que exibir� a mensagem
    public TextMeshProUGUI txtCancelButton; // Texto que exibir� a mensagem

    private UnityAction okAction; // A��o a ser executada ao clicar em OK
    private UnityAction cancelAction; // A��o a ser executada ao clicar em Cancelar (opcional)

    public static MessageBoxController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void ShowMessage(string title, string message, UnityAction okAction = null, UnityAction cancelAction = null, string okText = "Ok", string cancelText = "Cancelar")
    {
        sellInfo.SetActive(false);

        messageBoxPanel.SetActive(true); // Ativa o painel
        titleText.text = title; // Define a mensagem
        messageText.text = message; // Define a mensagem

        this.okAction = okAction; // Define a a��o de OK
        this.cancelAction = cancelAction; // Define a a��o de Cancelar (opcional)

        // Adiciona os listeners aos bot�es
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(OnOkButtonClick);

        if(cancelAction == null)
            cancelButton.gameObject.SetActive(false);
        else
            cancelButton.gameObject.SetActive(true);

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelButtonClick);
            cancelButton.gameObject.SetActive(true);
        }

        txtOkButton.text = okText;
        txtCancelButton.text = cancelText;
    }

    public void ShowSellMessage(string title, string message, Sprite icon, string value, UnityAction okAction = null, UnityAction cancelAction = null, string okText = "Ok", string cancelText = "Cancelar")
    {
        ShowMessage(title, message, okAction, cancelAction, okText, cancelText);

        sellInfo.SetActive(true);

        sellIcon.sprite = icon;
        txtPrice.text = value;

        
    }

    // M�todo chamado ao clicar no bot�o OK
    private void OnOkButtonClick()
    {
        okAction?.Invoke(); // Executa a a��o de OK
        CloseMessageBox(); // Fecha a MessageBox
    }

    // M�todo chamado ao clicar no bot�o Cancelar
    private void OnCancelButtonClick()
    {
        cancelAction?.Invoke(); // Executa a a��o de Cancelar (opcional)
        CloseMessageBox(); // Fecha a MessageBox
    }

    // M�todo para fechar a MessageBox
    public void CloseMessageBox()
    {
        sellInfo.SetActive(false);
        messageBoxPanel.SetActive(false); // Desativa o painel
    }
}
