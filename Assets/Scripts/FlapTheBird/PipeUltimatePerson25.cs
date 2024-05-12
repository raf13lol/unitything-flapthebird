using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PipeUltimatePerson25 : MonoBehaviour
{
    public List<GameObject> pipes = new List<GameObject>();

    public float spaceBetweenPipes;
    public GameObject pipe;
    public float distance;
    public float deltaDistance;
    public float speed;

    public TextMeshProUGUI scoreText;
    public int score = -3;

    public bool fuckingDead = false;

    // Start is called before the first frame update
    void Start()
    {
        deltaDistance = spaceBetweenPipes;
    }
    
    void createPipes()
    {
        GameObject obj = GameObject.Instantiate(pipe, new Vector3(pipe.transform.position.x, Random.Range(-6.5f, 6.5f), 0f), Quaternion.Euler(0f, 0f, 0f));
        obj.transform.parent = gameObject.transform;
        pipes.Add(obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (fuckingDead)
            return;

        float distanceCovered = speed * Time.deltaTime;
        distance += distanceCovered;
        deltaDistance += distanceCovered;

        while (deltaDistance >= spaceBetweenPipes)
        {
            deltaDistance -= spaceBetweenPipes;
            createPipes();
            score++;
            scoreText.text = $"Score: {Mathf.Max(score, 0)}";
            break;
        }

        List<GameObject> shitToRemove = new List<GameObject>();

        // Now update positions
        foreach (GameObject obj in pipes)
        {
            Vector3 pos = obj.transform.position;
            pos.x -= distanceCovered;
            obj.transform.position = pos;
            if (pos.x < -30)
            {
                GameObject.Destroy(obj);
                shitToRemove.Add(obj); // have to do this outside of the foreach, what a shame
            }
        }   
        // remove stuff
        foreach (GameObject nightnight in shitToRemove)
        {
            pipes.Remove(nightnight);
        }
        shitToRemove.Clear(); // clear so hopefully the gc plays nice
    }
}
