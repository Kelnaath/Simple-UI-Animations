namespace Maikel.UIAnimations
{
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class ScaleRectTransform : BaseAnimation<RectTransform>
    {

        protected override void AnimationStep()
        {
            animationComponent.localScale = new Vector3(_newvalue, _newvalue);
        }
    }
}

