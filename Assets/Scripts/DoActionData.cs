using System;
using UnityEngine;

[Serializable]
public class DoActionData
{
    public float power;
    public int stopFrame;
    public float distance;

    public AudioClip hitAudioClip;

    public GameObject hitParticle;
    public Vector3 hitParticlePositionOffset;
    public Vector3 hitParticleScaleOffset = Vector3.one;
}
