using UnityEngine;

public class RewardBlock : MonoBehaviour
{
    private bool _isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected == false)
        {
            if (other.TryGetComponent(out BagForRewardBlock bagForReward) && bagForReward.CurrentBlocksCount < bagForReward.BagCapacity)
            {
                bagForReward.AddBlockInBag(transform);
                _isCollected = true;
            }
        }
    }
}
