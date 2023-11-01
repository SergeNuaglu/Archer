using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class ArcherAnimator : MonoBehaviour
{
    [SerializeField] private SightRotator _sightRotator;
    [SerializeField] private List<SkeletonUtilityBone> _overidingBonesUtilitys;

    [SpineBone(dataField: "skeletonAnimation")]
    [SerializeField] private string boneName;

    [SpineAnimation]
    [SerializeField] private string _idleAnimationName;

    [SpineAnimation]
    [SerializeField] private string _attackStartAnimationName;

    [SpineAnimation]
    [SerializeField] private string _attackFinishAnimationName;

    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;

    private void OnEnable()
    {
        _sightRotator.RotateStarted += OnSightRotateStarted;
        _sightRotator.RotateEnded += OnSightRotateEnded;
    }

    private void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        _spineAnimationState = _skeletonAnimation.AnimationState;
    }

    private void OnDisable()
    {
        _sightRotator.RotateStarted -= OnSightRotateStarted;
        _sightRotator.RotateEnded -= OnSightRotateEnded;
    }

    private void OnSightRotateStarted()
    {
        StartCoroutine(Animate(_attackStartAnimationName, StartAiming));
    }

    private void OnSightRotateEnded()
    {
        StartCoroutine(Animate(_attackFinishAnimationName, FinishAiming));
    }

    private void SwitchOverridingBonesMode(SkeletonUtilityBone.Mode mode)
    {
        foreach (SkeletonUtilityBone utilityBone in _overidingBonesUtilitys)
        {
            utilityBone.mode = mode;
        }
    }

    private void FinishAiming()
    {
        SwitchOverridingBonesMode(SkeletonUtilityBone.Mode.Follow);
        _spineAnimationState.AddAnimation(0, _idleAnimationName, true, 0);
    }

    private void StartAiming()
    {
        SwitchOverridingBonesMode(SkeletonUtilityBone.Mode.Override);
    }

    IEnumerator Animate(string animationName, Action callBack)
    {
        TrackEntry trackIndex = _skeletonAnimation.state.SetAnimation(0, animationName, false);
        yield return new WaitForSpineAnimationComplete(trackIndex);
        callBack?.Invoke();
    }
}

