using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text damageText;
    public float lifeTime = 1f;
    public float textSpeed = 1f;
    public float offset = .5f;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime);
        transform.position += new Vector3(0, textSpeed * Time.deltaTime, 0);
    }

    public void SetText(int amount)
    {
        damageText.text = amount.ToString();
        transform.position += new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0); 
    }
}
