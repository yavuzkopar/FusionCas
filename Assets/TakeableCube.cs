using Fusion;
using System.Collections;
using UnityEngine;

public class TakeableCube : NetworkBehaviour, IInteractable, IDroppable
{
    [Networked(OnChanged = nameof(ChangeByte))]
    public byte status { get; set; }
    public Transform parentTransform { get; set; }
    Vector3 pos;

    float sinceLastHit;
    Collider _collider;
    Rigidbody _rigidBody;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Interact(Transform t)
    {
        status = 1;
        parentTransform = t;
    }
    public override void FixedUpdateNetwork()
    {
        if(!Object.HasInputAuthority) return;
        if(transform.parent != null)
        {
            transform.localPosition = Vector3.zero;
        }
    }
    public static void ChangeByte(Changed<TakeableCube> changed)
    {
        byte newValue = changed.Behaviour.status;
        changed.LoadOld();
        byte oldValue = changed.Behaviour.status;
        if (newValue == 1 && oldValue == 0)
        {
            changed.Behaviour.TakeRemote();
        }
        if (newValue == 0 && oldValue == 1)
        {
            changed.Behaviour.ResetParent();
        }
    }

    void TakeRemote()
    {
        _collider.isTrigger = true;
        _rigidBody.isKinematic = true;
        transform.parent = parentTransform;
        transform.localPosition = Vector3.zero;

    }
    void ResetParent()
    {
        _collider.isTrigger = false;
        _rigidBody.isKinematic = false;
        transform.parent = null;

    }
    public void Drop()
    {
        status = 0;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
public interface IInteractable
{
    Transform GetTransform();
    void Interact(Transform t);
}
public interface IDroppable
{
    void Drop();
}
