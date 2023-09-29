using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public abstract class InteractableInterface : MonoBehaviour
{
    public string interactText;
    [SerializeField] public TMP_FontAsset font;
    [SerializeField] public ParticleSystem particle;
    public virtual void interact() {}

}
