﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank
{
    const float EMPTY_THRESHOLD = 0.1f;
    float capacity;
    float currentLevel;

    public FuelTank(float capacity) {
        currentLevel = 0;
        this.capacity = capacity;
    }

    public void RechargeFull() {
        Recharge(capacity);
    }

    public void Recharge(float amount) {
        currentLevel = Mathf.Min(currentLevel + amount, capacity);
    }

    public void Drain(float amount) {
        currentLevel = Mathf.Max(0, currentLevel - amount);
    }

    public float GetLevel() {
        return currentLevel;
    }

    public float GetPercentage() {
        return currentLevel / capacity;
    }

    public void SetCapacity(float newCapacity, bool scaling) {
        if (scaling) {
            currentLevel = newCapacity * GetPercentage();
        }
        capacity = newCapacity;
    }

    public bool isEmpty() {
        return currentLevel < EMPTY_THRESHOLD;
    }
}