using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections.Generic;
using UnityEngine;
using static Spine.Skin;

public class SkinController<T> where T : ISkeletonAnimation, IAnimationStateComponent
{
    public T skeletonAnimation;

    protected List<string> currentActiveSkinName;
    protected Skin currentActiveSkin;

    protected List<AttachmentData> activeAttachments;

    public SkinController(T skeleton)
    {
        skeletonAnimation = skeleton;

        currentActiveSkin = new Skin("");
        var newSkin = skeletonAnimation.Skeleton.Skin;
        if (newSkin == null)
        {
            newSkin = skeletonAnimation.Skeleton.Data.DefaultSkin;
        }

        currentActiveSkinName = new List<string>();
        currentActiveSkinName.Add(newSkin.Name);
        currentActiveSkin.AddSkin(currentActiveSkin);
        activeAttachments = new List<AttachmentData>();
    }

    public void AddActiveSkin(string skin)
    {
        if (currentActiveSkinName.Contains(skin))
        {
            return;
        }
        Debug.Log($"Loading {skin} skin");
        Skin newSkin = skeletonAnimation.Skeleton.Data.FindSkin(skin);
        if (newSkin != null)
        {
            currentActiveSkin = new Skin(skin);
            currentActiveSkin.AddSkin(skeletonAnimation.Skeleton.Skin);
            currentActiveSkin.AddSkin(newSkin);
            currentActiveSkinName.Add(skin);

            if (activeAttachments.Count > 0)
            {
                LoadAttachments();
            }
            else
            {
                skeletonAnimation.Skeleton.SetSkin(currentActiveSkin);
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
            }
        }
    }

    public void SwapActiveSkin(string skin)
    {
        if (currentActiveSkinName.Contains(skin))
        {
            return;
        }
        Debug.Log($"Loading {skin} skin");
        Skin newSkin = skeletonAnimation.Skeleton.Data.FindSkin(skin);
        if (newSkin != null)
        {
            currentActiveSkin = new Skin(skin);
            currentActiveSkin.AddSkin(newSkin);
            currentActiveSkinName.Clear();
            currentActiveSkinName.Add(skin);

            if (activeAttachments.Count > 0)
            {
                LoadAttachments();
            }
            else
            {
                skeletonAnimation.Skeleton.SetSkin(currentActiveSkin);
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
            }
        }
    }

    protected void LoadAttachments()
    {
        Skin newSkin = new Skin("newSkin");
        foreach (string skinName in currentActiveSkinName)
        {
            newSkin.AddSkin(skeletonAnimation.AnimationState.Data.SkeletonData.FindSkin(skinName));
        }

        foreach (var attachmentData in activeAttachments)
        {
            Skin loadSkin = new Skin("loadSkin");
            loadSkin.AddSkin(skeletonAnimation.AnimationState.Data.SkeletonData.FindSkin(attachmentData.skin));

            if (attachmentData.slotName != "" && attachmentData.attachmentName != "")
            {
                SkinEntry goodAttachment = new SkinEntry();
                SkinEntry firstAttachment = new SkinEntry();
                bool addedFirst = false;

                foreach (var attachment in loadSkin.Attachments)
                {
                    if (!addedFirst)
                    {
                        addedFirst = true;
                        firstAttachment = attachment;
                    }

                    if (attachment.Name == attachmentData.attachmentName)
                    {
                        goodAttachment = attachment;
                    }
                }
                loadSkin.RemoveAttachment(firstAttachment.SlotIndex, firstAttachment.Name);
                loadSkin.SetAttachment(firstAttachment.SlotIndex, firstAttachment.Name, goodAttachment.Attachment);
            }
            newSkin.AddSkin(loadSkin);
        }

        currentActiveSkin = newSkin;

        skeletonAnimation.Skeleton.SetSkin(newSkin);
        skeletonAnimation.Skeleton.SetToSetupPose();
        skeletonAnimation.Skeleton.UpdateCache();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
    }

    public void ActivateSlotSkin(AttachmentData newAttachment)
    {
        if (activeAttachments.Contains(newAttachment))
        {
            Debug.Log($"attachment already active!");
            return;
        }

        activeAttachments.Add(newAttachment);
        LoadAttachments();
    }

    public void DeactivateSlotSkin(AttachmentData newAttachment)
    {
        if (!activeAttachments.Contains(newAttachment))
        {
            return;
        }

        activeAttachments.Remove(newAttachment);
        LoadAttachments();
    }

    public void DeactivateAllSlotSkins()
    {
        activeAttachments.Clear();
        LoadAttachments();
    }
}

public struct AttachmentData
{
    public string skin, slotName, attachmentName;

    public AttachmentData(string skin, string slotName, string attachmentName)
    {
        this.skin = skin;
        this.slotName = slotName;
        this.attachmentName = attachmentName;
    }

    public override string ToString()
    {
        return $"{skin}_{attachmentName}";
    }
}