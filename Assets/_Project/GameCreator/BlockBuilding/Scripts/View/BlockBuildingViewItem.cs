using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBuildingViewItem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image preview;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(BlockAsset asset)
    {
        this.preview.sprite = asset.preview;
        button.onClick.AddListener(() =>
        {
            BlockBuildingSystem.instance.SetBlockAsset(asset);
        });
    }

}
