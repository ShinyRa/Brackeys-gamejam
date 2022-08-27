using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aciton", menuName = "Action")]
public class Action : ScriptableObject {
    public string actionName;
    public string animationClip;
    public string audioFile;
    public int successRate;
    public int healthModifier;
    public int margin;
    public string target;

    public float CalculateDamage() {
        if (!this.DoesHit()) {
            return 0f;
        }

        return -this.healthModifier + RandomHelper.Range(-margin, margin);
    }

    private bool DoesHit() {
        return RandomHelper.Range(1, 100) <= successRate;
    }
}