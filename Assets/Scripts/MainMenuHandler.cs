using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField inputfield;
    [SerializeField] TMP_InputField sessionNameInputField;

    [Header("Panels")]
    [SerializeField] GameObject playerDeatilsPanel;
    [SerializeField] GameObject sessionListPanel;
    [SerializeField] GameObject createGamePanel;
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            inputfield.text = PlayerPrefs.GetString("PlayerName");
        }
        
    }

    public void JoinButton()
    {
        PlayerPrefs.SetString("PlayerName", inputfield.text);
        PlayerPrefs.Save();
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.OnJoin += ActiveJoinPanel;
        networkRunnerHandler.OnJoinLobby();
    }

    private void ActiveJoinPanel()
    {
        HideAllPanels();
        sessionListPanel.SetActive(true);
    }

    public void OnCreateNewGameClicked()
    {
        HideAllPanels();
        createGamePanel.SetActive(true);
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.OnJoin -= ActiveJoinPanel;
    }
    public void OnStartNewSessionClicked()
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.CreateGame(sessionNameInputField.text);
    }
    void HideAllPanels()
    {
        playerDeatilsPanel.SetActive(false);
        sessionListPanel.SetActive(false);
        createGamePanel.SetActive(false);
    }
}
