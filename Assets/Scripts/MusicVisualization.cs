using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisualization : MonoBehaviour
{
    //音频相关
    public AudioSource thisAudioSource;
    private float[] spectrumData = new float[8192];
    private float[] avg = new float[50];
    //cube相关
    public GameObject cubePrototype;
    public Transform startPoint;
    private Transform[] cube_transforms = new Transform[50];
    private Vector3[] cubes_position = new Vector3[50];
    //颜色相关
    private MeshRenderer[] cube_meshRenderers = new MeshRenderer[50];
    private bool cubeColorChange;
    // Start is called before the first frame update
    void Start()
    {
        //cube生成与排列
        Vector3 p = startPoint.position;

        for (int i = 0; i < 43; i++)
        {
            GameObject cube = Object.Instantiate(cubePrototype, p, cubePrototype.transform.rotation) as GameObject;
            cube_transforms[i] = cube.transform;
            cube_meshRenderers[i] = cube.GetComponent<MeshRenderer>();
            cube_transforms[i].parent = startPoint;
        }

        
        //颜色相关
        cubeColorChange = false;
        Invoke("SwitchCC", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCube();
        Spectrum2Cube();
        DynamicColor();
    }
    void SwitchCC()
    {
        cubeColorChange = !cubeColorChange;
    }
    private void MoveCube()
    {
        Vector3 p = startPoint.position;
        float a = 2f * Mathf.PI / 43;

        for (int i = 0; i < 43; i++)
        {
            cube_transforms[i].position = new Vector3(p.x + Mathf.Cos(a) * 2, 0.0f, p.z + Mathf.Sin(a) * 2);
            a += 2f * Mathf.PI / 43;
            cubes_position[i] = cube_transforms[i].position;
            
        }
    }
    void DynamicColor()
    {
        if (thisAudioSource != null)
        {
            if (cubeColorChange)
            {

                for (int i = 0; i < 43; i++)
                {
                    cube_meshRenderers[i].material.SetColor("_Color", new Vector4(Mathf.Lerp(cube_meshRenderers[i].material.color.r, avg[i] * 500f, 0.2f), 0.5f, 1f, 1f));
                }
            }
        }
    }
    //thisAudioSource当前帧频率波功率，传到对应cube的localScale
    void Spectrum2Cube()
    {
        if (thisAudioSource != null)
        {
            thisAudioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

            for (int i = 0; i < 5461; i++) avg[i / 127] += spectrumData[i];
            for (int i = 0; i < 43; i++) avg[i] /= 127;
            for (int i = 0; i < 43; i++)
            {
                cube_transforms[i].localScale = new Vector3(0.2f, Mathf.Lerp(cube_transforms[i].localScale.y, avg[i] * 10000f, 0.5f), 0.2f);
            }
        }
    }
}
