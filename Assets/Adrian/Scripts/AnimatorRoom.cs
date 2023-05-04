using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRoom : MonoBehaviour
{
    public bool playAnim;

    public Animator mAnimator;

    // Start is called before the first frame update
    void Start()
    {
        mAnimator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
        {
            if (playAnim)
            {
                playAnim = false;
                mAnimator.SetTrigger("Start");
            }
        }
    }
}
