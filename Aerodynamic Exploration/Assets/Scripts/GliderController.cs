using System.Collections;
using UnityEngine;
using FlyingSystem;
using UnityEngine.SceneManagement;

public class GliderController : MonoBehaviour
{
    private GliderFlyingSystem gliderFlyingSystem;

    private Airflow airflow;

    public bool activated;

    public bool autoTakeOff;

    public float reloadSeconds = 3f;
    
    public float autoTakeOffSpeed = 10.0f;
    
    void Start()
    {
        gliderFlyingSystem = this.GetComponent<GliderFlyingSystem>();

        if (autoTakeOff)
            gliderFlyingSystem.TakeOff(autoTakeOffSpeed);
        
        if (activated)
            Activate();
    }

    void Update()
    {
        if (activated)
        {
            HandleInput();
        }
    }
    
    private void Activate()
    {
        activated = true;
    }
    
    

    public void Deactivate()
    {
        activated = false;
    }

    void HandleInput()
    {
        gliderFlyingSystem.AddPitchInput(Input.GetAxis("Pitch"));
        gliderFlyingSystem.AddRollInput(Input.GetAxis("Roll"));
        gliderFlyingSystem.AddYawInput(Input.GetAxis("Yaw"));
        if (Input.GetAxis("Yaw") == 0f) gliderFlyingSystem.StopYawInput();
        if (Input.GetKey(KeyCode.Return)) StartCoroutine(ReloadScene(0f));
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }
    

    public void MobileTurnLeft()
    {
        gliderFlyingSystem.AddYawInput(-1.0f);
    }

    public void MobileTurnRight()
    {
        gliderFlyingSystem.AddYawInput(1.0f);
    }

    public void MobileReleaseTurnLeft()
    {
        gliderFlyingSystem.StopYawInput();
    }

    public void MobileReleaseTurnRight()
    {
        gliderFlyingSystem.StopYawInput();
    }

    public void MobilePointUp()
    {
        gliderFlyingSystem.AddPitchInput(-1.0f);
    }

    public void MobilePointDown()
    {
        gliderFlyingSystem.AddPitchInput(1.0f);
    }

    public void MobileRollLeft()
    {
        gliderFlyingSystem.AddRollInput(-1.0f);
    }

    public void MobileRollRight()
    {
        gliderFlyingSystem.AddRollInput(1.0f);
    }

    public void TakeOff(float takeOffSpeed)
    {
        gliderFlyingSystem.TakeOff(takeOffSpeed);
    }

    public float GetFlyingSpeed()
    {
        return gliderFlyingSystem.flyingSpeed;
    }

    public float GetWeightPercentage()
    {
        return gliderFlyingSystem.weightPercentage;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (gliderFlyingSystem.inAir)
            {
                gliderFlyingSystem.Land();
                StartCoroutine(ReloadScene(reloadSeconds));
            }
        }
    }

    private IEnumerator ReloadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Airflow")
        {
            airflow = other.GetComponent<Airflow>();
            gliderFlyingSystem.AddAirflowForce(airflow.intensity, airflow.acceleration, airflow.fadeOutAcceleration);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Airflow")
            gliderFlyingSystem.EndAirflowForce();
    }
}