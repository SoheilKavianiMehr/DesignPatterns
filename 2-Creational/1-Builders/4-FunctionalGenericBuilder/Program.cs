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

    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSubject : new()
        where TSelf : FunctionalBuilder<TSubject, TSelf>
    {
        public readonly List<Func<TSubject, TSubject>> _actions
          = new List<Func<TSubject, TSubject>>();

        public TSubject Build()
            => _actions.Aggregate(new TSubject(), (p, f) => f(p));

        public TSelf Do(Action<TSubject> action)
            => AddAction(action);

        private TSelf AddAction(Action<TSubject> action)
        {
            _actions.Add(p =>
            {
                action(p);
                return p;
            });
            return (TSelf)this;
        }
    }

    public sealed class PersonBuilder : FunctionalBuilder<Person, PersonBuilder>
    {
        public PersonBuilder Called(string name)
            => Do(p => p.Name = name);
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