using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun, IPunObservable
{
    private bool Ready; // 발사 준비 상태
    
    public Transform fireTransform; // 총알이 발사될 위치

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    public AudioClip shotClip; // 발사 소리

    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private float fireDistance = 50f; // 사정거리

    public float fireRate; // 총알 발사 간격

    public int damage;

    // 주기적으로 자동 실행되는, 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            // 현재 총의 상태를 네트워크를 통해 보내기
            stream.SendNext(Ready);
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨
            // 현재 총의 상태를 네트워크를 통해 받기
            Ready = (bool)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        // 사용할 점을 두개로 변경
        bulletLineRenderer.positionCount = 2;
        // 라인 렌더러를 비활성화
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 총의 현재 상태를 총을 쏠 준비가 된 상태로 변경
        Ready = true;
        // 마지막으로 총을 쏜 시점을 초기화
        lastFireTime = 0;
    }


    // 발사 시도
    public void Fire()
    {
        // 현재 상태가 발사 가능한 상태
        // && 마지막 총 발사 시점에서 timeBetFire 이상의 시간이 지남
        if (Ready && Time.time >= lastFireTime + fireRate)
        {
            // 마지막 총 발사 시점을 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
    {
        // 실제 발사 처리는 호스트에게 대리
        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient);
    }

    // 호스트에서 실행되는, 실제 발사 처리
    [PunRPC]
    public void ShotProcessOnServer()
    {
        // 레이캐스트에 의한 충돌 정보를 저장하는 컨테이너
        RaycastHit hit;
        // 총알이 맞은 곳을 저장할 변수
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트(시작지점, 방향, 충돌 정보 컨테이너, 사정거리)
        if (Physics.Raycast(fireTransform.position,
            fireTransform.forward, out hit, fireDistance))
        {
            // 레이가 어떤 물체와 충돌한 경우

            // 충돌한 상대방으로부터 IDamageable 오브젝트를 가져오기 시도
            IDamageable target =
                hit.collider.GetComponent<IDamageable>();

            // 상대방으로 부터 IDamageable 오브젝트를 가져오는데 성공했다면
            if (target != null)
            {
                // 상대방의 OnDamage 함수를 실행시켜서 상대방에게 데미지 주기
                target.OnDamage(damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }
        else
        {
            // 레이가 다른 물체와 충돌하지 않았다면
            // 총알이 최대 사정거리까지 날아갔을때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position +
                          fireTransform.forward * fireDistance;
        }

        // 발사 이펙트 재생, 이펙트 재생은 모든 클라이언트들에서 실행
        photonView.RPC("ShotEffectPrccessOnClients", RpcTarget.All, hitPosition);
    }

    // 이펙트 재생 코루틴을 랩핑하는 메서드
    [PunRPC]
    public void ShotEffectPrccessOnClients(Vector3 hitPosition)
    {
        StartCoroutine(ShotEffect(hitPosition));
    }


    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 총구 화염 효과 재생
        muzzleFlashEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(shotClip);

        // 선의 시작점은 총구의 위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // 선의 끝점은 입력으로 들어온 충돌 위치
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.enabled = true;
        

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 총알 궤적을 지운다
        bulletLineRenderer.enabled = false;
    }
}
