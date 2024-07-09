using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private ParticleSystem aoParticle;
    public ParticleSystem AOParticle { get { return aoParticle; } }
    [SerializeField] private ParticleSystem wicklineParticle;
    public ParticleSystem WicklineParticle { get {  return wicklineParticle; } }
}
