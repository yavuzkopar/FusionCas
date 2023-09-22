using Fusion;
using UnityEngine;

public class InteractionHandler : NetworkBehaviour
{
    public Transform aimOrigin;
    [SerializeField] LayerMask hittableMask;
    public Transform hand;

    private void OnEnable()
    {
        GetComponentInChildren<AnimEvents>().Take += InteractionHandler_Take; ;
    }
    bool interacted;
    private void InteractionHandler_Take()
    {
       
        interacted = true;
        GetComponentInChildren<Animator>().ResetTrigger("take");
    }

    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData data))
        {
            if(interacted)
            {
                Vector3 p1 = aimOrigin.position;
                Vector3 p2 = aimOrigin.position + aimOrigin.forward;
                if (Physics.CapsuleCast(p1, p2, 1f, aimOrigin.forward, out RaycastHit hit, 2f, hittableMask))
                {
                    if (hit.collider != null)
                    {
                        hit.collider.GetComponent<IInteractable>().Interact(hand);
                    }
                }
            }
            //if(data.isInteracted)
            //{
            //    //Vector3 p1 = aimOrigin.position;
            //    //Vector3 p2 = aimOrigin.position+ aimOrigin.forward;
            //    //if (Physics.CapsuleCast(p1, p2, 1f, aimOrigin.forward, out RaycastHit hit, 2f, hittableMask))
            //    //{
            //    //    if (hit.collider != null)
            //    //    {
            //    //        hit.collider.GetComponent<IInteractable>().Interact(hand);
                        

            //    //    }
            //    //}
            //}
            if(data.isDropped)
            {
                if(hand.childCount>0)
                {
                    if(hand.GetChild(0).TryGetComponent(out IDroppable droppable))
                    {
                        droppable.Drop();
                    }
                    
                }
            }
            
        }
        
        interacted= false;
    }
}
