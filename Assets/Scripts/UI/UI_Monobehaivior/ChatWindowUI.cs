using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Main;
using static GameController;
using static GameControllerUI;
using static UsefulMethod;
using static SpriteSourceUI;
using static Localized;
using System;
using TMPro;
using Photon.Pun;
using Photon.Chat;

public class ChatWindowUI : MonoBehaviour
{
    OpenCloseUI openCloseUI;
    RectTransform thisRect;
    [SerializeField] Button openButton, openSubButton, closeButton, leftButton, rightButton;
    [SerializeField] Button[] channelButtons;
    string[] channelNameStrings;
    SwitchTabUI switchChannelUI;
    Text[] channelTexts;

    // Start is called before the first frame update
    void Start()
    {
        thisRect = gameObject.GetComponent<RectTransform>();
        openCloseUI = new OpenCloseUI(gameObject, true);
        openCloseUI.SetOpenButton(openButton);
        openCloseUI.SetCloseButton(closeButton);
        //closeButton.onClick.AddListener(() => { PhotonNetwork.Disconnect(); });
        rightButton.onClick.AddListener(() => SetRight());
        leftButton.onClick.AddListener(() => SetLeft());
        openButton.onClick.AddListener(() => SetActive(openSubButton.gameObject, true));
        SetActive(openSubButton.gameObject, false);
        openSubButton.onClick.AddListener(()=>
        {
            if (openCloseUI.isOpen)
            {
                openCloseUI.Close();
                //gameUI.chatGUI.Disconnect();
                //switchChannelUI.currentId = 0;
                //currentChannelId = 0;
            }
            else
            {
                openCloseUI.Open();
                //if (gameUI.chatGUI.isConnectedOnce)
                //    gameUI.chatGUI.Connect();
            }
        });
        //closeButton.onClick.AddListener(() =>
        //{
        //    //gameUI.chatGUI.Disconnect();
        //    //switchChannelUI.currentId = 0;
        //    //currentChannelId = 0;
        //    //channelButtons[0].onClick.Invoke();
        //});


        switchChannelUI = new SwitchTabUI(channelButtons, true);
        switchChannelUI.isNotTextMeshPro = true;
        switchChannelUI.openAction = ChangeChannel;
        channelTexts = new Text[channelButtons.Length];
        channelNameStrings = new string[channelButtons.Length];
        for (int i = 0; i < channelNameStrings.Length; i++)
        {
            int count = i;
            channelTexts[i] = channelButtons[count].gameObject.GetComponentInChildren<Text>();
            channelNameStrings[i] = channelTexts[count].text;
        }
        channelButtons[0].onClick.Invoke();
    }
    int currentChannelId;
    void ChangeChannel()
    {
        if (!gameUI.chatGUI.isConnected) return;
        gameUI.chatGUI.ChangeChannel(channelNameStrings[currentChannelId], channelNameStrings[switchChannelUI.currentId]);
        currentChannelId = switchChannelUI.currentId;
        //var channelCreationOptions = new ChannelCreationOptions();
        //channelCreationOptions.PublishSubscribers = true;
        //channelCreationOptions.MaxSubscribers = 100;
        //gameUI.chatGUI.chatClient.Subscribe(channelNameStrings[switchChannelUI.currentId], 0, -1, );
        //UpdateUserNum();
    }

    void SetRight()
    {
        thisRect.anchoredPosition = new Vector2(-30f, -50f);
        thisRect.sizeDelta = new Vector2(800f, 800f);
    }
    void SetLeft()
    {
        thisRect.anchoredPosition = new Vector2(-880f, -150f);
        thisRect.sizeDelta = new Vector2(860f, 900f);
    }

    private void FixedUpdate()
    {
        if (!openCloseUI.isOpen) return;
        if (!gameUI.chatGUI.isConnected) return;
        count++;
        if (count >= 1 * 60)//1秒
        {
            UpdateUserNum();
            count = 0;
        }
    }
    int count;

    public void UpdateUserNum()
    {
        ChatChannel channel = gameUI.chatGUI.chatClient.PublicChannels[channelNameStrings[currentChannelId]];
        if (channel == null) return;
        for (int i = 0; i < channelTexts.Length; i++)
        {
            int count = i;
            channelTexts[i].text = channelNameStrings[i];
            if(count == currentChannelId)
                channelTexts[i].text += " (" + channel.Subscribers.Count + " / " + channel.MaxSubscribers + ")";
            //if (gameUI.chatGUI.chatClient.PublicChannels[channelNameStrings[count]] != null)
            //{
            //    channel = gameUI.chatGUI.chatClient.PublicChannels[channelNameStrings[count]];
            //    channelTexts[i].text += " (" + channel.Subscribers.Count + " / " + channel.MaxSubscribers + ")";
            //}
        }
    }
}
