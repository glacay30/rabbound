﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGenerator
{
    Buff.BuffType type;

    public Buff GetBuff() {
        if (type == Buff.BuffType.FuelBuff) {
            return new FuelBuff();
        }
        if (type == Buff.BuffType.SpeedBuff) {
            return new SpeedBuff();
        }
        if (type == Buff.BuffType.ToxicDebuff) {
            return new ToxicDebuff();
        }
        if (type == Buff.BuffType.Blackhole) {
            return new Blackhole();
        }
        if (type == Buff.BuffType.SlowDebuff) {
            return new SlowDebuff();
        }
        return null;
    }

    public BuffGenerator Type(Buff.BuffType type) {
        this.type = type;
        return this;
    }
}
