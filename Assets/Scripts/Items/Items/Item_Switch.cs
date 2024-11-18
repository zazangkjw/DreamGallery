using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Switch : Item
{
    public int mode = 1;
    public MeshRenderer led_l;
    public MeshRenderer led_r;
    public Material led_l_mat;
    public Material led_r_mat;
    public Material led_off_mat;
    public Material object_r_on_mat;
    public Material object_r_off_mat;
    public Material object_l_on_mat;
    public Material object_l_off_mat;

    public List<MeshRenderer> objects_l = new List<MeshRenderer>();
    public List<MeshRenderer> objects_r = new List<MeshRenderer>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private new void OnEnable()
    {
        base.OnEnable();

        handAnim.Play("Gift_Up");
        transform.localPosition = new Vector3(-0.06f, 0.12f, -0.05f);
        transform.localEulerAngles = new Vector3(0, -60, -10);
        StartCoroutine(SwitchCoroutine());
    }





    IEnumerator SwitchCoroutine()
    {
        while (true)
        {
            if (!putDialogScript.isClickMode && defaultRaycast.currentItem == this && !DefaultRaycast.inventoryOnOff && !DefaultSceneManager.isPausing) // 클릭형 대사 나오는 동안 기능 비활성화
            {
                // 클릭
                if (Input.GetMouseButtonDown(0))
                {
                    // 왼쪽 ON
                    if (mode == 2)
                    {
                        mode = 1;
                        led_l.material = led_l_mat;
                        led_r.material = led_off_mat;
                        foreach(MeshRenderer m in objects_l)
                        {
                            m.material = object_l_on_mat;
                        }
                        foreach (MeshRenderer m in objects_r)
                        {
                            m.material = object_r_off_mat;
                        }
                    }
                    // 오른쪽 ON
                    else
                    {
                        mode = 2;
                        led_l.material = led_off_mat;
                        led_r.material = led_r_mat;
                        foreach (MeshRenderer m in objects_l)
                        {
                            m.material = object_l_off_mat;
                        }
                        foreach (MeshRenderer m in objects_r)
                        {
                            m.material = object_r_on_mat;
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
