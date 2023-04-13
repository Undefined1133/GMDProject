using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    public string name;
    public string description;
    public Sprite icon;
    public float skillDuration = 1f;
    protected bool skillActive = false;

    
    
    public virtual void ActivateSkill() {
        skillActive = true;
        StartCoroutine(DeactivateSkill());
    }

    protected IEnumerator DeactivateSkill() {
        yield return new WaitForSeconds(skillDuration);
        skillActive = false;
    }

}
