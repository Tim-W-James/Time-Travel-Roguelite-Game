using UnityEngine;
using System.Collections;

//destroys animator after delay

public class AnimatorExpire : MonoBehaviour
{
    public float delay = 0f; //delay before animation expires

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay); //destroy animator after finished + delay
    }
}