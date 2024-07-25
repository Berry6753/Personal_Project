using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [Header("알파, 오메가 공격 파티클")]
    [SerializeField] private ParticleSystem aoParticle;
    public ParticleSystem AOParticle { get { return aoParticle; } }

    [Header("위클라인 공격 파티클")]
    [SerializeField] private ParticleSystem wicklineParticle;
    public ParticleSystem WicklineParticle { get {  return wicklineParticle; } }

    [Header("통행금지 벽 파티클")]
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
