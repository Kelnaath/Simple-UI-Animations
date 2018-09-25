namespace Maikel.UIAnimations
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class BaseAnimation<T> : MonoBehaviour
    {
        [Header("Animation Parameters")]
        [SerializeField]
        bool _autoStart;
        [SerializeField]
        AnimationCurve _animationCurve;
        [SerializeField]
        WrapMode animationType;                                     //Added as an extra property for convenience.
        [SerializeField]
        float _startDelay;
        [SerializeField]
        protected float _duration;
        [SerializeField]
        List<AnimationTrigger> _triggers;

        protected T animationComponent;
        protected float _time;
        protected float _newvalue;
        bool _canAnimate;

        // Use this for initialization
        void Start()
        {
            animationComponent = GetComponent<T>();
            SetupWrapMode();
            _time -= _startDelay; //This only works for a clamped curve, FIX!!!! >:(


            if (_autoStart)
                StartAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            if (_duration == 0f)
                return;

            if (_canAnimate)
            {
                Animate();
                CheckEndOfAnimation();
            }              
        }

        void SetupWrapMode()
        {
            if (animationType == WrapMode.Default)
                animationType = WrapMode.Clamp;

            _animationCurve.preWrapMode = animationType;
            _animationCurve.postWrapMode = animationType;
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public virtual void StartAnimation() { _canAnimate = true; }

        protected virtual void Animate()
        {
            _time += Time.deltaTime;
            CheckTriggers();
            _newvalue = EvaluateCurve();

            AnimationStep();
        }

        /// <summary>
        /// Use this if you want to manually control the animation step
        /// </summary>
        /// <param name="timestep">A value between 0 and 1 representing a point in the animation curve.</param>
        public virtual void ManualAnimationStep(float timestep)
        {
            timestep = Mathf.Clamp01(timestep);
            _newvalue = _animationCurve.Evaluate(timestep);

            AnimationStep();
        }

        protected virtual void AnimationStep()
        {
            //Override this and do animation stuff here.
        }

        /// <summary>
        /// Gets called when the animation is finished.
        /// </summary>
        protected virtual void AnimationComplete()
        {
            Reset();
        }

        /// <summary>
        /// Checks if the current animation has endend, depending on the wrap mode.
        /// </summary>
        private void CheckEndOfAnimation()
        {
            if (animationType == WrapMode.Loop || animationType == WrapMode.PingPong)
                return;

            if (_time / _duration > 1f)
                AnimationComplete();
        }

        /// <summary>
        /// Resets the properties used for animating the component.
        /// </summary>
        protected virtual void Reset()
        {
            _canAnimate = false;
            _time = 0f;
        }

        /// <summary>
        /// Gives a value from the animation curve based on the current time of the animation.
        /// </summary>
        /// <returns>The value of the animation curve based on time.</returns>
        float EvaluateCurve()
        {
            return _animationCurve.Evaluate(_time / _duration);
        }

        /// <summary>
        /// Checks if any trigger events need to be fired.
        /// TODO: This can probably be made way more efficient and precise!
        /// </summary>
        void CheckTriggers()
        {
            if (_triggers == null || _triggers.Count == 0)
                return;

            foreach(AnimationTrigger t in _triggers)
            {
                //if (!t.played && Mathf.Approximately((float)Math.Round(_time / _duration, 2), t.triggerPoint))
                if(!t.played && _time / _duration >= t.triggerPoint)
                    t.FireEvent();
            }
        }
    }

    [Serializable]
    public class AnimationTrigger
    {
        [Tooltip("The number represents a point in the animation curve. So 0 is at the start and 1 is at the end of the curve.")]
        [Range(0f,1f)]
        public float triggerPoint;
        public UnityEvent triggerEvent;

        [HideInInspector]
        public bool played;

        public void FireEvent()
        {
            triggerEvent.Invoke();
            played = true;
        }
    }
}

