using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft;
using Newtonsoft.Json;

public class LevelDataController : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtLevelName;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [Space]
    public LevelData data;
    public BlockAssetList blockAssetList;

    // Start is called before the first frame update
    void Start()
    {
        data = new LevelData();
        saveButton.onClick.AddListener(SaveData);
        loadButton.onClick.AddListener(LoadData);
    }

    private void SaveData()
    {
        string folderPath = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}/PlayCode";

        // Check if the directory exists, if not, create it
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string json = JsonConvert.SerializeObject(data);

        System.IO.File.WriteAllText(folderPath + $"/{txtLevelName.text}.json", json);
    }

    private void LoadData()
    {
        string folderPath = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}/PlayCode";
        string filePath = $"{folderPath}/{txtLevelName.text}.json";

        // Check if the file exists before trying to load it
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            data = JsonConvert.DeserializeObject<LevelData>(json);

            foreach (var item in data.blocks)
            {
                var block = BlockBuildingSystem.instance.SystemPlaceBlock(blockAssetList.GetBlock(item.assetId), item.x, item.y);
                if(block is EventBlock)
                {
                    var eventJson = item.customData.ToString();
                    ((EventBlock)block).DeserializeFromJson(eventJson);
                }
            }
        }
        else
        {
            // Handle the case where the file doesn't exist
            Debug.LogWarning("File does not exist.");
        }
    }

    private void OnEnable()
    {
        BlockBuildingSystem.OnPlaceBlock += BlockBuildingSystem_OnPlaceBlock;
        BlockBuildingSystem.OnEraseBlock += BlockBuildingSystem_OnEraseBlock;
        EventBlock.OnActionAdded += EventBlock_OnActionAdded;
        EventBlock.OnActionUpdated += EventBlock_OnActionAdded;
    }

    private void OnDisable()
    {
        BlockBuildingSystem.OnPlaceBlock -= BlockBuildingSystem_OnPlaceBlock;
        BlockBuildingSystem.OnEraseBlock -= BlockBuildingSystem_OnEraseBlock;
        EventBlock.OnActionAdded -= EventBlock_OnActionAdded;
        EventBlock.OnActionUpdated -= EventBlock_OnActionAdded;
    }

    private void BlockBuildingSystem_OnEraseBlock(object sender, BlockBuildingSystem.BlockEventArgs e)
    {
        data.RemoveBlock(e.block);
    }

    private void BlockBuildingSystem_OnPlaceBlock(object sender, BlockBuildingSystem.BlockEventArgs e)
    {
        data.AddBlock(e.block);
    }

    private void EventBlock_OnActionAdded(object sender, EventBlock e)
    {
        foreach (var item in data.blocks)
        {
            if(item.x == e.x && item.y == e.y && item.assetId == e.blockSO.Id) 
            {
                item.customData = e.GetData();
            }
        }
    }
}
