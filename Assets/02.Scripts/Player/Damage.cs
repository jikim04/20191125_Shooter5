using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour {
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float initHp = 100.0f;
    public float currHp;
    public Image bloodScreen;
    public Image hpBar;
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currColor;
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    void Start() {
        currHp = initHp;
        hpBar.color = initColor;
        currColor = initColor;
    }

    void OnTriggerEnter(Collider coll)
    { if (coll.tag == bulletTag)
        {
            Destroy(coll.gameObject);
            StartCoroutine(ShowBloodScreen());
            currHp -= 5.0f;
            Debug.Log("player Hp =" + currHp.ToString());
            DisplayHpbar();

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }

    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;

    }

    void PlayerDie()
    {
        OnPlayerDie();
        GameManager.instance.isGameOver = true;
        Debug.Log("Player Die !");
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        //for (int i = 0; i < enemies.Length; i++)
        //{
            //enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }
    void DisplayHpbar()
    {  if ((currHp / initHp) > 0.5f)
            currColor.r = (1 - (currHp / initHp)) * 2.0f;
        else
            currColor.g = (currHp / initHp) * 2.0f;

        hpBar.color = currColor;
        hpBar.fillAmount = (currHp / initHp);
    }


}
