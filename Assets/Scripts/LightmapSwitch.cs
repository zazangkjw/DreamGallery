using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LightmapSwitch : MonoBehaviour
{
    // 라이트맵 텍스처 3가지를 인스펙터로 끌어와서 배열로 저장
    public Texture2D[] lightmap_dir; 
    public Texture2D[] lightmap_light;
    public Texture2D[] lightmap_shadow;
    LightmapData currentLightmap; // 씬에 적용 시킬 라이트맵 데이터. 위 3개 텍스처를 이 변수 안의 속성에 적용해야 함
    LightmapData[] lightmapArray; // 위 currentLightmap이 요소로 들어갈 배열. 라이트맵이 여러 장으로 베이크 된 씬에서는 이 배열의 행을 그만큼 늘려야 함

    public Light[] lights; // 라이트 배열
    public Renderer[] lightObjects; // emission이 적용된 라이트 오브젝트의 렌더러 배열
    public Material lightOnMat;
    public Material lightOffMat;

    public ReflectionProbe[] reflectionProbe; // 반사 프로브는 불이 꺼지고 켜질 때마다 다시 베이크

    SphericalHarmonicsL2[] bakedProbesChanges; // 라이트 프로브의 데이터를 담을 변수




    void Start()
    {
        bakedProbesChanges = LightmapSettings.lightProbes.bakedProbes; // 현재 라이트 프로브 상태를 가져옴
        currentLightmap = new LightmapData(); // 라이트맵 객체를 생성
        lightmapArray = new LightmapData[1]; // 씬에서 사용하는 라이트맵이 한 장이라 1
        reflectionProbe[0].RenderProbe(); // 반사 프로브 베이크
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

                // 라이트맵 교체
                currentLightmap.lightmapDir = lightmap_dir[0];
                currentLightmap.lightmapColor = lightmap_light[0];
                currentLightmap.shadowMask = lightmap_shadow[0];
                lightmapArray[0] = currentLightmap;
                LightmapSettings.lightmaps = lightmapArray;

                // 라이트 프로브 색 변경
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

                // 반사 프로브 베이크
                reflectionProbe[0].RenderProbe();
            }
            else if (lights[0].enabled)
            {
                lights[0].enabled = false;
                lightObjects[0].material = lightOffMat;

                // 라이트맵 교체
                currentLightmap.lightmapDir = lightmap_dir[1];
                currentLightmap.lightmapColor = lightmap_light[1];
                currentLightmap.shadowMask = lightmap_shadow[1];
                lightmapArray[0] = currentLightmap;
                LightmapSettings.lightmaps = lightmapArray;

                // 라이트 프로브 색 변경
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

                // 반사 프로브 베이크
                reflectionProbe[0].RenderProbe();
            }
        }
    }




    // 어떤 라이트 프로브를 바꿔야 하는 지를 체크하기 위한 코루틴. 프로브가 하나 씩 빨개졌다가 다시 원래 색으로 돌아감. 콘솔 로그에 나타나는 번호를 기록해야 함
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
