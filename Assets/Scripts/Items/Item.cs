using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int team;
    
    public Animator handAnim;
    public Collider[] cols;

    protected WaitForSeconds delay = new WaitForSeconds(0.2f);
    protected float chargeTimer; // 차징 시간

    public PutDialogScript putDialogScript; // 클릭형 대사 나오는 동안 기능 비활성화
}
