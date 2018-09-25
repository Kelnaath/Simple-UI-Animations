namespace Maikel.UIAnimations
{
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvasGroup : BaseAnimation<CanvasGroup>
    {
        protected override void AnimationStep()
        {
            animationComponent.alpha = _newvalue;
        }
    }
}


