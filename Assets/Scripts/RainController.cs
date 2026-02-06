using UnityEngine;

public class RainController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float rainIntensity = 0.5f;
    void Update()
    {
        Shader.SetGlobalFloat("_RainIntensity", rainIntensity);
    }
}
