using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    [Header("Movement")]
    public float maxPower = 10f;
    public float gravity = 1f;
    [Range(0f, 0.1f)] public float slowMotion = 0.02f;

    public bool shootWhileMoving = false;
    public bool forwardDraging = true;
    public bool showLineOnScreen = false;
    public bool freeAim = true;

    [Header("Managers")]
    public CoinManager cm;
    public GemManager gm;
    public PotionManager ps;

    [Header("Health")]
    public int maxHealth = 100;
    public int shootDamage = 5;
    public int currentHealth;
    public HealthBar healthBar;

    Transform direction;
    Rigidbody2D rb;
    LineRenderer line;
    LineRenderer screenLine;

    Vector2 startMousePos;
    Vector2 currentMousePos;

    float shootPower;
    bool canShoot = true;
    bool isAiming = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;

        line = GetComponent<LineRenderer>();
        direction = transform.GetChild(0);
        screenLine = direction.GetComponent<LineRenderer>();

        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
        else
            Debug.LogError("HealthBar not assigned in Inspector!");
    }

    void Update()
    {
        // Disable input if health is zero
        if (currentHealth <= 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (freeAim)
                MouseClick();
            else
                BallClick();
        }

        if (Input.GetMouseButton(0) && isAiming)
        {
            MouseDrag();

            if (shootWhileMoving)
                rb.velocity /= (1 + slowMotion);
        }

        if (Input.GetMouseButtonUp(0) && isAiming)
        {
            MouseRelease();
        }

        if (!shootWhileMoving && rb.velocity.magnitude < 0.7f)
        {
            rb.velocity = Vector2.zero;
            canShoot = true;
        }
    }

    // ---------------- INPUT ----------------

    void MouseClick()
    {
        if (!canShoot && !shootWhileMoving) return;

        isAiming = true;
        startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LookAtShootDirection();
    }

    void BallClick()
    {
        if (!objectClicked()) return;
        if (!canShoot && !shootWhileMoving) return;

        isAiming = true;
        startMousePos = transform.position;
        LookAtShootDirection();
    }

    void MouseDrag()
    {
        LookAtShootDirection();
        DrawLine();

        if (showLineOnScreen)
            DrawScreenLine();
    }

    void MouseRelease()
    {
        if (canShoot || shootWhileMoving)
            Shoot();

        isAiming = false;
        line.enabled = false;
        screenLine.enabled = false;
    }

    // ---------------- ACTIONS ----------------

    void LookAtShootDirection()
    {
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = startMousePos - currentMousePos;
        transform.right = forwardDraging ? -dir : dir;

        float dis = Vector2.Distance(startMousePos, currentMousePos) * 4f;
        shootPower = Mathf.Min(dis, maxPower);

        direction.localPosition = new Vector2(shootPower / 6f, 0);
    }

    void Shoot()
    {
        canShoot = false;
        rb.velocity = transform.right * shootPower;

        TakeDamage(shootDamage);
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            DisableBall();
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth > 0)
        {
            canShoot = true;
            isAiming = false;
        }
    }

    void DisableBall()
    {
        canShoot = false;
        isAiming = false;

        line.enabled = false;
        screenLine.enabled = false;
        // Rigidbody untouched → gravity continues
    }

    // ---------------- LINE RENDERERS ----------------

    void DrawLine()
    {
        line.enabled = true;
        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, direction.position);
    }

    void DrawScreenLine()
    {
        screenLine.enabled = true;
        screenLine.positionCount = 2;
        screenLine.SetPosition(0, startMousePos);
        screenLine.SetPosition(1, currentMousePos);
    }

    // ---------------- COLLISIONS ----------------

    bool objectClicked()
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition),
            0.2f,
            Vector2.zero
        );

        return hit.collider != null && hit.collider.gameObject == gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
        else if (other.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            gm.gemCount++;
        }
        else if (other.CompareTag("Potion"))
        {
            int healAmount = Mathf.RoundToInt(maxHealth * 0.2f);
            Heal(healAmount);

            Destroy(other.gameObject);
        }
    }
}
