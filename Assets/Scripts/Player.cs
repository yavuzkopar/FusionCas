using Fusion;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour,IPlayerLeft
{
    [SerializeField]
    TextMeshProUGUI nameText;
    public static Player Local { get; private set; }

    [Networked(OnChanged = nameof(OnNicknamehanged))]
    public NetworkString<_16> playerNickname { get; set; }

    private static void OnNicknamehanged(Changed<Player> changed)
    {
        changed.Behaviour.OnNicknameChanged();
    }
    void OnNicknameChanged()
    {
        nameText.text = playerNickname.ToString();
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            Camera.main.gameObject.SetActive(false);
            if(PlayerPrefs.HasKey("PlayerName"))
            {
                RPC_SetNickname(PlayerPrefs.GetString("PlayerName"));
            }
            
        }
        else
        {
            GetComponentInChildren<Camera>().enabled= false;
        }
    }
    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    void RPC_SetNickname(string nickname,RpcInfo info = default)
    {
        playerNickname = nickname;
    }
  
}
