using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGenerator : MonoBehaviour {

    [SerializeField] private GameObject m_slime;
    [SerializeField] private float m_spawnTime;
    private float m_time = 5f;
    private IEnumerator m_coroutine;
    [SerializeField] private int m_maxNumOfSlime = 3;
    public int CurrentNumOfSlime = 0;


    // Use this for initialization
    void Start () {


        //m_coroutine = WaitAndPrint(m_time);
        m_coroutine = SlimeGenerateCoroutine(m_time);
        StartCoroutine(m_coroutine);
    }

    void SpawnSlime()
    {
        GameObject[] currentSlimes = GameObject.FindGameObjectsWithTag("Slime");
        print(currentSlimes.Length);
        if(currentSlimes.Length <= m_maxNumOfSlime)
        {
            GameObject slime = Instantiate(this.m_slime, transform.position, transform.rotation);
            slime.GetComponent<SlimeMovement>().Target = GameObject.FindGameObjectWithTag("Player");
            CurrentNumOfSlime++;
        }
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            print("WaitAndPrint " + Time.time);
        }
    }

    private IEnumerator SlimeGenerateCoroutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SpawnSlime();
        }
    }

}
