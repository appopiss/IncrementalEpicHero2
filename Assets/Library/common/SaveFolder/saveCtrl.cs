using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static Main;
using Cysharp.Threading.Tasks;

//[DefaultExecutionOrder(-2)]
public class saveCtrl : MonoBehaviour
{
    void getSaveKey()
    {
        //SaveR
        if (saveClass.GetObject<SaveR>(keyList.resetSaveKey) == null)
        {
            main.SR = new SaveR();
        }
        else
        {
            main.SR = saveClass.GetObject<SaveR>(keyList.resetSaveKey);
        }
        //Save
        if (saveClass.GetObject<Save>(keyList.permanentSaveKey) == null)
        {
            main.S = new Save();
        }
        else
        {
            main.S= saveClass.GetObject<Save>(keyList.permanentSaveKey);
        }

        //Version//正式リリース後はこれは消す
        if (main.S.version < 00030000 && !Input.GetKey(KeyCode.LeftShift))
        {
            bool isPlaytestBeforeJune10 = false;
            if (main.S.version > 0)//2022/06/10以前にPlaytestに参加した人
            {
                isPlaytestBeforeJune10 = true;
            }
            main.SR = new SaveR();
            main.S = new Save();
            main.S.isPlaytestBeforeJune10 = isPlaytestBeforeJune10;
        }
        main.S.version = version;
    }

    public void setSaveKey()
    {
        saveClass.SetObject(keyList.resetSaveKey, main.SR);
        saveClass.SetObject(keyList.permanentSaveKey, main.S);
    }


    private void Awake()
    {
        getSaveKey();
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    SavePerSec();
    //}

    public void SavePerSec()
    {
        setSaveKey();
    }

    //async void SavePerSec()
    //{
    //    while (true)
    //    {
    //        Debug.Log("a");
    //        await UniTask.DelayFrame(60);
    //        setSaveKey();
    //    }
    //}
}
