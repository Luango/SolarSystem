using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {
    public Transform SpawnPoint;
    public Transform SpawnPoint2;
    public GameObject Goal;
    public GameObject Win;
    public GameObject RestartButton;
    
    private float StringMaxLength;
    private float StringIdleLength = 2.0f;
    private float MaxDeltaDis = 3f;

    private bool isStringOut = false;
    private LineRenderer lineRenderer;
    private bool isDraging = false;
    private Vector3 hitPos;

    private Vector3 preMousePos;
    private Vector3 curMousePos;
    private bool hasSavedPrincess = false;

    private static PlayerControl instance = null;
    public static PlayerControl Instance
    {
        get
        {
            return Instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        StringMaxLength = StringIdleLength + MaxDeltaDis;
    }

    // Use this for initialization
    void Start ()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
    }

    private IEnumerator DismissLine()
    {
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
        isDraging = false;
    }

    private void DragForce(Vector3 start, Vector3 end)
    {
        if (isDraging)
        {
            // Hook's law.
            // F = -kX
            Vector3 dir = (end - start).normalized;
            float dis = Vector3.Distance(start, end);
            float x = StringIdleLength - dis;
            transform.GetComponent<Rigidbody2D>().AddForce(dir * -x * 30f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "KillWall")
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            transform.rotation = Quaternion.identity;
            Time.timeScale = 0f;
            isDraging = false;
            hasSavedPrincess = false;

            RestartButton.gameObject.SetActive(true);
        }
        if(collision.tag == "Princess" && !hasSavedPrincess)
        {
            hasSavedPrincess = true;
            collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            collision.gameObject.transform.parent = this.transform;
            print("saved princess");
            Goal.SetActive(true);
        }
        if (collision.tag == "Goal" && hasSavedPrincess)
        {
            Win.SetActive(true);
            Time.timeScale = 0f;
            if (GameManager.BestPerformance > GameManager.ClickCount)
            {
                GameManager.BestPerformance = GameManager.ClickCount;
            }
            GameManager.UpdateBestCount();
            print("win");

            RestartButton.gameObject.SetActive(true);
        }
    }

    private void SwingForce()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            GameManager.UpdateClickCount();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            Vector3 dir = (mousePos - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir);
             
            if (hit.collider != null && Vector3.Distance(hit.transform.position, transform.position) < StringMaxLength)
            {
                hitPos = hit.transform.position;
                if (Vector3.Distance(transform.position, hit.transform.position) < StringMaxLength)
                {
                    //Drag something
                    isDraging = true;
                }
            }
            else
            {
                // Render the line then dismiss the line.

                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                }
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + dir * (StringMaxLength-0.3f));
                StartCoroutine(DismissLine());
                isDraging = false;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            lineRenderer.enabled = false;
        }

        if (isDraging)
        {
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitPos);
        }
    }

    private void FixedUpdate()
    { 
        DragForce(transform.position, hitPos);
    }

    public void Reset()
    {
        transform.position = SpawnPoint.position;
        hasSavedPrincess = false;
        Time.timeScale = 1f;
    }
}
