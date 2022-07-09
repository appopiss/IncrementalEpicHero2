using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrlDiscord : MonoBehaviour
{
    public void onClick()
    {
#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX)
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", "https://discord.gg/t39nrGB"));
        return;
#endif

            Application.OpenURL("https://discord.gg/t39nrGB");//""の中には開きたいWebページのURLを入力します
    }

}
