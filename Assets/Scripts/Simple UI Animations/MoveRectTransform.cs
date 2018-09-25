namespace Maikel.UIAnimations
{
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class MoveRectTransform : BaseAnimation<RectTransform>
    {
        [SerializeField]
        Vector3 _distance;
        Vector3 _basePosition;

        public override void StartAnimation()
        {
            _basePosition = animationComponent.localPosition;
            base.StartAnimation();
        }

        protected override void AnimationStep()
        {
            Vector3 newPos = (_distance * _newvalue) + _basePosition;
            animationComponent.localPosition = newPos;
        }
    }
}

