using DG.Tweening;
using System;
using UnityEngine;
using UniRx;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Player owner;
    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private string targetTag;
    private bool ActiveMode = true;

    [SerializeField]
    private int Damage;

    [Header("Bullet")]
    [SerializeField]
    private GameObject explosivePrefab;
    [SerializeField]
    private float explosiveLifeTime;

    [Header("Fire rate")]
    [SerializeField]
    private Vector3 recoilAngle;
    [SerializeField]
    private float cooldown;
    private float currentCooldown;

    [Header("Ammo")]
    [SerializeField]
    private int ammoInClip;
    public IntReactiveProperty CurrentAmmoInClip;
    [field: SerializeField]
    public int maxAmmo { get; private set; }

    [SerializeField]
    private float reloadTime;
    private float currentReloadTime;
    [SerializeField]
    private Vector3 reloadAngle;

    void Update()
    {
        Cooldowns();

        if (!ActiveMode)
            return;

        Inputs();
    }

    private void Cooldowns()
    {
        if (currentCooldown > 0)
            currentCooldown = Math.Max(currentCooldown - Time.deltaTime, 0);

        if (currentReloadTime > 0)
            currentReloadTime = Math.Max(currentReloadTime - Time.deltaTime, 0);
    }
    private void Inputs()
    {
        if (Input.GetMouseButtonDown(0) && currentCooldown == 0 && currentReloadTime == 0)
        {
            if (CurrentAmmoInClip.Value > 0)
                Fire();
            else
                Reload();
        }

        else if (Input.GetKeyDown(KeyCode.R) && CurrentAmmoInClip.Value < ammoInClip)
        {
            Reload();
        }
    }

    public void SwitchCursor()
    {
        ActiveMode = !ActiveMode;
    }
    public void Activate()
    {
        ActiveMode = true;
    }

    private void Reload()
    {
        if (currentReloadTime != 0 && maxAmmo <= 0)
            return;

        currentReloadTime = reloadTime;
        ReloadAnimation();
    }

    private void Fire()
    {
        RecoilAnimation();

        CurrentAmmoInClip.Value--;
        currentCooldown += cooldown;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            GameObject explosive = Instantiate(explosivePrefab, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
            Destroy(explosive, explosiveLifeTime);
            if (raycastHit.transform.tag == targetTag)
            {
                Destroy(raycastHit.transform.gameObject);
                owner.TakeHit(1);
            }
        }
    }

    private void ReloadAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        var toEndRotation = transform.DOLocalRotate(reloadAngle, reloadTime / 2f, RotateMode.Fast);
        var toStartRotation = transform.DOLocalRotate(Vector3.zero, reloadTime/2f, RotateMode.Fast);

        sequence.Append(toEndRotation);
        sequence.Append(toStartRotation);

        sequence.OnComplete(OnReloadComplite);

        sequence.Play();
        
    }
    private void OnReloadComplite()
    {
        int ammo = Math.Min(maxAmmo, ammoInClip - CurrentAmmoInClip.Value);
        maxAmmo -= ammo;
        CurrentAmmoInClip.Value += ammo;
    }

    private void RecoilAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        var toEndRotation = transform.DOLocalRotate(recoilAngle, 0.15f, RotateMode.Fast);
        var toStartRotation = transform.DOLocalRotate(Vector3.zero, 0.15f, RotateMode.Fast);

        sequence.Append(toEndRotation);
        sequence.Append(toStartRotation);
        sequence.Play();
    }
}
