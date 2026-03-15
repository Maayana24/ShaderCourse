using UnityEngine;

struct TriangleData
{
    public Vector3 positionOffset;
    public Vector3 velocity;
    public float lifetime;
    public Vector3 acceleration;
}

public class ComputeShaderManager : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] private Renderer renderer;
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private float explosionPower = 5;

    private int triangleCount;
    private int kernel;

    private ComputeBuffer buffer;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        kernel = computeShader.FindKernel("CSMain");

        triangleCount = mesh.triangles.Length / 3;

        buffer = new ComputeBuffer(triangleCount, 10 * 4);

        TriangleData[] data = new TriangleData[triangleCount];
        for (int i = 0; i < triangleCount; i++)
        {
            data[i].positionOffset = Vector3.zero;
            data[i].velocity = Random.insideUnitSphere * explosionPower;
            data[i].lifetime = Random.Range(1, 3);
            data[i].acceleration = -data[i].velocity * 0.5f;
        }
        buffer.SetData(data);

        computeShader.SetBuffer(kernel, "triangleBuffer", buffer);
        renderer.material.SetBuffer("triangleBuffer", buffer);
        computeShader.SetInt("triangleCount", triangleCount);

    }

    private void Update()
    {
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.Dispatch(kernel, triangleCount / 64 + 1, 1, 1);
    }
    void OnDestroy()
    {
        buffer.Release();
    }
}
