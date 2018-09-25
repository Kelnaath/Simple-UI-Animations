namespace Maikel.UIAnimations
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class FadeImage : BaseAnimation<Image>
        {

        Color newAlpha;

        protected override void AnimationStep()
        {
            newAlpha = animationComponent.color;
            newAlpha.a = _newvalue;
            animationComponent.color = newAlpha;
        }
    }
}
