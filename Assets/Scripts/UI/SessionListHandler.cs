using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionListHandler : MonoBehaviour
{
    [SerializeField] SessionListItem listItem;
    [SerializeField] VerticalLayoutGroup layoutGroup;

    private void Awake()
    {
        ClearList();    
    }
    public void ClearList()
    {
        foreach (Transform item in layoutGroup.transform)
        {
            Destroy(item.gameObject);
        }
    }
    public void AddToList(SessionInfo sessionInfo) 
    {
        SessionListItem item = Instantiate(listItem.gameObject,layoutGroup.transform).GetComponent<SessionListItem>();
        item.SetInformation(sessionInfo);
        item.OnJoinSession += Item_OnJoinSession;
    }

    private void Item_OnJoinSession(SessionInfo obj)
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.JoinGame(obj);
    }
}
