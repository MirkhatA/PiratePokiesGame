using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Roulette : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    
    public float StopPower;
    
    [SerializeField] private Button RouletteButton;
    [SerializeField] private GridManager gridManager;
    
    private Rigidbody2D rbody;
    private int inRotate;

    private float t;
    
    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        RouletteButton.enabled = true;
    }

    private void Update()
    {
        if (rbody.angularVelocity > 0)
        {
            rbody.angularVelocity -= StopPower * Time.deltaTime;
            rbody.angularVelocity = Mathf.Clamp(rbody.angularVelocity, 0, 1440);
        }

        if (rbody.angularVelocity == 0 && inRotate == 1)
        {
            t += 1 * Time.deltaTime;
            if (t >= 0.5f)
            {
                GetReward();
                inRotate = 0;
                t = 0;
            }
        }
    }

    public void Rotate()
    {
        if (inRotate == 0)
        {
            rbody.AddTorque(Random.Range(2, 7) * 100);
            inRotate = 1;
        }

        RouletteButton.enabled = false;
    }

    private void GetReward()
    {
        float rot = transform.eulerAngles.z;

        if (rot > 0+22 && rot <= 45+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,45);
            print("Bomb +2");
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveRedCanvas();
        }
        else if (rot > 45+22 && rot <= 90+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,90);
            print("Money +100");
            gridManager.UpdateCoin(100);
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveGreenCanvas();
        }
        else if (rot > 90+22 && rot <= 135+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,135);
            print("Bomb +1");
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveRedCanvas();
        }
        else if (rot > 135+22 && rot <= 180+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,180);
            print("health +2");
            gridManager.UpdateLifes(2);
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveGreenCanvas();
        }
        else if (rot > 180+22 && rot <= 225+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,225);
            print("Money +300");
            gridManager.UpdateCoin(300);
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveGreenCanvas();
        }
        else if (rot > 225+22 && rot <= 270+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,270);
            print("health + 1");
            gridManager.UpdateLifes(1);
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveGreenCanvas();
        }
        else if (rot > 270+22 && rot <= 315+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,315);
            print("money +200");
            gridManager.UpdateCoin(200);
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveGreenCanvas();
        }
        else if (rot > 315+22 && rot <= 360+22)
        {
            GetComponent<RectTransform>().eulerAngles = new Vector3(0,0,0);
            print("Bomb + 1");
            StartCoroutine(DisablePanelWithDelay(1.5f));
            gridManager.ActiveRedCanvas();
        }
    }
    
    IEnumerator DisablePanelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.DisableRoulettePanel();
        SceneManager.LoadScene("GameScene");
    }
}
