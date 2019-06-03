using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMManager : MonoBehaviour
{
    public static readonly System.Random rd = new System.Random(GetRandomSeed());//随机种子

    public const KeyCode UP_KEY = KeyCode.W;
    public const KeyCode DOWN_KEY = KeyCode.S;
    public const KeyCode LEFT_KEY = KeyCode.A;
    public const KeyCode RIGHT_KEY = KeyCode.D;
    public const KeyCode JUMP_KEY = KeyCode.J;
    public const KeyCode DASH_KEY = KeyCode.K;
    public const KeyCode DOOR_KEY = KeyCode.L;
    public const KeyCode FUNC_KEY = KeyCode.E;
    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
    }

    private static int GetRandomSeed() {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}
