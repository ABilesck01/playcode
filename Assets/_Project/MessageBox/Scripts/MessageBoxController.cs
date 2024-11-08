using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageBoxController : MonoBehaviour
{
    public GameObject messageBoxPanel; // Painel que será ativado/desativado
    public TextMeshProUGUI titleText; // Texto que exibirá a mensagem
    public TextMeshProUGUI messageText; // Texto que exibirá a mensagem
    public Button okButton; // Botão de OK
    public Button cancelButton; // Botão de Cancelar (opcional)
    public GameObject sellInfo;
    public UnityEngine.UI.Image sellIcon;
    public TextMeshProUGUI txtPrice;
    public TextMeshProUGUI txtOkButton; // Texto que exibirá a mensagem
    public TextMeshProUGUI txtCancelButton; // Texto que exibirá a mensagem

    private UnityAction okAction; // Ação a ser executada ao clicar em OK
    private UnityAction cancelAction; // Ação a ser executada ao clicar em Cancelar (opcional)

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

        this.okAction = okAction; // Define a ação de OK
        this.cancelAction = cancelAction; // Define a ação de Cancelar (opcional)

        // Adiciona os listeners aos botões
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

    // Método chamado ao clicar no botão OK
    private void OnOkButtonClick()
    {
        okAction?.Invoke(); // Executa a ação de OK
        CloseMessageBox(); // Fecha a MessageBox
    }

    // Método chamado ao clicar no botão Cancelar
    private void OnCancelButtonClick()
    {
        cancelAction?.Invoke(); // Executa a ação de Cancelar (opcional)
        CloseMessageBox(); // Fecha a MessageBox
    }

    // Método para fechar a MessageBox
    public void CloseMessageBox()
    {
        sellInfo.SetActive(false);
        messageBoxPanel.SetActive(false); // Desativa o painel
    }
}
