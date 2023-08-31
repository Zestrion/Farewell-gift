using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SmolsieAnimationRandomizer : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;
        [SerializeField] List<string> animationNames;

        // Start is called before the first frame update
        void Start()
        {
            TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, animationNames[Random.Range(0, animationNames.Count)], false);
            trackEntry.Complete += HandleEvent;
        }

        void HandleEvent(TrackEntry trackEntry)
        {
            TrackEntry newTrackEntry = skeletonAnimation.state.SetAnimation(0, animationNames[Random.Range(0, animationNames.Count)], false);
            newTrackEntry.Complete += HandleEvent;
        }

    }
}