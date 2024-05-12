using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    // angle shit
    public float jumpAngle = 0f;
    public float fallAngle = 0f;

    // gravity shit
    public float fallGrav = 0f; // increase ~ per second
    public float jumpGrav = 0f;

    public bool fuckingDead = false;
    public PipeUltimatePerson25 pipePerson;

    // falling shit (private)
    private bool goToFall = false;
    private float currentGrav = 0f; // current fella
    private bool stopGravAndRot = false;
    // timers
    private float targetTimer = 0f;
    private float targetTweenTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void setTimers(float targetTim = 0.0f, float targetTweenTim = 0.0f)
    {
        targetTimer = targetTim;
        targetTweenTimer = targetTweenTim;
    }

    // Update is called once per frame
    void Update()
    { 
        // extracted these bitches to functions because uh, looks cleaner? i mean it does in all honesty
        handleInput();
        tickTimers();
        if (!stopGravAndRot)
            handleRotationAndGravity();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        tagCol(col.gameObject.tag);
    }

    void tagCol(string tag)
    {
        switch (tag)
        {
            case "Ground":
                stopGravAndRot = true;
                fuckingDead = true;
                pipePerson.fuckingDead = true;

                Vector3 newPosition = gameObject.transform.position;
                newPosition.y = -11.95f;
                gameObject.transform.position = newPosition;

                SceneManager.LoadScene("Test");
                break;

            case "Pipe":
                fuckingDead = true;
                pipePerson.fuckingDead = true;
                break;
        }
    }

    #region Functions for cleanness (?)

    void handleInput()
    {
        if (Input.GetMouseButtonDown(0) && fuckingDead != true)
        {
            // Start where we were falling
            if (goToFall)
                targetTweenTimer = 1 - targetTweenTimer;
            goToFall = false;
            targetTimer = 0.3f;
            currentGrav = jumpGrav;
        }
    }

    void tickTimers()
    {
        // target 
        if (!goToFall)
        {
            if (targetTimer > 0.0f)
                targetTimer -= Time.deltaTime;
            else
            {
                targetTweenTimer = 0f;
                goToFall = true;
            }
        }
        targetTweenTimer = Mathf.Min(1f / (goToFall ? 1.3f : 5f), targetTweenTimer + Time.deltaTime);
    }

    void handleRotationAndGravity()
    {
        // tween, tween, the magical tween
        float diff = jumpAngle - fallAngle;
        float rot;
        float timeMoneyMoney = goToFall ? 1.3f : 5f;

        if (goToFall)
            rot = -(diff * expoIn(targetTweenTimer * timeMoneyMoney)) + jumpAngle; // -(110 * ExpoInTweenShit) + 20 = -90  which uh just look at lower comment
        else 
            rot = diff * expoOut(targetTweenTimer * timeMoneyMoney) + fallAngle; // fall angle should be a negative so it does 110 * ExpoOutTweenShit - 90 = 20  which is the angles i have chosen for now

        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot);  

        currentGrav += (Mathf.Min(currentGrav, 0f) + fallGrav) * Time.deltaTime;
        if (fuckingDead)
            currentGrav = Mathf.Max(currentGrav, -25f);

        Vector3 newPosition = gameObject.transform.position;
        newPosition.y = Mathf.Clamp(newPosition.y + currentGrav * Time.deltaTime, -20f, 20f);
        gameObject.transform.position = newPosition;
    }

    #endregion

    #region Tweenings

    // https://easings.net
    float expoIn(float t) 
    {
        return t == 0f ? 0f : Mathf.Pow(2, 10f * t - 10f);
    }
    float expoOut(float t)
    {
        return t == 1f ? 1f : 1 - Mathf.Pow(2, -10f * t);
    }

    #endregion
}
