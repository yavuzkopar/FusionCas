using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : NetworkBehaviour,IInteractable
{
    [Networked(OnChanged = nameof(OnInteracted))]
    byte interacted { get; set; }
    Vector3 cloased;
    Vector3 opened;
    Vector3 targetRotation;
    public override void Spawned()
    {
        cloased = Vector3.zero;
        opened = new Vector3(0,-90,0);
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform t)
    {
        if(interacted == 0)
        {
            interacted = 1;
        }
        else if(interacted == 1)
        {
            interacted = 0;
        }
    }
    static void OnInteracted(Changed<Door> changed)
    {
        byte newValue = changed.Behaviour.interacted;
        changed.LoadOld();
        byte oldValue = changed.Behaviour.interacted;
        if(newValue ==1 && oldValue == 0)
        {
            changed.Behaviour.Open();
        }
        else if(newValue == 0 && oldValue ==1)
        {
            changed.Behaviour.Close();
        }
    }
    public override void FixedUpdateNetwork()
    {
        Quaternion quaternion = Quaternion.Euler(targetRotation);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, quaternion, Runner.DeltaTime);
    }
    void Open()
    {
        targetRotation = opened;
    }
    void Close()
    {
        targetRotation = cloased;
    }
}
