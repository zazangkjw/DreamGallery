using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClownColliderTrigger : MonoBehaviour
{
    [SerializeField]
    Collider trigger_Chase; // 광대 추격 시작 트리거 콜라이더

    [SerializeField]
    Collider trigger_SafeZone; // 안전 구역 트리거 콜라이더

    [SerializeField]
    Clown_Chase clown_Chase;
    public Collider clown_chase_col;

    [SerializeField]
    Collider[] triggers_CircusSuccess;

    [SerializeField]
    CircusFlash circusFlash;

    [SerializeField]
    Collider Ladder_Trigger;

    Rigidbody myRigid;

    [SerializeField]
    ClownRaycast clownRaycast;

    bool is_falling;
    public GameObject bed;
    public AudioSource bounce_sound;
    public AudioSource neck_crack_sound;
    public AudioSource wind_sound;

    bool is_chasing_music_on;
    bool pre_is_chasing_music_on;
    public AudioSource chasing_music;
    Coroutine audio_coroutine;

    public bool[] isSuccess = new bool[1];

    public ClownWorm clownWorm;

    public Collider fallTrigger;
    public AudioSource scream;

    public RawImage fadeInOutImage; // 페이드-인, 아웃 이미지
    public FadeInOutScript fadeInOutScript; // 페이드-인, 아웃 스크립트




    void Start()
    {
        wind_sound.Stop();
        is_falling = true;
        myRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (is_falling)
        {
            if (collision.gameObject == bed)
            {
                bounce_sound.Play();
            }
            else
            {
                neck_crack_sound.Play();
                StartCoroutine(DieCoroutine());
            }
            is_falling = false;
            wind_sound.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 안전 구역 트리거
        if (other == trigger_SafeZone)
        {
            ChasingMusicControl(false, 0f);
            clown_Chase.isSafe = true;
        }

        // 추격 시작 트리거에 들어가면 플래시 OFF
        if (other == trigger_Chase && clown_Chase.isSafe == false)
        {
            ChasingMusicControl(true, 0.7f);
            circusFlash.isFlashOn = false;
        }

        // 서커스 도전 성공 트리거
        for(int i = 0; i < triggers_CircusSuccess.Length; i++)
        {
            if (!isSuccess[i] && other == triggers_CircusSuccess[i] && clownRaycast.life > 0)
            {
                isSuccess[i] = true;
                circusFlash.isFlashOn = true;
                StartCoroutine(AudioOnOffScript.VolumeCoroutine(clownRaycast.applause, true, 2f, 0.5f));
                clownRaycast.yay.Play();
                // clownRaycast.circusSong.Stop();
                clownRaycast.elevatorAnims[3].Play("Open");
            }
        }

        // 실패하면 재시작
        if(other.gameObject == clownWorm.deadTrigger || other == fallTrigger || other == clown_chase_col)
        {
            if(other == clown_chase_col)
            {
                clown_Chase.sound_and_collider.transform.parent.gameObject.SetActive(false);
                neck_crack_sound.Play();
            }
            is_falling = false;
            wind_sound.Stop();
            clownWorm.deadTrigger.SetActive(false);
            fallTrigger.enabled = false;
            StartCoroutine(DieCoroutine());
        }
    }

    private void OnTriggerStay(Collider other)

    {
        // 추격 시작 트리거에 들어가면 광대 추격 시작
        if (other == trigger_Chase)
        {
            clown_Chase.isChasing = true;
        }

        // 사다리 트리거
        if (other == Ladder_Trigger)
        {
            UpLadder();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 추격 시작 트리거에서 나가면 광대 추격 종료
        if (other == trigger_Chase)
        {
            ChasingMusicControl(false, 0f);
            clown_Chase.isChasing = false;
        }
        if (other == trigger_SafeZone)
        {
            //clown_Chase.isSafe = false;
        }
    }

    // 광대 추격 음악 재생
    void ChasingMusicControl(bool turnOnOff, float volume)
    {
        if (turnOnOff != pre_is_chasing_music_on)
        {
            pre_is_chasing_music_on = turnOnOff;
            if (audio_coroutine != null)
            {
                StopCoroutine(audio_coroutine);
            }
            audio_coroutine = StartCoroutine(AudioOnOffScript.VolumeCoroutine(chasing_music, turnOnOff, 1f, volume));
        }
    }

    // 사다리 기능
    private void UpLadder()
    {
        if (Input.GetKey(KeyCode.W))
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, 2.5f, myRigid.velocity.z);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                myRigid.velocity = new Vector3(myRigid.velocity.x, 5f, myRigid.velocity.z);
            }
        }
        else
        {
            myRigid.velocity = new Vector3(myRigid.velocity.x, -2.5f, myRigid.velocity.z);
        }
    }

    // 죽고 다시시작
    IEnumerator DieCoroutine()
    {
        fadeInOutImage.color = Color.black;
        scream.Play();
        yield return new WaitForSeconds(2f);
        LoadSceneScript.FailLoadScene("04_Clown");
    }
}
