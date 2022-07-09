using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Main;
using static UsefulMethod;
using static GameControllerUI;


/// <summary>
/// NOTE:セーブの前にセーブの処理を止める必要がある
/// </summary>
public class SaveManager : MonoBehaviour
{
    [SerializeField] string sceneName = "Main";
    //UI
    [SerializeField] Button loadButton;
    [SerializeField] Button saveButton;
    [SerializeField] Button steam_loadButton;
    [SerializeField] Button steam_saveButton;
    [SerializeField] TextAsset editorSaveData;
    PopupUI popupUI, popupUILocalLoad;
    ConfirmUI confirmUI;

    //SaveData
    public List<ISaveElement> saveDataList = new List<ISaveElement>();

    //SaveExecutor
    ISaveExecutor local_executor;
    ISaveExecutor playfab_executor;

    private void Start()
    {
        
        //セーブデータの設定
        saveDataList.Add(new SaveElement<SaveR>(main.SR, (x) => LoadFunc(ref main.SR, x)));
        saveDataList.Add(new SaveElement<Save>(main.S, (x) => LoadFunc(ref main.S, x)));

        // Local
        ISaveLocation<string> local_location = new LocalAndEditorLocation("IEH2", gameObject.name, editorSaveData);
        local_executor = new SaveExecutor(saveDataList, local_location, true)
            { LoadAction = AfterOverload };
        //loadButton.onClick.AddListener(BeforeLoadAction);
        loadButton.onClick.AddListener(local_executor.Load);
        saveButton.onClick.AddListener(local_executor.Save);

        //STEAM
        ISaveLocation<string> cloud_location = new SteamSave("IEH2", gameObject.name);
        var steam_executor = new SaveExecutor(saveDataList, cloud_location, true)
        { LoadAction = AfterOverload};
        steam_executor.SaveFailureAction = () => { confirmUI.SetUI("Failed in Cloud Save.\n<size=18>Please check if you are connected to the internet and try again after a while."); SetActive(confirmUI.okButton.gameObject, false); };
        steam_executor.SaveSuccessAction = () => { confirmUI.SetUI("Succeeded in Cloud Save!"); SetActive(confirmUI.okButton.gameObject, false); };
        //steam_executor.LoadFailureAction = () => { Debug.Log("failed"); GameControllerUI.gameUI.logCtrlUI.Log("faild"); };
        //steam_executor.LoadAction = () => {  };
        //Debug.Log(UnityEngine.Application.persistentDataPath);//MacユーザーのLocalSaveの保存先
        //steam_loadButton.onClick.AddListener(BeforeLoadAction);
        steam_loadButton.onClick.AddListener(
            () =>
            {
                confirmUI.SetUI("Are you sure to import a save from Steam Server right now?\n<color=yellow>Please reboot the game after importing the save to stabilize the game performance.</color>");
                SetActive(confirmUI.okButton.gameObject, true);
                confirmUI.okButton.onClick.RemoveAllListeners();
                confirmUI.okButton.onClick.AddListener(() => steam_executor.Load());
                //steam_executor.Load(); //Confirmにかいた
            }
            );
        steam_saveButton.onClick.AddListener(steam_executor.Save);
        //StartCoroutine(PersistCloudSave());

        popupUI = new PopupUI(gameUI.popupCtrlUI.defaultPopup);
        popupUILocalLoad = new PopupUI(gameUI.popupCtrlUI.defaultFixedWidthPopup);
        confirmUI = new ConfirmUI(gameUI.popupCtrlUI.defaultConfirm);
        //confirmUI.okButton.onClick.AddListener(() => steam_executor.Load());
        localLoadPopupString = LocalLoadPopupString();
        popupUILocalLoad.SetTargetObject(loadButton.gameObject, () => localLoadPopupString);
        popupUI.SetTargetObject(saveButton.gameObject, () => "<size=20>Export the current save file to your computer");
        popupUILocalLoad.SetTargetObject(steam_loadButton.gameObject, () => CloudLoadPopupString());
        popupUI.SetTargetObject(steam_saveButton.gameObject, () => "<size=20>Export the current save file to Steam Server");
    }

    string localLoadPopupString;
    string LocalLoadPopupString()
    {
        string tempStr = optStrL + "<size=20>Import a local save file from your computer" +
            "\n<size=18><color=yellow>To stabilize the game performance, it is recommended to reboot the game after importing the save file.</color>";
#if UNITY_STANDALONE_WIN
        return tempStr;
#endif
        tempStr += "\n- For Mac Users, you need to put \"localsave.txt\" file in " + UnityEngine.Application.persistentDataPath;
        return tempStr;
    }
    string CloudLoadPopupString()
    {
        string tempStr = optStrL + "<size=20>Import a save file from Steam Server" +
            //"\n<size=18>Last Cloud Save Date : " + lastCloudSaveDate + 
            "\n<size=18><color=yellow>To stabilize the game performance, it is recommended to reboot the game after importing the save file.</color>";
        return tempStr;
    }
    public void UpdateUI()
    {
        popupUI.UpdateUI();
        popupUILocalLoad.UpdateUI();
    }

    //IEnumerator PersistCloudSave()
    //{
    //    //if(main.platform != Platform.steam)
    //    //{
    //    //    setFalse(steam_saveButton.gameObject);
    //    //    setFalse(steam_loadButton.gameObject);
    //    //    yield break;
    //    //}
    //    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(300f);
    //    while (true)
    //    {
    //        yield return wait;
    //        steam_saveButton.onClick.Invoke();
    //    }
    //}

    void BeforeLoadAction()
    {
        //Debug.Log("よばれてるよ");
        //StopAllCoroutines();
        //Time.timeScale = 0;
    }

    // オブジェクトに代入した後に呼ぶ関数
    void AfterOverload()
    {
        main.saveCtrl.setSaveKey(); //NOTE:saveCtrlに依存させないようにする
        SceneManager.LoadScene(sceneName);
    }


    void LoadFunc<T>(ref T obj, string save_data)
    {
        obj = JsonUtility.FromJson<T>(save_data);
    }
}
