using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoffeeMachineTwo : MonoBehaviour
{
    public Slider targetSlider;
    [SerializeField] private float targetSliderMoveSpeed;

    [SerializeField] private float targetSliderIdleTime = 3f;

    public float targetSliderRandomFloat;
    public bool targetSliderMoving;


    // Start is called before the first frame update
    void Start()
    {
        targetSliderMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (targetSliderMoving)
        {
            StartCoroutine(MoveTargetSlider());
        }
        else
        {
            IdleTime();
        }
        
    }

    

    private void IdleTime()
    {
        if (targetSliderIdleTime > 0 && !targetSliderMoving)
        {
            targetSliderIdleTime -= Time.deltaTime; //counting down idle time
        }
        else
        {
            targetSliderMoving = true;
        }
    }

    private IEnumerator MoveTargetSlider()
    {
        targetSliderRandomFloat = Mathf.Round(Random.Range(0.3f, 0.8f) * 10f) / 10f; //round to one decimal place
        Debug.Log(targetSliderRandomFloat);
        yield return null;

        while(Mathf.Abs(targetSlider.value + targetSliderRandomFloat) > 0.000005f)
        {
            Debug.Log("targetSlider value is not equal to randomFloatTarget");

            if (targetSlider.value > targetSliderRandomFloat)
            {
                targetSlider.value -= targetSliderMoveSpeed * Time.deltaTime;
                Debug.Log($"target slider value is greater than randomFloat so Decreases value of target slider");
            }
            else
            {
                targetSlider.value += targetSliderMoveSpeed * Time.deltaTime;
                Debug.Log($"target slider value is greater than randomFloat so Increases value of target slider");
            }
            yield return null;
        }

        targetSlider.value = targetSliderRandomFloat;
        targetSliderMoving = false;
        targetSliderIdleTime = 2f;
    }
}
