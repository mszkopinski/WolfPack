using UnityEngine;
using Wolfpack;

public class Obstacle : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        animator.Play("Obstacle@Destroy");
        Destroy(gameObject, animator.GetClipLength("Obstacle@Destroy"));
    }
}