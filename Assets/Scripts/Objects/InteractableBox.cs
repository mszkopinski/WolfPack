using UnityEngine;

namespace Wolfpack
{
    public class InteractableBox : MonoBehaviour
    {
        Rigidbody rb;
    
        void Awake()
        {
            rb = GetComponentInParent<Rigidbody>();
        }
    
        void OnMouseDown()
        {
            Log.Console("Clicked");
            rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
        }

        void OnMouseEnter()
        {
            Log.Console("Clicked");
        }
    }
}