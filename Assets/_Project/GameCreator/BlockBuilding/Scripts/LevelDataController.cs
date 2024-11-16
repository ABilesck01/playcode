using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelDataController : MonoBehaviour
{
    [SerializeField] private TMP_InputField txtLevelName;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [Space]
    public LevelData data;
    public BlockAssetList blockAssetList;

    public static event EventHandler OnLoadLevel;

    // Start is called before the first frame update
    void Start()
    {
        if (PersistentGameData.level == null)
            return;
        Debug.Log($"Editing level {PersistentGameData.level.nome}");
        LoadData();
    }

    public void SaveData()
    {
        SaveLevelDto saveLevelDto = new SaveLevelDto();
        saveLevelDto.userLevelId = PersistentGameData.level.id;
        saveLevelDto.blocos = data.blocos;
        saveLevelDto.variaveis = VariableController.instance.GetAllVariables().ToArray();
        ApiController.instance.SendRequest<string>(RequestType.POST, "UserLevel/save-level", OnSaveUserLevel, OnError, saveLevelDto);
    }

    private void OnSaveUserLevel(string t)
    {
        Debug.Log($"Saved Data {t}");
    }

    private void LoadData()
    {
        if (PersistentGameData.level.id == 0)
            return;

        ApiController.instance.SendRequest<GetLevelDto>(RequestType.GET, $"UserLevel/{PersistentGameData.level.id}", OnLoadData, OnError);
    }

    private void OnLoadData(GetLevelDto data)
    {
        Debug.Log(data);
        this.data = new LevelData
        {
            id = data.id,
            nome = data.nome,
            usuarioNome = data.usuarioNome,
            blocos = data.blocos.ToList()
        };
        foreach (var item in data.blocos)
        {
            BlockAsset blockAsset = blockAssetList.GetBlock(item.id_Interno);

            if (blockAsset == null)
            {
                Debug.LogError($"No block with id {item.id_Interno} was found");
                continue;
            }
            var block = BlockBuildingSystem.instance.SystemPlaceBlock(blockAsset, item.x, item.y);
            if (block is EventBlock && !string.IsNullOrEmpty(item.customData))
            {

                var eventJson = item.customData;
                Debug.Log(eventJson);
                ((EventBlock)block).DeserializeFromJson(eventJson);
            }
        }



        OnLoadLevel?.Invoke(this, EventArgs.Empty);
    }

    private void OnError(string obj)
    {
        Debug.LogError(obj);
    }

    private void OnEnable()
    {
        BlockBuildingSystem.OnPlaceBlock += BlockBuildingSystem_OnPlaceBlock;
        BlockBuildingSystem.OnEraseBlock += BlockBuildingSystem_OnEraseBlock;
        EventBlock.OnActionAdded += EventBlock_OnActionAdded;
        EventBlock.OnActionUpdated += EventBlock_OnActionAdded;
        VariableController.onVariablesChanged += VariableController_onVariablesChanged;
    }

    private void OnDisable()
    {
        BlockBuildingSystem.OnPlaceBlock -= BlockBuildingSystem_OnPlaceBlock;
        BlockBuildingSystem.OnEraseBlock -= BlockBuildingSystem_OnEraseBlock;
        EventBlock.OnActionAdded -= EventBlock_OnActionAdded;
        EventBlock.OnActionUpdated -= EventBlock_OnActionAdded;
        VariableController.onVariablesChanged += VariableController_onVariablesChanged;
    }

    private void VariableController_onVariablesChanged()
    {
        List<string> variableNames = new List<string>();
        foreach (var item in VariableController.instance.Dict)
        {
            variableNames.Add(item.key);
        }

        //data.variables = variableNames;
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
        foreach (var item in data.blocos)
        {
            if(item.x == e.x && item.y == e.y && item.id_Interno == e.blockSO.ID) 
            {
                item.customData = e.GetData();
            }
        }
    }
}

[System.Serializable]
public class GetLevelDto
{
    public int id;
    public string nome;
    public string usuarioNome;
    public List<Bloco> blocos;
}

public class Message
{
    public string message = "";
}