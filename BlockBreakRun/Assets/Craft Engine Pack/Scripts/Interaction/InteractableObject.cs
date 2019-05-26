using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    public ItemDescription m_itemDescription;
    public bool m_HaveInteractionMessage = false;
    public virtual void Interact() { }
    public virtual string GetMessage(InputUnit m_interactionKey) { return "'" + m_interactionKey.Key.ToString() + "'"; }
    protected void DisableGameObjectDisturbComponents(GameObject go)
    {
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        InteractableObject io = go.GetComponent<InteractableObject>();
        if (io)
        {
            Destroy(io);
        }

        foreach (Collider c in go.GetComponentsInChildren(typeof(Collider), true))
            Destroy(c);

        Savable s = go.GetComponent<Savable>();
        if (s)
            s.m_isFromFileOrInventory = true;
    }
}
