/* 
    Adapted from and inspired by the following sources:
    - Natty GameDev: https://youtu.be/gPPGnpV1Y1c?si=SAQ9WT-HTEw2CLDQ
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class InteractableInterface : MonoBehaviour
{
    public string interactText;
    [SerializeField] public TMP_FontAsset font;
    [SerializeField] public ParticleSystem[] particle;
    [SerializeField] public AudioClip clip;
    public virtual void interact() {}

}
