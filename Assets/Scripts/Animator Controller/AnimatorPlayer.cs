using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayer : MonoBehaviour
{
    [Serializable]
    struct FloatParameter {
        public string id;
        public float positiveValue;
        public float negativeValue;
    }

    [Header("Parameters")]
    [SerializeField] FloatParameter floatParameter;
    public Animator animator;

    private void Start() {
        Init();
    }

    public void Init() {
        //animator = gameObject.GetComponent<Animator>();
    }

    public void SetSpeed(bool isWalk) {
        if (ReferenceEquals(animator.runtimeAnimatorController, null)) return;

        if (isWalk)
            animator.SetFloat(floatParameter.id, floatParameter.positiveValue);
        else
            animator.SetFloat(floatParameter.id, floatParameter.negativeValue);
    }
}
