namespace Characters.Animations
{
    public class SetFloatAnimation : IAnimate
    {
        private string _parameterName;
        private float _value;
        public SetFloatAnimation(string parameterName, float value)
        {
            _parameterName = parameterName;
            _value = value;
        }
        public void Apply(AnimatorController script)
        {
            script.Float(_parameterName, _value);
        }
    }
}