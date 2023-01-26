using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed, gravitiyModifier, jumpPower,runSpeed=10f;
    public CharacterController charCon;
    private Vector3 moveInput;


    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;
    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;


    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;

    public Gun activeGun;
    public List<Gun> allGuns = new List<Gun>();
    public int currentGun;

    public GameObject muzzleFlash;



    public float maxViewAngle = 60f;

    
    private void Awake()
    {
        instance = this;
    }

    // Start wird aufgerufen, bevor das erste Update
    void Start()
    {
        currentGun --;
        SwitchGun();


    }
    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy&&!GameManager.instance.levelEnding)
        {

            //store y velocity
            float yStore = moveInput.y;
            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horiMove + vertMove;
            moveInput.Normalize();


            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }



            moveInput *= moveSpeed;

            moveInput.y = yStore;

            moveInput.y += Physics.gravity.y * gravitiyModifier * Time.deltaTime;
            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravitiyModifier * Time.deltaTime;
            }


            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;
            if (canJump)
            {
                canDoubleJump = false;
            }

            //handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
            }
            else if (canDoubleJump && Input.GetKeyDown(KeyCode.Space))
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }

            charCon.Move(moveInput * Time.deltaTime);

            //kontrolliere Kamera Rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            if (invertX)
            {
                mouseInput.x *= -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y *= -mouseInput.y;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));


            if (camTrans.rotation.eulerAngles.x > maxViewAngle && camTrans.rotation.eulerAngles.x < 180f)
            {
                camTrans.rotation = Quaternion.Euler(maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }
            else if (camTrans.rotation.eulerAngles.x > 180f && camTrans.rotation.eulerAngles.x < 360f - maxViewAngle)
            {
                camTrans.rotation = Quaternion.Euler(-maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }

            muzzleFlash.SetActive(false);


            //Handle Shooting
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }

                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }
                FireShot();
            }

            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }


            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);

            }
            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }


            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;
            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
            activeGun.fireCounter = activeGun.fireRate;
            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            muzzleFlash.SetActive(true);
        }
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);
        currentGun++;
        if (currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }
        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;


        firePoint.position = activeGun.firepoint.position;

    }
}
