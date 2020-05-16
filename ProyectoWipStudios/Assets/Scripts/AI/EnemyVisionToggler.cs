using UnityEngine;

public class EnemyVisionToggler : AMonoBehaivourWithInputs
{
    [SerializeField] private MeshRenderer[] meshRenderer = null;
    private bool visionMeshesAreOcult;

    private void Start()
    {
        visionMeshesAreOcult = false;
        ToggleVisionMaterial();
    }

    protected override void SetControls()
    {
        controls.Debug.AIVision.performed += _ => ToggleVisionMaterial();
    }

    private void ToggleVisionMaterial()
    {
        visionMeshesAreOcult = !visionMeshesAreOcult;
        foreach(MeshRenderer m in meshRenderer)
        {
            m.enabled = !visionMeshesAreOcult;
        }
        
    }
}
