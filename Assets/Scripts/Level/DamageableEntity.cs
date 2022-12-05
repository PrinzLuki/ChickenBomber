using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (SoundManagmentComponent))]
public class DamageableEntity : BaseStats
{
    [SerializeField] protected int rewardPoints;
    [SerializeField] protected Vector3 popUpSpawnOffset;

    static public Action<int,Vector3, GameObject> OnDestroyObject;
    static public event Action OnColission;

    Rigidbody rb;
    SoundManagmentComponent soundManagmentComponent;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        soundManagmentComponent = GetComponent<SoundManagmentComponent>();
    }
    public override void GetDmg(Rigidbody rb, float baseDmg)
    {
        CalculateDmg(out float dmg, rb);
        if (dmg == 0) return;
        
        PlayDmgedSound();
        this.rb.isKinematic = false;
        rb.drag = 0.1f;
        health -= dmg + baseDmg;

        if (health <= 0)
        {
            OnDestroyObject?.Invoke(rewardPoints, transform.position + popUpSpawnOffset, this.gameObject);
            Destroy(gameObject);
        }
    }
    void PlayDmgedSound()
    {
        var soundManager = (InGameSoundManager)SoundManager.Instance;
        soundManager.SpawnSoundObject(soundManagmentComponent.GetAudioClipFromSoundFileOfType(SoundFileType.Hit), transform.position, AudioMixerGroupType.Sfx);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.TryGetComponent(out IDamageable damageable) && rb.velocity.magnitude >= velocityThreshHold)
        {
            this.rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            damageable.GetDmg(rb, GetDmgValue());
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bird"))
        {
            this.rb.isKinematic = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            this.rb.constraints = RigidbodyConstraints.FreezePositionZ;
            this.rb.isKinematic = false;
        }
    }
}
