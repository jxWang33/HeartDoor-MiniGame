using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class UsrDustEffect : MonoBehaviour
{
    public ParticleSystem hand;
    public ParticleSystem leg;
    public ParticleSystem land;

    private float handStartTime;
    private float legStartTime;
    private float landStartTime;

    private bool isOn = false;

    private void Awake() {
        handStartTime = hand.emission.rateOverTime.constant;
        legStartTime = leg.emission.rateOverTime.constant;
        landStartTime= land.emission.rateOverTime.constant;

        CancelClimbEffect();
        CancelLandEffect();
    }

    public void SetClimbEffect() {
        if (isOn)
            return;
        EmissionModule temp = hand.emission;
        temp.rateOverTime = new MinMaxCurve(handStartTime);
        temp = leg.emission;
        temp.rateOverTime = new MinMaxCurve(legStartTime);
        isOn = true;
    }

    public void SetLandEffect() {
        if (isOn)
            return;
        EmissionModule temp = land.emission;
        temp.rateOverTime = new MinMaxCurve(landStartTime);
        isOn = true;
    }

    public void CancelClimbEffect() {
        EmissionModule temp = hand.emission;
        temp.rateOverTime = new MinMaxCurve(0);
        temp = leg.emission;
        temp.rateOverTime = new MinMaxCurve(0);
        isOn = false;
    }

    public void CancelLandEffect() {
        EmissionModule temp = land.emission;
        temp.rateOverTime = new MinMaxCurve(0);
        isOn = false;
    }
}
