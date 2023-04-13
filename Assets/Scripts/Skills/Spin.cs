using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : Skill
{
    public int damage;
    
    void Start()
    {
        name = "Spin of the Abyss";
        description = "Player spins and hits all enemies around themself";
    }

    public void SetDuration(float animationDuration)
    {
        skillDuration = animationDuration;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }
}
