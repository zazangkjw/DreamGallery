using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtGalleryColliderTrigger : MonoBehaviour
{
    public PutDialogScript putDialogScript;

    public Collider[] ColliderTriggers; // Ʈ���� �ݶ��̴���

    public Collider deskCollider;
    public HeadTracking deskManHeadTracking;

    private void OnTriggerStay(Collider other)
    {
        if (other == deskCollider)
        {
            deskManHeadTracking.isLooking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == deskCollider)
        {
            deskManHeadTracking.isLooking = false;
        }
    }
}
