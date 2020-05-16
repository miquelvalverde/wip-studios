using UnityEngine;

public class EnemySenses : MyMonoBehaivour
{

    [SerializeField] private float chaseMaxDistance = 0;
    [SerializeField] private float shootDistance = 0;

    [SerializeField] private Enemy self = null;

    private void Update()
    {
        if (self.stats.isSeeingPlayer && CheckPlayerDistance() > chaseMaxDistance)
            self.stats.isPlayerFar = true;

        if (self.stats.isSeeingPlayer && CheckPlayerDistance() < shootDistance)
            self.stats.isPlayerClose = true;
    }

    private float CheckPlayerDistance()
    {
        return Vector3.Distance(player.transform.position, self.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            self.stats.isSeeingPlayer = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!self){
            Debug.LogWarning("Enemy of " + gameObject + " is null.", gameObject);
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(self.position, chaseMaxDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(self.position, shootDistance);

    }

}
