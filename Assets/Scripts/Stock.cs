using UnityEngine;
using UnityEngine.Events;

public class Stock : MonoBehaviour
{
    [SerializeField] private Transform _stockPlace;
    public static UnityEvent HandOverBlock = new UnityEvent();
    public Transform StockPlace => _stockPlace;

    public void SaleBlock()
    {
        HandOverBlock.Invoke();
    }
}
