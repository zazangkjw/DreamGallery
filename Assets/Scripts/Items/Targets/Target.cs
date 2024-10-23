using UnityEngine;

public class Target : MonoBehaviour
{
    public int team;

    public GameObject original;
    public GameObject[] parts;
    Vector3[] parts_position;
    Vector3[] parts_rotation;


    private void Start()
    {
        parts_position = new Vector3[parts.Length];
        parts_rotation = new Vector3[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            parts_position[i] = parts[i].transform.position;
            parts_rotation[i] = parts[i].transform.eulerAngles;
        }
    }

    void FixedUpdate()
    {
        
    }

    // 파괴
    public void DestoryTarget()
    {
        original.SetActive(false);
        foreach (GameObject part in parts)
        {
            part.SetActive(true);
        }
    }

    // 상태 리셋
    public void ResetTarget()
    {
        original.SetActive(true);
        foreach (GameObject part in parts)
        {
            part.SetActive(false);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i].transform.position = parts_position[i];
                parts[i].transform.eulerAngles = parts_rotation[i];
            }
        }
    }
}
