using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMManager : MonoBehaviour
{
    public static readonly System.Random rd = new System.Random(GetRandomSeed());//随机种子
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
