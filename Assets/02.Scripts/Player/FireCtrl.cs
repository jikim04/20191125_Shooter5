using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//총알 발사와 재장전 오디오 클립을 저장할 구조체
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    //무기 타입
    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    }
    //주인공이 현재 들고 있는 무기를 저장할 변수
    public WeaponType currWeapon = WeaponType.RIFLE;

    //총알 프리팹
    public GameObject bullet;

    //탄피 추출 파티클
    public ParticleSystem cartridge;
    //총구 화염 파티클
    private ParticleSystem muzzleFlash;
    //AudioSource 컴포넌트를 저장할 변수
    private AudioSource _audio;

    //총알 발사좌표
    public Transform firePos;
    //오디오 클립을 저장할 변수
    public PlayerSfx playerSfx;

    private Shake shake;
    public Image magazineImg;
    public Text magazineText;
    public int maxBullet = 10;
    public int remainingBullet = 10;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    public Sprite[] weaponIcons;
    public Image weaponImage;


    void Start()
    {
        //FirePos 하위에 있는 컴포넌트 추출
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        //AudioSource 컴포넌트 추출
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    void Update()
    {   if (EventSystem.current.IsPointerOverGameObject()) return;
        //마우스 왼쪽 버튼을 클릭했을 때 Fire 함수 호출
        if (!isReloading && Input.GetMouseButtonDown(0))
        {
            --remainingBullet;
            Fire();
        }
        if (remainingBullet == 0)
        {
            StartCoroutine(Reloading());
        }

    }

    void Fire()
    {
        StartCoroutine(shake.ShakeCamera(0.02f,0.05f,0.1f));
        //Bullet 프리팹을 동적으로 생성
        //Instantiate(bullet, firePos.position, firePos.rotation);
        //파티클 실행
        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        cartridge.Play();
        //총구 화염 파티클 실행
        muzzleFlash.Play();
        //사운드 발생
        FireSfx();
        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();
    }

    void FireSfx()
    {
        //현재 들고 있는 무기의 오디오 클립을 가져옴
        var _sfx = playerSfx.fire[(int)currWeapon];
        //사운드 발생
        _audio.PlayOneShot(_sfx, 1.0f);
    }

    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1.0f);

        yield return new WaitForSeconds(playerSfx.reload[(int)currWeapon].length + 0.3f);

        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }

    void UpdateBulletText()
    { magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}",remainingBullet, maxBullet);

    }

    public void OnChangeWeapon()
    {
        currWeapon = (WeaponType)((int)++currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }

}