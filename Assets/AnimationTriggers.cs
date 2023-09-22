using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : NetworkBehaviour
{
    [SerializeField] Animator animator;
    [Networked(OnChanged = nameof(OnJumped))]
    bool isJumped { get; set; }

   
    static void OnJumped(Changed<AnimationTriggers> changed)
    {
        bool newvalue = changed.Behaviour.isJumped;
        changed.LoadOld();
        bool oldValue = changed.Behaviour.isJumped;
        if (newvalue && !oldValue)
        {
            changed.Behaviour.Remote();
        }
    }
    void Remote()
    {
        if (!Object.HasInputAuthority)
        {
            animator.SetTrigger("jump");
        }
    }
    IEnumerator JumpCO()
    {
        isJumped = true;
        yield return new WaitForSeconds(0.1f);
        isJumped = false;

    }
    [Networked(OnChanged = nameof(OnInteract))]
    bool isInteracted { get; set; }
    static void OnInteract(Changed<AnimationTriggers> changed)
    {
        bool newvalue = changed.Behaviour.isInteracted;
        changed.LoadOld();
        bool oldValue = changed.Behaviour.isInteracted;
        if (newvalue && !oldValue)
        {
            changed.Behaviour.RemoteInteract();
        }
    }
    void RemoteInteract()
    {
        if (!Object.HasInputAuthority)
        {
            animator.SetTrigger("take");
        }
    }
    IEnumerator TakeCO()
    {
        isInteracted = true;
        yield return new WaitForSeconds(0.1f);
        isInteracted = false;

    }
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            if(data.isJumpPressed)
            {
                 StartCoroutine(JumpCO());
               // jumpTrigger.StartAnim();
            }
            if(data.isInteracted)
            {
                  StartCoroutine(TakeCO());
            //    takeTrigger.StartAnim();
            }
        }
    }
}
public class Trigger :NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnValueChange))]
    public bool triggerBool { get; set; }

    public Animator animator;
    public string triggerName;

    public Trigger(Animator animator,string triggerName)
    {
        this.animator = animator;
        this.triggerName = triggerName;
    }

    static void OnValueChange(Changed<Trigger> changed)
    {
        bool newValue = changed.Behaviour.triggerBool;
        changed.LoadOld();
        bool oldValue = changed.Behaviour.triggerBool;
        if(newValue && !oldValue)
        {
            changed.Behaviour.RemoteTrigger();
        }
    }
    void RemoteTrigger()
    {
        if(!Object.HasInputAuthority)
        {
            animator.SetTrigger(triggerName);
        }
    }
    public void StartAnim()
    {
        StartCoroutine(TriggerCO());
    }
    IEnumerator TriggerCO()
    {
        triggerBool = true;
        yield return new WaitForSeconds(0.1f);
        triggerBool = false;
    }
}
