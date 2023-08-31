using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmolsieAttachmentAdd : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skeletonAnimation;
    AttachmentData attachmentData;

    // Start is called before the first frame update
    void Start()
    {
        attachmentData = new AttachmentData(skin: "object", slotName: "storytime objects", attachmentName: "book open");

        var skinController = new SkinController<SkeletonAnimation>(skeletonAnimation);
        skinController.ActivateSlotSkin(attachmentData);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
