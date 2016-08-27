namespace OwinDocker.Tests
{
    public class SomeModel
    {
        public SomeModel(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { private set; get; }
        public int Value { private set; get; }
    }
}