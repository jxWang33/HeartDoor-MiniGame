using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class UsrDustEffect : MonoBehaviour
{
    public ParticleSystem hand;
    public ParticleSystem leg;

    private float handStartTime;
    private float legStartTime;

    private bool isOn = false;

    private void Awake() {
        handStartTime = hand.emission.rateOverTime.constant;
        legStartTime = leg.emission.rateOverTime.constant;

        CancelEffect();
    }

    public void SetEffect() {
        if (isOn)
            return;
        EmissionModule temp = hand.emission;
        temp.rateOverTime = new MinMaxCurve(handStartTime);
        temp = leg.emission;
        temp.rateOverTime = new MinMaxCurve(legStartTime);
        isOn = true;
    }

    public void CancelEffect() {
        EmissionModule temp = hand.emission;
        temp.rateOverTime = new MinMaxCurve(0);
        temp = leg.emission;
        temp.rateOverTime = new MinMaxCurve(0);
        isOn = false;
    }
}
