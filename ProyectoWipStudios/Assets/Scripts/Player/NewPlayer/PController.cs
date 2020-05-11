using UnityEngine;

[RequireComponent(typeof(PMovementController))]
[RequireComponent(typeof(PSpecificController))]
public class PController : MonoBehaviour
{
    
    public static PController instance { get; private set; }

    public struct Stats
    {
        public Vector3 velocity;

        public float speed
        {
            get
            {
                return PController.instance.specificController.GetSpeed();
            }

            private set { }
        }

        public bool isGrounded;

        public override string ToString()
        {
            return "Player Stats"
                + "\nVelocity: " + velocity
                + "\nSpeed: " + speed
                + "\nIsGrounded: " + isGrounded;
        }
    }
    public Stats stats = new Stats();

    //Controllers
    public PMovementController movementController { get; private set; }
    public PSpecificController specificController { get; private set; }


    private void Awake()
    {
        if (instance)
            Destroy(instance.gameObject);

        instance = this;

        movementController = GetComponent<PMovementController>();
        specificController = GetComponent<PSpecificController>();

    }

    private void Update()
    {
        movementController.DoUpdate();
        specificController.DoUpdate();
    }

    private void FixedUpdate()
    {
        movementController.DoFixedUpdate();
        specificController.DoFixedUpdate();
    }

}
