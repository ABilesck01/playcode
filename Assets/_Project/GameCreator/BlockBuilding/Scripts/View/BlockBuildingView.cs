using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBuildingView : MonoBehaviour
{
    [SerializeField] private BlockAssetList list;
    [SerializeField] private Transform container;
    [SerializeField] private BlockBuildingViewItem viewItemPrefab;
    [Space]
    [SerializeField] private Button btnBrush;
    [SerializeField] private Button btnEraser;
    [Header("Hide objects on play")]
    [SerializeField] private GameObject[] objectsToHide;

    private void Start()
    {
        foreach (var item in list.blockAssets)
        {
            var itemInstance = Instantiate(viewItemPrefab, container);
            itemInstance.Setup(item);
        }

        btnBrush.onClick.AddListener(() => BlockBuildingSystem.instance.SetBrush(BlockBuildingSystem.BrushType.brush));
        btnEraser.onClick.AddListener(() => BlockBuildingSystem.instance.SetBrush(BlockBuildingSystem.BrushType.eraser));

    }

    private void OnEnable()
    {
        LevelController.OnStartLevel += LevelController_OnStartLevel;
        LevelController.OnStopLevel += LevelController_OnStopLevel;
    }

    private void OnDisable()
    {
        LevelController.OnStartLevel -= LevelController_OnStartLevel;
        LevelController.OnStopLevel -= LevelController_OnStopLevel;
    }

    private void LevelController_OnStopLevel(object sender, System.EventArgs e)
    {
        ToggleObjects(true);
    }

    private void LevelController_OnStartLevel(object sender, System.EventArgs e)
    {
        ToggleObjects(false);
    }

    private void ToggleObjects(bool show)
    {
        for (int i = 0; i < objectsToHide.Length; i++)
        {
            objectsToHide[i].SetActive(show);
        }
    }

}
