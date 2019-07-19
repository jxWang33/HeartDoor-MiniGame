using UnityEngine;
using System.Collections;

public class CameraWaterWave : MonoBehaviour
{
    private Camera cam;
    public AnimationCurve waveform = new AnimationCurve(
        new Keyframe(0.00f, 0.50f, 0, 0), new Keyframe(0.05f, 1.00f, 0, 0), new Keyframe(0.15f, 0.10f, 0, 0), new Keyframe(0.25f, 0.80f, 0, 0),
        new Keyframe(0.35f, 0.30f, 0, 0), new Keyframe(0.45f, 0.60f, 0, 0), new Keyframe(0.55f, 0.40f, 0, 0), new Keyframe(0.65f, 0.55f, 0, 0),
        new Keyframe(0.75f, 0.46f, 0, 0), new Keyframe(0.85f, 0.52f, 0, 0), new Keyframe(0.99f, 0.50f, 0, 0)
    );//预设的涟漪振幅曲线
    [Range(0.01f, 1.0f)]
    public float refractionStrength = 0.5f;//折射强度
    public Color reflectionColor = Color.gray;//反射默认色
    [Range(0.01f, 1.0f)]
    public float reflectionStrength = 0.7f;//反射效果强度
    [Range(0.0f, 3.0f)]
    public float waveSpeed = 1.25f;//传播速度
    [SerializeField]
    private Shader shader;
    class Droplet
    {
        Vector2 position;
        float time = 1000.0f;
        public Droplet() { }
        public void Reset(Vector2 pos) {
            position = new Vector2(pos.x/1920,pos.y/1080);
            time = 0;
        }
        public void Update() {
            time += Time.deltaTime;
        }
        public Vector4 MakeShaderParameter(float aspect) {
            return new Vector4(position.x * aspect, position.y, time, 0);
        }
    }
    Droplet droplet;
    Texture2D gradTexture;
    Material material;
    void UpdateShaderParameters()//更新shader参数
    {
        material.SetVector("_Drop", droplet.MakeShaderParameter(cam.aspect));
        material.SetColor("_Reflection", reflectionColor);
        material.SetVector("_Params1", new Vector4(cam.aspect, 1, 1 / waveSpeed, 0));
        material.SetVector("_Params2", new Vector4(1, 1 / cam.aspect, refractionStrength, reflectionStrength));
    }
    void Awake() {
        cam = GetComponent<Camera>();//获取相机组件
        if (cam == null)
            Debug.Log("error  it is null");
        droplet = new Droplet();
        gradTexture = new Texture2D(2048, 1, TextureFormat.Alpha8, false) {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear
        };
        for (var i = 0; i < gradTexture.width; i++) {
            var x = 1.0f / gradTexture.width * i;
            var a = waveform.Evaluate(x);
            gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
        }//初始化振幅贴图（也就是把waveform曲线初始化到gradTexture上面）
        gradTexture.Apply();
        material = new Material(shader) {
            hideFlags = HideFlags.DontSave
        };
        material.SetTexture("_GradTex", gradTexture);
        UpdateShaderParameters();
    }
    void Update() {
        droplet.Update();//更新涟漪
        UpdateShaderParameters();
    }
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, material); //效果作用于画面
    }

    public void Emit(Vector2 pos)
    {
        droplet.Reset(pos);
    }
}