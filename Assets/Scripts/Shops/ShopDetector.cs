using UnityEngine;
using UnityEngine.UI;

public class ShopDetector : MonoBehaviour
{
    public Transform ShootPoint;
    public float DetectRange = 2f;
    public Text ShopText;

    private void Start()
    {
        var inGameUITransform = GameObject.Find("/Canvas/InGame").transform;
        ShopText = inGameUITransform.Find("ShopText").GetComponent<Text>();
    }
    
    private void Update()
    {
        if (Physics.Raycast(ShootPoint.position, ShootPoint.forward, out var hit, DetectRange))
        {
            var shopBase = hit.transform.GetComponent<ShopBase>();

            ShopText.text = shopBase is null ? string.Empty : shopBase.ShopText;
        }
    }
}
