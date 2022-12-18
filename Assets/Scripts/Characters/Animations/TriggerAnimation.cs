namespace Characters.Animations
{
    public class TriggerAnimation : IAnimate
    {
        private string _parameterName;
        public TriggerAnimation(string parameterName)
        {
            _parameterName = parameterName;
        }
        public void Apply(AnimatorController script)
        {
            script.Trigger(_parameterName);
        }
    }
}