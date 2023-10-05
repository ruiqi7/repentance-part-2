using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class InteractableInterface : MonoBehaviour
{
    public string interactText;
    [SerializeField] public TMP_FontAsset font;
    [SerializeField] public ParticleSystem particle;
    [SerializeField] public AudioClip clip;
    public virtual void interact() {}

}
