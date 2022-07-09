// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Photon.Chat.Demo
{
    public class ChannelSelector : MonoBehaviour, IPointerClickHandler
    {
        public string Channel;
        public ChatGui chatGui;
        Text t;
        public void SetChannel(ChatGui chatGui, string channel)
        {
            this.chatGui = chatGui;
            this.Channel = channel;
            t = this.GetComponentInChildren<Text>();
            SetChannelName();
            //t.text = this.Channel;
        }
        public void SetChannelName()
        {
            t.text = this.Channel + " (" + chatGui.chatClient.PublicChannels[Channel].Subscribers.Count + " / " + chatGui.chatClient.PublicChannels[Channel].MaxSubscribers + ")";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChatGui handler = FindObjectOfType<ChatGui>();
            handler.ShowChannel(this.Channel);
        }
    }
}