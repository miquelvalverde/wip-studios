using System.Collections;
using UnityEngine;

public class DestroyGradually : MonoBehaviour
{
    [SerializeField] private float time = 2;

    private void Awake()
    {
        foreach (Transform child in transform)
        {   
            var renderer = child.GetComponent<Renderer>();
            // copy material
            var material = renderer.material;
            // create new intstance material
            var materialInstance = new Material(material);
            // apply material
            renderer.material = materialInstance;
            // gradual destroy
            StartCoroutine(DestroyRoutine(materialInstance));
        }
    }

    IEnumerator DestroyRoutine(Material material)
    {
        SoundManager.RockShatter.start();
        float stepTime = 0.01F;
        float numSteps = time / stepTime;
        float alphaStep = 1F / numSteps;
        for (int t = 0; t < numSteps; t++)
        {   
            // decrement alpha
            var color = material.color;
            color.a -= alphaStep;
            material.color = color;
            // don't freeze Unity
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
