using UnityEngine;

public class IKD_Material : MonoBehaviour
{
    void Start()
    {
        if (IKD_MaterialManager.Instance != null)
        {
            MeshRenderer mesh = GetComponent<MeshRenderer>();
            mesh.material = IKD_MaterialManager.Instance.GetMaterial();
        }
    }
}
