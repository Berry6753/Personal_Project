using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [Header("����, ���ް� ���� ��ƼŬ")]
    [SerializeField] private ParticleSystem aoParticle;
    public ParticleSystem AOParticle { get { return aoParticle; } }

    [Header("��Ŭ���� ���� ��ƼŬ")]
    [SerializeField] private ParticleSystem wicklineParticle;
    public ParticleSystem WicklineParticle { get {  return wicklineParticle; } }

    [Header("������� �� ��ƼŬ")]
    [SerializeField] private ParticleSystem wallParticle;
    public ParticleSystem WallParticle { get { return wallParticle; } }

    public void OnPlayWallParticle(Vector3 pos)
    { 
        wallParticle.transform.position = pos;
        wallParticle.Play();
    }
    public void OnStopWallParticle()
    { 
        wallParticle.Stop();
    }
}
