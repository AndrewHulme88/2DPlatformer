using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] KeyCode grappleKey = KeyCode.LeftAlt;
    [SerializeField] float ropeClimbSpeed = 2f;
    [SerializeField] float aimSpeed = 120f;
    [SerializeField] float initialAngle = 90f;
    [SerializeField] float swingForce = 10f;

    private Vector2 aimDirection = Vector2.right;
    private bool isAiming = false;
    private bool ropeAttached = false;
    private float currentAimAngle;
    private HingeJoint2D joint;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private GameObject grappleAnchor;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        joint = gameObject.AddComponent<HingeJoint2D>();
        joint.enabled = false;
        joint.autoConfigureConnectedAnchor = false;
        joint.useLimits = false;
        joint.enableCollision = false;
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        currentAimAngle = initialAngle;
    }

    private void Update()
    {
        HandleAimingInput();

        if(Input.GetKeyDown(grappleKey))
        {
            isAiming = true;

            if(playerController != null)
            {
                playerController.canMove = false;
            }
        }
        else if(Input.GetKeyUp(grappleKey))
        {
            if(isAiming)
            {
                FireRope();
            }

            isAiming = false;
            lineRenderer.enabled = false;

            if(!ropeAttached && playerController != null)
            {
                playerController.canMove = true;
            }
        }

        if(isAiming)
        {
            UpdateAimingLine();
        }

        if(ropeAttached)
        {
            //playerRb.gravityScale = playerController.startingGravity * 10f;

            float swingInput = Input.GetAxisRaw("Horizontal");
            playerRb.AddForce(transform.right * swingInput * swingForce, ForceMode2D.Force);

            //HandleRopeClimb();
            UpdateRopeLine();

            if(Input.GetKeyDown(grappleKey))
            {
                DetachRope();

                Vector2 jumpDirection = playerRb.linearVelocity.normalized;
                playerRb.AddForce(jumpDirection * 5f, ForceMode2D.Impulse);
            }
        }
        //else
        //{
        //    playerRb.gravityScale = playerController.startingGravity;
        //}
    }

    private void HandleAimingInput()
    {
        if (!isAiming) return;

        if (Input.GetKey(KeyCode.RightArrow)) currentAimAngle -= aimSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow)) currentAimAngle += aimSpeed * Time.deltaTime;

        // Optional Up / Down to control pitch
        //if (Input.GetKey(KeyCode.UpArrow)) currentAimAngle = Mathf.Clamp(currentAimAngle, -89f, 89f);
        //if (Input.GetKey(KeyCode.DownArrow)) currentAimAngle = Mathf.Clamp(currentAimAngle, -89f, 89f);

        if (currentAimAngle > 360f) currentAimAngle -= 360f;
        if (currentAimAngle < 0f) currentAimAngle += 360f;

        Vector2 lastAimDirection = new Vector2(Mathf.Cos(currentAimAngle * Mathf.Deg2Rad), Mathf.Sin(currentAimAngle * Mathf.Deg2Rad)).normalized;

        aimDirection = lastAimDirection;

        Vector2 aimOrigin = firePoint.position;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, aimOrigin);
        lineRenderer.SetPosition(1, aimOrigin + aimDirection * maxDistance);

    }

    private void UpdateAimingLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, firePoint.position + (Vector3)(aimDirection * maxDistance));
    }

    private void FireRope()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, aimDirection, maxDistance, grappleLayer);

        if(hit.collider != null)
        {
            if(grappleAnchor == null)
            {
                grappleAnchor = new GameObject("GrappleAnchor");
                grappleAnchor.transform.position = hit.point;
                grappleAnchor.transform.parent = null;

                Rigidbody2D anchorRb = grappleAnchor.AddComponent<Rigidbody2D>();
                anchorRb.bodyType = RigidbodyType2D.Static;
            }

            Debug.Log("Grapple hit: " + hit.collider.name);
            joint.enabled = true;
            joint.connectedBody = grappleAnchor.GetComponent<Rigidbody2D>();
            joint.connectedAnchor = Vector2.zero;
            joint.anchor = firePoint.localPosition;
            joint.autoConfigureConnectedAnchor = false;
            joint.useLimits = false;
            ropeAttached = true;
            lineRenderer.positionCount = 2;
        }
        else
        {
            Debug.Log("Grapple missed");
        }
    }

    private void UpdateRopeLine()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, joint.connectedAnchor);
    }

    //private void HandleRopeClimb()
    //{
    //    float climbInput = Input.GetAxisRaw("Vertical");
    //    joint.distance -= climbInput * ropeClimbSpeed * Time.deltaTime;
    //    joint.distance = Mathf.Clamp(joint.distance, 1f, maxDistance);
    //}

    private void DetachRope()
    {
        ropeAttached = false;
        joint.enabled = false;
        lineRenderer.enabled = false;

        if(playerController != null)
        {
            playerController.canMove = true;
        }
    }
}
