namespace DotNetDesignPatternDemos.Creational.Builder
{
    public class Person
    {
        public string Name, Position;

        public override string? ToString()
        {
            return $"Name is {Name} and position is {Position}";
        }
    }

    public sealed class PersonBuilder
    {
        public readonly List<Func<Person, Person>> _actions
          = new List<Func<Person, Person>>();

        public PersonBuilder Called(string name)
            => Do(p => p.Name = name);

        public Person Build()
            => _actions.Aggregate(new Person(), (p, f) => f(p));

        public PersonBuilder Do(Action<Person> action)
            => AddAction(action);

        private PersonBuilder AddAction(Action<Person> action)
        {
            _actions.Add(p =>
            {
                action(p);
                return p;
            });
            return this;
        }
    }

    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA
          (this PersonBuilder builder, string position)
            => builder.Do(p => p.Position = position);
    }

    public class FunctionalBuilder
    {
        public static void Main(string[] args)
        {
            var pb = new PersonBuilder()
                .Called("Dmitri")
                .WorksAsA("Programmer")
                .Build();

            Console.WriteLine(pb.ToString());
        }
    }
}