using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {
    public string actionName;
    public string animationClip;
    public string audioFile;
    public int successRate;
    public int healthModifier;
    public int margin;

    public Action(ActionData action) {
        (string _actionName, string _animationClip, string _audioFile, int _successRate, int _healthModifier, int _margin) = action;
        _actionName = actionName;
        _animationClip = animationClip;
        _audioFile = audioFile;
        _successRate = successRate;
        _healthModifier = healthModifier;
        _margin = margin;
    }
    
    public float CalculateDamage() {
        if (!this.DoesHit()) {
            return 0f;
        }

        return this.healthModifier + RandomHelper.Range(-margin, margin);
    }

    private bool DoesHit() {
        return RandomHelper.Range(1, 100) < successRate;
    }
}