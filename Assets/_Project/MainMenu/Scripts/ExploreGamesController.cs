using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExploreGamesController : MonoBehaviour
{
    [Header("All projects")]
    public Transform container;
    public ExploreGamesViewItem projectPrefab;

    private void Start()
    {
        ApiController.instance.SendRequest<List<GetLevelDTO>>(RequestType.GET, "UserLevel", OnSuccess, OnError);
    }

    private void OnError(string obj)
    {
        MessageBoxController.instance.ShowMessage("Erro", obj);
    }

    private void OnSuccess(List<GetLevelDTO> list)
    {
        var orderedList = list.OrderByDescending(levelData => levelData.trofeus).ToList();

        foreach (var levelData in orderedList)
        {
            if (levelData.usuarioID == PersistentGameData.usuario.ID)
                continue;

            var item = Instantiate(projectPrefab, container);
            item.Setup(levelData.nome, $"{levelData.usuarioNome} | {levelData.trofeus}", levelData.id);
        }
    }

}
