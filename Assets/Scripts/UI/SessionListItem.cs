using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
using System;

public class SessionListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sessionNameText;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] Button joinButton;

    SessionInfo sessionInfo;

    public event Action<SessionInfo> OnJoinSession;
    public void SetInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        sessionNameText.text = sessionInfo.Name;
        playerCountText.text = sessionInfo.PlayerCount.ToString() +" / "+sessionInfo.MaxPlayers.ToString();
        bool isJoinButtonActive = true;

        if(sessionInfo.PlayerCount>= sessionInfo.MaxPlayers)
        {
            isJoinButtonActive = false;
        }
        joinButton.gameObject.SetActive(isJoinButtonActive);
        joinButton.onClick.AddListener(OnClick);
    }
    public void OnClick()
    {
        OnJoinSession?.Invoke(sessionInfo);
    }
}
