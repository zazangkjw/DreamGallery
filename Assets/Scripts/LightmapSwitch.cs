using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightmapSwitch : MonoBehaviour
{
    // ����Ʈ�� �ؽ�ó 3������ �ν����ͷ� ����ͼ� �迭�� ����
    public Texture2D[] lightmap_dir; 
    public Texture2D[] lightmap_light;
    public Texture2D[] lightmap_shadow;
    LightmapData currentLightmap; // ���� ���� ��ų ����Ʈ�� ������. �� 3�� �ؽ�ó�� �� ���� ���� �Ӽ��� �����ؾ� ��
    LightmapData[] lightmapArray; // �� currentLightmap�� ��ҷ� �� �迭. ����Ʈ���� ���� ������ ����ũ �� �������� �� �迭�� ���� �׸�ŭ �÷��� ��

    public Light[] lights; // ����Ʈ �迭
    public Renderer[] lightObjects; // emission�� ����� ����Ʈ ������Ʈ�� ������ �迭
    public Material lightOnMat;
    public Material lightOffMat;

    public ReflectionProbe[] reflectionProbe; // �ݻ� ���κ�� ���� ������ ���� ������ �ٽ� ����ũ

    SphericalHarmonicsL2[] bakedProbesChanges; // ����Ʈ ���κ��� �����͸� ���� ����




    void Start()
    {
        bakedProbesChanges = LightmapSettings.lightProbes.bakedProbes; // ���� ����Ʈ ���κ� ���¸� ������
        currentLightmap = new LightmapData(); // ����Ʈ�� ��ü�� ����
        lightmapArray = new LightmapData[1]; // ������ ����ϴ� ����Ʈ���� �� ���̶� 1
        reflectionProbe[0].RenderProbe(); // �ݻ� ���κ� ����ũ
    }




    void Update()
    {
        Key();
    }




    void Key() 
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!lights[0].enabled)
            {
                lights[0].enabled = true;
                lightObjects[0].material = lightOnMat;

                // ����Ʈ�� ��ü
                currentLightmap.lightmapDir = lightmap_dir[0];
                currentLightmap.lightmapColor = lightmap_light[0];
                currentLightmap.shadowMask = lightmap_shadow[0];
                lightmapArray[0] = currentLightmap;
                LightmapSettings.lightmaps = lightmapArray;

                // ����Ʈ ���κ� �� ����
                bakedProbesChanges[13].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[58].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[95].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[105].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[108].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[109].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[112].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[113].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                bakedProbesChanges[150].AddAmbientLight(new Color(255f / 255f, 255f / 255f, 255f / 255f));
                LightmapSettings.lightProbes.bakedProbes = bakedProbesChanges;

                // �ݻ� ���κ� ����ũ
                reflectionProbe[0].RenderProbe();
            }
            else if (lights[0].enabled)
            {
                lights[0].enabled = false;
                lightObjects[0].material = lightOffMat;

                // ����Ʈ�� ��ü
                currentLightmap.lightmapDir = lightmap_dir[1];
                currentLightmap.lightmapColor = lightmap_light[1];
                currentLightmap.shadowMask = lightmap_shadow[1];
                lightmapArray[0] = currentLightmap;
                LightmapSettings.lightmaps = lightmapArray;

                // ����Ʈ ���κ� �� ����
                bakedProbesChanges[13].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[58].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[95].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[105].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[108].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[109].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[112].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[113].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                bakedProbesChanges[150].AddAmbientLight(new Color(-255f / 255f, -255f / 255f, -255f / 255f));
                LightmapSettings.lightProbes.bakedProbes = bakedProbesChanges;

                // �ݻ� ���κ� ����ũ
                reflectionProbe[0].RenderProbe();
            }
        }
    }




    // � ����Ʈ ���κ긦 �ٲ�� �ϴ� ���� üũ�ϱ� ���� �ڷ�ƾ. ���κ갡 �ϳ� �� �������ٰ� �ٽ� ���� ������ ���ư�. �ܼ� �α׿� ��Ÿ���� ��ȣ�� ����ؾ� ��
    IEnumerator CheckingLightProbes()
    {
        for (int i = 0; i < bakedProbesChanges.Length; i++)
        {
            bakedProbesChanges[i].AddAmbientLight(new Color(255f / 255f, 0f / 255f, 0f / 255f));
            LightmapSettings.lightProbes.bakedProbes = bakedProbesChanges;
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
            bakedProbesChanges[i].AddAmbientLight(new Color(-1f, 0f, 0f));
        }
    }
}
