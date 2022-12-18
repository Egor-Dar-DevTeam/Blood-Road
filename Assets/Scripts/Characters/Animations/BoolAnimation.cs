namespace Characters.Animations
{
    public class BoolAnimation : IAnimate
    {
        private string _parameterName;
        private bool _value;
        public BoolAnimation(string parameterName, bool value)
        {
            _parameterName = parameterName;
            _value = value;
        }
        public void Apply(AnimatorController script)
        {
            script.Bool(_parameterName,_value);
        }
    }
}