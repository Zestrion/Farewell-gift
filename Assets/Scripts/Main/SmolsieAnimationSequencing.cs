using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class SmolsieAnimationSequencing : MonoBehaviour
    {
        [SerializeField] SkeletonAnimation skeletonAnimation;
        [SerializeField] List<string> animationNames;

        int animationIndex;

        // Start is called before the first frame update
        void Start()
        {
            animationIndex = 0;
            TrackEntry trackEntry = skeletonAnimation.state.SetAnimation(0, animationNames[animationIndex], false);
            trackEntry.Complete += HandleEvent;
        }

        void HandleEvent(TrackEntry trackEntry)
        {
            animationIndex++;
            if (animationIndex >= animationNames.Count)
            {
                animationIndex = 0;
            }
            TrackEntry newTrackEntry = skeletonAnimation.state.SetAnimation(0, animationNames[animationIndex], false);
            newTrackEntry.Complete += HandleEvent;
        }

    }
}