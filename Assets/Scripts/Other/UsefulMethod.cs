﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine.Events;
//using MathNet.Numerics.Distributions;
using static System.Math;
using StrOpe = StringOperationUtil.OptimizedStringOperation;

public class UsefulMethod : MonoBehaviour
{
    public static Color purple = new Color(200 / 255f, 100 / 255f, 255 / 255f);
    public static Color green = new Color(0, 150 / 255f, 25 / 255f);
    public static Color orange = new Color(255 / 255f, 180 / 255f, 0 / 255f);
    public static Color halftrans = new Color(255f / 255, 255f / 255, 255f / 255, 128f / 255);
    // Start is called before the first frame update
    /// <summary>
    /// テキストを指定するだけで，マウスオーバーでテキストを表示させる関数．
    /// </summary>
    public GameObject windowPre;
    public Transform windowTransform;
    public static GameObject window;

    public delegate void usefuleDelegate();
    public static StrOpe optStr { get => StrOpe.i; }
    public static StrOpe optStrL { get => StrOpe.large; }
    public static void setOnPointerClick(ref GameObject game)
    {
        game.AddComponent<EventTrigger>().triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        game.GetComponent<EventTrigger>().triggers.Add(entry);
    }

    public static void SetActive(GameObject gameObject, bool toActive)
    {
        if (gameObject.activeSelf == toActive) return;
        gameObject.SetActive(toActive);
    }

    public static void setActive(GameObject go)
    {
        if (!go.activeSelf)
        {
            go.SetActive(true);
        }
    }

    public static void setFalse(GameObject go)
    {
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
    }
    /// <summary>
    /// 桁数が大きいものを，アルファベット表記に変える関数
    /// </summary>
    static string[] digit = new string[]
{
       "", "K","M","B","T","Qa","Qi","Sx","Sp","Oc","No","Dc",
        "Ud","Dd","Td","Qad","Qid","Sxd","Spd","Ods","Nod","Vg","Uvg","Dvg","Tvg","Qavg","Qivg","Sxvg","Spvg","Ocvg","Novg",
       "Tg","Utg","Dtg","Ttg","Qatg","Qitg","Sxts","Sptg","Octg","Notg","Qag", "Uqag", "Dqag", "Tqag", "Qaqag", "Qiqag", "Sxqag", "Spqag", "Ocqag", "Noqag",
        "Qig", "UQig, DQig", "TQig", "QaQig", "QiQig", "SxQig", "SpQig", "OcQig", "NoQig", "Sxg", "USxg", "DSxg", "TSxg", "QaSxg", "QiSxg", "SxSxg", "SpSxg", "OcSxg", "NoSxg",
       "Spg", "USpg", "DSpg", "TSpg", "QaSpg", "QiSpg", "SxSpg", "SpSpg", "OcSpg", "NoSpg", "Ocg", "UOcg", "DOcg", "TOcg", "QaOcg", "QiOcg", "SxOcg", "SpOcg", "OcOcg", "NoOcg",
        "Nog", "UNog", "DNog", "TNog", "QaNog", "QiNog", "SxNog", "SpNog", "OcNog", "NoNog", "C", "Uc"
};
    static StringBuilder digit_builder = new StringBuilder(100);
    static int log_1000_tdigit;
    static double head_value_tdigit;
    static string argument_ToString_tdigit = "";
    static bool isMinus_tdigit;
    public enum NumberType
    {
        normal,
        percent,
        meter,
    }
    public static string tDigit(double value, NumberType numberType)
    {
        switch (numberType)
        {
            case NumberType.normal:
                return tDigit(value, 1);
            case NumberType.percent:
                return percent(value, 4);
            case NumberType.meter:
                return meter(value, 3);
            default:
                return tDigit(value, 1);
        }
    }

    public static string tDigit(double value, int decimal_point = 0, bool removeZero = false, string argument_toString = null, bool toShowSign = false, bool toRoundDown = false)
    {
        if (value == 0) return "0";
        if (value < 0.000001 && value > - 0.000001)//0.0001%以下
        {
            return value.ToString("0.00e0"); 
        }
        if (value < 10000 && value > -10000)//10K未満は通常表記にする
        {
            switch (decimal_point)
            {
                case 1: return value.ToString("F1"); 
                case 2: return value.ToString("F2");
                case 3: return value.ToString("F3");
                case 4: return value.ToString("F4");
                default: return Floor(value).ToString("F0");
            }
        }
        if (value >= 1e15) return value.ToString("0.00e0");
        //ToStringとして使う場合そのまま返す
        if (argument_toString != null) { return value.ToString(argument_toString); }
        if (double.IsInfinity(value))
        {
            return value.ToString();
        }
        //マイナスの処理
        isMinus_tdigit = (value < 0);
        if (isMinus_tdigit) { value = -value; }
        //共通の処理
        log_1000_tdigit = value >= 1000 ? (int)Math.Log(value, 1000) : 0;
        head_value_tdigit = value / Math.Pow(1000, log_1000_tdigit);
        digit_builder.Clear();

        argument_ToString_tdigit = "";
        if (value >= 1000)
        {
            if (head_value_tdigit >= 100) { argument_ToString_tdigit = "F1"; }
            else if (head_value_tdigit >= 10) { argument_ToString_tdigit = "F2"; }
            else if (head_value_tdigit >= 1) { argument_ToString_tdigit = "F3"; }
        }
        else
        {
            if (toRoundDown) head_value_tdigit = RoundDown(head_value_tdigit, decimal_point); //切り捨ての処理
            //IEH2では、10000以上の表示を３桁にする
            //switch (decimal_point)
            //{
            //    case 1: argument_ToString_tdigit = "F1"; break;
            //    case 2: argument_ToString_tdigit = "F2"; break;
            //    case 3: argument_ToString_tdigit = "F3"; break;
            //    default: argument_ToString_tdigit = "F0"; break;
            //}
            argument_ToString_tdigit = "F3";
        }
        if (removeZero) digit_builder.Append(RemoveZero(head_value_tdigit.ToString(argument_ToString_tdigit)));
        else digit_builder.Append(head_value_tdigit.ToString(argument_ToString_tdigit));
        digit_builder.Append(digit[log_1000_tdigit]);
        //マイナスの処理
        if (isMinus_tdigit) digit_builder.Insert(0, "-");
        else if (toShowSign) digit_builder.Insert(0, "+");
        return digit_builder.ToString();
    }

    public static string percent(double d, int num = 2)
    {
        if (d == 0)
            return 0 + "%";
        return tDigit(d * 100, num) + "%";
    }
    public static string meter(double d, int num = 3, bool isShort = false)
    {
        if(isShort) return tDigit(d / 100d, num) + "m";
        return tDigit(d / 100d, num) + " meters";
    }

    //不要な0を取り消す関数
    static System.Text.RegularExpressions.Regex pattern1 = new System.Text.RegularExpressions.Regex(@"(\.\d+?)0+\b");
    static System.Text.RegularExpressions.Regex pattern2 = new System.Text.RegularExpressions.Regex(@"\.0*$");
    public static string RemoveZero(string number)
    {
        //末尾の0を消す関数
        string strcut = pattern1.Replace(number, "$1");                                                //0を消す
        strcut = pattern2.Replace(strcut, "");                                                         //.0,,,で終わっていたらそれを消す
        //string strcut = System.Text.RegularExpressions.Regex.Replace(number, @"(\.\d+?)0+\b", "$1"); //0を消す
        //strcut = System.Text.RegularExpressions.Regex.Replace(strcut, @"\.0*$", "");                 //.0,,,で終わっていたらそれを消す
        return strcut;
    }

    static int factor_roundDown;
    public static double RoundDown(double number, int digit)
    {
        switch (digit)
        {
            case 0: factor_roundDown = 1; break;
            case 1: factor_roundDown = 10; break;
            case 2: factor_roundDown = 100; break;
            case 3: factor_roundDown = 1000; break;
            case 4: factor_roundDown = 10000; break;
            case 5: factor_roundDown = 100000; break;
            default: factor_roundDown = (int)Math.Pow(10, digit); break;
        }
        return FastFloor(number * factor_roundDown) / factor_roundDown;
    }

    public static double FastFloor(double num)
    {
        if ((num >= long.MinValue) && (num <= long.MaxValue))
        {
            return (double)(long)num;
        }
        else
        {
            return Math.Floor(num);
        }
    }
    public static void writeGyoretsu(int[,] ary)
    {
        string str = "";
        int FEcount = 0;
        foreach (int expInt in ary)
        {
            str = str + "," + expInt.ToString();
            FEcount++;
            if (FEcount % ary.GetLength(1) == 0)
            {
                str = str + "\n";
            }

        }
        Debug.Log(str);
    }
    public static void writeList(List<int> list)
    {
        string str = "";
        foreach (int num in list)
        {
            str = str + "," + num.ToString();
        }
        Debug.Log(str);
    }

    public static void writeList(List<double> list)
    {
        string str = "";
        foreach (double num in list)
        {
            str = str + "," + num.ToString("F1");
        }
        Debug.Log(str);
    }

    public static void writeArray(int[] Array)
    {
        string str = "";
        foreach (int num in Array)
        {
            str = str + "," + num.ToString("F1");
        }
        Debug.Log(str);
    }
    public static Vector2 normalize(Vector2 vector)
    {
        if (vector == Vector2.zero) return Vector2.zero;
        float x = vector.x;
        float y = vector.y;
        float normalizeFactor = 1.0f / Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        return vector * normalizeFactor;
    }

    public static float vectorAbs(Vector2 vector)
    {
        float x = vector.x;
        float y = vector.y;
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
    }

    public static IEnumerator NewInvokeCor(usefuleDelegate Dele, float Time)
    {
        yield return new WaitForSeconds(Time);
        Dele();
    }

    //減算の処理
    public static bool IfCanSubSub(ref double Target, double Value)
    {
        if (Target >= Value)
        {
            Target -= Value;
            return true;
        }
        else
        {
            Debug.Log("値が大きすぎます");
            return false;
        }
    }



    //windowを出すコルーチン
    public static IEnumerator bigAndBig(GameObject targetUI)
    {
        targetUI.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        targetUI.SetActive(true);
        for (float i = 0.5f; i <= 1.0f; i += 0.1f)
        {
            targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.01f);
        }
        targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    //windowを消すコルーチン
    public static IEnumerator smallAndSmall(GameObject targetUI)
    {
        targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
        for (float i = 1.0f; i >= 0.5f; i -= 0.1f)
        {
            targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.01f);
        }
        targetUI.SetActive(false);
        targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    //windowを非アクティブにせず、消すコルーチン
    public static IEnumerator smallAndSmallActive(GameObject targetUI)
    {
        targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
        for (float i = 1.0f; i >= 0.5f; i -= 0.1f)
        {
            targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.01f);
        }
        //targetUI.SetActive(false);
        targetUI.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    //
    public static Main GetMain()
    {
        GameObject mainCtrl = GameObject.FindGameObjectWithTag("mainCtrl");
        return mainCtrl.GetComponent<Main>();
    }

    //
    public static IEnumerator EasyForCor(usefuleDelegate dele, int num, float interval)
    {
        for (int i = 0; i < num; i++)
        {
            dele();
            yield return new WaitForSeconds(interval);
        }
    }

    //the difference of Datetime to float
    public static float DeltaTimeFloat(DateTime DT)
    {
        if(DT == null)
        {
            return 0;
        }

        float ans = ((float)DateTime.Now.Subtract(DT).Seconds
            + DateTime.Now.Subtract(DT).Minutes * 60
            + DateTime.Now.Subtract(DT).Hours * 3600
            + DateTime.Now.Subtract(DT).Days * 86400);

        if(ans >= 0)
        {
            return ans;
        }
        else
        {
            return 0;
        }
    }

    public static float DeltaTimeSec(DateTime dateTimeBase, DateTime dateTime)
    {
        if (dateTimeBase == null || dateTimeBase == null) return 0;

        float deltaTimesec = dateTime.Subtract(dateTimeBase).Seconds + dateTime.Subtract(dateTimeBase).Minutes * 60f + dateTime.Subtract(dateTimeBase).Hours * 3600f + dateTime.Subtract(dateTimeBase).Days * 86400f;
        //float baseDateTimeSec = (dateTimeBase.Second + dateTimeBase.Minute * 60f + dateTimeBase.Hour * 3600f + dateTimeBase.Day * 86400f);
        //float dateTimeSec = (dateTime.Second + dateTime.Minute * 60f + dateTime.Hour * 3600f + dateTime.Day * 86400f);
        if (deltaTimesec > 0) return deltaTimesec;
        return 0;
    }

    public static string DoubleTimeToDate(double Time, bool isPreventSec = false, bool CoronMode = false)
    {
        if (Time < 1) return "0s";
        int Day = 0;
        int Hour = 0;
        int Minute = 0;
        int Second = 0;
        string DayString = "";
        string HourString = "";
        string MinuteString = "";
        string SecondString = "";

        Day = (int)Math.Floor(Time / 86400);
        Hour = (int)Math.Floor((Time % 86400) / 3600);
        Minute = (int)Math.Floor((Time % 3600) / 60);
        Second = (int)Math.Floor((Time % 60));

        if (CoronMode)
        {
            Hour = (int)Math.Floor(Time / 3600);
            Minute = (int)Math.Floor((Time % 3600) / 60);
            Second = (int)Math.Floor((Time % 60));
            if (Hour > 0) { HourString = Hour.ToString("D2") + ":"; }
            MinuteString = Minute.ToString("D2") + ":";
            SecondString = Second.ToString("D2");
        }
        else
        {
            Day = (int)Math.Floor(Time / 86400);
            Hour = (int)Math.Floor((Time % 86400) / 3600);
            Minute = (int)Math.Floor((Time % 3600) / 60);
            Second = (int)Math.Floor((Time % 60));
            if (Day > 0) { DayString = Day.ToString() + "d"; }
            if (Day > 0 || Hour > 0) { HourString = Hour.ToString() + "h"; }
            if (Day > 0 || Hour > 0 || Minute > 0) { MinuteString = Minute.ToString() + "m"; }
            if (!isPreventSec && (Day > 0 || Hour > 0 || Minute > 0 || Second >= 0)) { SecondString = Second.ToString() + "s"; }
        }

        return DayString + HourString + MinuteString + SecondString;
    }

    public static float Pdf_Ed(double Ramda, double X)
    {
        return 1.0f - (float)Math.Pow(Math.E, (-1 * Ramda * X));
    }

    public static double ZC(double number)//ZC stands for "Zero Check";
    {
        return Math.Max(0.000000001, number);
    }

    public static double Domain(double Value, double Max, double Min)
    {
        double tempDouble = 0;
        //tempDouble = Math.Max(Value, Min);
        //tempDouble = Math.Min(Value, Max);
        tempDouble = Value <= Min ? Min : Value;
        tempDouble = Value >= Max ? Max : Value;
        return tempDouble;
    }

    //public static double LogNormalDistribution(double Mode,double Mean)
    //{
    //    return (LogNormal.Sample(1d / 3d * Math.Log(Mean * Mean * Mode), Math.Sqrt(2d / 3d * Math.Log(Mean / Mode))));
    //}


    /// <summary>
    /// Componentを全探索するので重い。
    /// 指定されたインターフェイスを実装したコンポーネントのリストを返す
    /// </summary>
    public static List<Component> FindObjectsOfInterface<T>() where T : class
    {
        var list = new List<Component>();
        foreach (var n in FindObjectsOfType<Component>())
        {
            var Interface = n as T;
            if (Interface != null)
            {
                list.Add(n);
            }
        }
        return list;
    }


    public static bool isRange(float range, GameObject targetObject, GameObject homeObject)
    {
        if (homeObject.GetComponent<RectTransform>().anchoredPosition.x > targetObject.GetComponent<RectTransform>().anchoredPosition.x - range
                    && homeObject.GetComponent<RectTransform>().anchoredPosition.x < targetObject.GetComponent<RectTransform>().anchoredPosition.x + range
                     && homeObject.GetComponent<RectTransform>().anchoredPosition.y > targetObject.GetComponent<RectTransform>().anchoredPosition.y - range
                    && homeObject.GetComponent<RectTransform>().anchoredPosition.y < targetObject.GetComponent<RectTransform>().anchoredPosition.y + range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool isRange(float range, Vector2 targetObject, GameObject homeObject)
    {
        if (homeObject.GetComponent<RectTransform>().anchoredPosition.x > targetObject.x - range
                    && homeObject.GetComponent<RectTransform>().anchoredPosition.x < targetObject.x + range
                     && homeObject.GetComponent<RectTransform>().anchoredPosition.y > targetObject.y - range
                    && homeObject.GetComponent<RectTransform>().anchoredPosition.y < targetObject.y + range)
        {
            return true;
        }
        else
        {
            return false;

        }
    }



    public static bool isRange(float range, Vector2 targetObject, Vector2 homeObject)
    {
        if (homeObject.x > targetObject.x - range
                    && homeObject.x < targetObject.x + range
                     && homeObject.y > targetObject.y - range
                    && homeObject.y < targetObject.y + range)
        {
            return true;
        }
        else
        {
            return false;

        }
    }


    /// <summary>
    /// delegateに代入して使うと、引数なしで第一引数の値がそのまま出力され、引数ありで第二引数が第一引数に代入される。
    /// </summary>
    public static Type Sync<Type>(ref Type Value, Type? Sub = null)
        where Type : struct
    {
        if (Sub == null)
        {
            return Value;
        }
        else
        {
            Value = (Type)Sub;
            return Value;
        }
    }

    /// <summary>
    /// 配列のサイズが第二引数のものと違ったら合わせる関数
    /// </summary>
    public static void InitializeArray<Type>(ref Type[] Obj, int Length)
    {
        if (Obj == null) { Obj = new Type[0]; }
        if (Obj.Length != Length)
        {
            Array.Resize(ref Obj, Length);
        }
    }

    public static bool WithinRandom(double chancePercent)
    {
        return UnityEngine.Random.Range(0, Parameter.randomAccuracy) < chancePercent * Parameter.randomAccuracy;
    }

}