using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateProjectController : MonoBehaviour
{
    [Header("New project")]
    public Button btnNewProject;
    public Button btnCreateProject;
    public TMP_InputField txtProjectName;
    [Header("Current projects")]
    public Transform container;
    public projectItem projectPrefab;

    private void Awake()
    {
        btnCreateProject.onClick.AddListener(BtnCreateProjectClick);
    }

    private void Start()
    {
        GetUserProjects();
    }

    private void GetUserProjects()
    {
        ApiController.instance.SendRequestArray<LevelDTO>(RequestType.GET, $"UserLevel/basic-info/{PersistentGameData.usuario.ID}", OnGetProjects, OnError);
    }

    private void OnGetProjects(LevelDTO[] list)
    {
        foreach (LevelDTO level in list)
        {
            projectItem project = Instantiate(projectPrefab, container);
            project.SetLevelDTO(level);
        }
    }

    public void BtnCreateProjectClick()
    {
        btnCreateProject.interactable = false;
        txtProjectName.interactable = false;

        NovoLevelDTO novoLevelDTO = new NovoLevelDTO()
        {
            userId = PersistentGameData.usuario.ID,
            nome = txtProjectName.text
        };

        ApiController.instance.SendRequest<LevelDTO>(RequestType.POST, "UserLevel/create", OnSuccess, OnError, novoLevelDTO);
    }

    private void OnError(string obj)
    {
        Debug.LogError(obj);
    }

    private void OnSuccess(LevelDTO dto)
    {
        Debug.Log(dto.nome);
        PersistentGameData.level = dto;
        SceneManager.LoadScene("CreateGame");
    }
}

[System.Serializable]
public class NovoLevelDTO
{
    [SerializeField] public int userId;
    [SerializeField] public string nome;
}


[System.Serializable]
public class LevelDTO
{
    [SerializeField] public int id;
    [SerializeField] public string nome;
}

[System.Serializable]
public class GetLevelDTO
{
    [SerializeField] public int id;
    [SerializeField] public string nome;
    [SerializeField] public int usuarioID;
    [SerializeField] public string usuarioNome;
}
