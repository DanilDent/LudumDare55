using UnityEngine;

namespace Game
{
    // This script prevents agents with movement component moving through each 
    // other and resets agent target position to the neareset free cell
    public class LocalAvoidance : MonoBehaviour
    {
        [SerializeField] private MovementComp _movementComp;
        [SerializeField] private CapsuleCollider2D _capsuleCollider;
        [SerializeField] private float _avoidanceDistance = 0.5f;
        [SerializeField] private float _raycastRate = 0.5f;


        [SerializeField] private int _resultsLength;

        private Vector2 _position2D => new Vector2(transform.position.x, transform.position.y);
        private float _lastRaycast = float.NegativeInfinity;

        public void Construct()
        {
            _movementComp = GetComponent<MovementComp>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            //if (Time.time > _lastRaycast + _raycastRate)
            //{
            //    _lastRaycast = Time.time;

            //    RaycastHit2D[] raycastHits = new RaycastHit2D[10];
            //    _resultsLength = _capsuleCollider.Cast(_movementComp.NextMoveDirection2D, raycastHits, _avoidanceDistance);
            //    if (_resultsLength > 0)
            //    {
            //        _movementComp.canMove = false;
            //    }
            //    else
            //    {
            //        _movementComp.canMove = true;
            //    }
            //}
        }
    }
}
