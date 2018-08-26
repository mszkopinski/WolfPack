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
            Debug.Log("Clicked");
            rb.AddForce(transform.forward * 200f, ForceMode.Impulse);
        }

        void OnMouseEnter()
        {
            Debug.Log("Clicked");
        }
    }
}