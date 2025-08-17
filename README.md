# Design Patterns in C#

A comprehensive collection of design patterns implemented in C# with clear examples and explanations. This repository serves as a learning resource and reference guide for software developers looking to understand and implement design patterns in their projects.

## 📚 Table of Contents

- [SOLID Design Principles](#️-solid-design-principles)
  - [Single Responsibility Principle](#1-single-responsibility-principle-srp)
  - [Open-Closed Principle](#2-open-closed-principle-ocp)
  - [Liskov Substitution Principle](#3-liskov-substitution-principle-lsp)
  - [Interface Segregation Principle](#4-interface-segregation-principle-isp)
  - [Dependency Inversion Principle](#5-dependency-inversion-principle-dip)
- [Creational Patterns - Builder](#️-creational-patterns---builder)
  - [Basic Builder](#1-basic-builder)
  - [Builder Inheritance](#2-builder-inheritance)
  - [Stepwise Builder](#3-stepwise-builder)
  - [Functional Builder](#4-functional-builder)
  - [Functional Generic Builder](#5-functional-generic-builder)
  - [Faceted Builder](#6-faceted-builder)
- [Creational Patterns - Factories](#️-creational-patterns---factories)
  - [Factory Method](#1-factory-method)
  - [Asynchronous Factory](#2-asynchronous-factory)
  - [Bulk Replacement Factory](#3-bulk-replacement-factory)
  - [Abstract Factory](#4-abstract-factory)
- [Creational Patterns - Prototypes](#️-creational-patterns---prototypes)
  - [ICloneable is Bad](#1-icloneable-is-bad)
  - [Copy Constructors](#2-copy-constructors)
  - [Inheritance](#3-inheritance)
  - [Copy Through Serialization](#4-copy-through-serialization)
- [Getting Started](#-getting-started)
- [Contributing](#-contributing)
- [License](#-license)

## 🏗️ SOLID Design Principles

The SOLID principles are five design principles intended to make software designs more understandable, flexible, and maintainable.

### 1. Single Responsibility Principle (SRP)

**Principle**: A class should have only one reason to change, meaning it should have only one job or responsibility.

**Implementation**: [`1-SOLID_Design_Principles/1-SingleResponsibility`](./1-SOLID_Design_Principles/1-SingleResponsibility)

**Example**:
```csharp
// ❌ Violates SRP - Journal class handles both journal entries AND persistence
public class Journal
{
    public void AddEntry(string text) { /* ... */ }
    public void Save(string filename) { /* ... */ } // This violates SRP!
}

// ✅ Follows SRP - Separate concerns
public class Journal
{
    public void AddEntry(string text) { /* ... */ }
}

public class Persistence
{
    public void SaveToFile(Journal journal, string filename) { /* ... */ }
}
```

**Key Benefits**:
- Easier to maintain and modify
- Reduced coupling between functionalities
- Better testability

### 2. Open-Closed Principle (OCP)

**Principle**: Software entities should be open for extension but closed for modification.

**Implementation**: [`1-SOLID_Design_Principles/2-Open-Closed`](./1-SOLID_Design_Principles/2-Open-Closed)

**Example**:
```csharp
// ❌ Violates OCP - Need to modify class for new filter criteria
public class ProductFilter
{
    public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color) { /* ... */ }
    public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size) { /* ... */ }
    // Adding new criteria requires modifying this class
}

// ✅ Follows OCP - Use specification pattern
public interface ISpecification<T>
{
    bool IsSatisfied(T item);
}

public class BetterFilter
{
    public IEnumerable<T> Filter<T>(IEnumerable<T> items, ISpecification<T> spec)
    {
        foreach (var item in items)
            if (spec.IsSatisfied(item))
                yield return item;
    }
}
```

**Key Benefits**:
- Extensible without modifying existing code
- Reduces risk of breaking existing functionality
- Promotes code reusability

### 3. Liskov Substitution Principle (LSP)

**Principle**: Objects of a superclass should be replaceable with objects of a subclass without breaking the application.

**Implementation**: [`1-SOLID_Design_Principles/3-LiskovSubstitution`](./1-SOLID_Design_Principles/3-LiskovSubstitution)

**Example**:
```csharp
// ❌ Violates LSP - Square changes behavior unexpectedly
public class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
}

public class Square : Rectangle
{
    public override int Width 
    { 
        set { base.Width = base.Height = value; } // Unexpected side effect!
    }
}

// This breaks when using Square as Rectangle
Rectangle rect = new Square();
rect.Width = 4;
rect.Height = 5; // Width becomes 5 too!
```

**Key Benefits**:
- Ensures behavioral consistency in inheritance hierarchies
- Maintains polymorphism integrity
- Prevents unexpected behavior in subclasses

### 4. Interface Segregation Principle (ISP)

**Principle**: Clients should not be forced to depend on interfaces they do not use.

**Implementation**: [`1-SOLID_Design_Principles/4-Interface Segregation`](./1-SOLID_Design_Principles/4-Interface%20Segregation)

**Example**:
```csharp
// ❌ Violates ISP - Forces implementation of unused methods
public interface IMachine
{
    void Print(Document d);
    void Fax(Document d);
    void Scan(Document d);
}

public class OldFashionedPrinter : IMachine
{
    public void Print(Document d) { /* ... */ }
    public void Fax(Document d) { throw new NotImplementedException(); } // Forced to implement!
    public void Scan(Document d) { throw new NotImplementedException(); } // Forced to implement!
}

// ✅ Follows ISP - Segregated interfaces
public interface IPrinter { void Print(Document d); }
public interface IScanner { void Scan(Document d); }
public interface IFax { void Fax(Document d); }

public class SimplePrinter : IPrinter
{
    public void Print(Document d) { /* ... */ }
}
```

**Key Benefits**:
- Reduces unnecessary dependencies
- Improves code flexibility and maintainability
- Enables better composition

### 5. Dependency Inversion Principle (DIP)

**Principle**: High-level modules should not depend on low-level modules. Both should depend on abstractions.

**Implementation**: [`1-SOLID_Design_Principles/5-DependencyInversion`](./1-SOLID_Design_Principles/5-DependencyInversion)

**Example**:
```csharp
// ❌ Violates DIP - High-level module depends on low-level module
public class Research
{
    public Research(Relationships relationships) // Direct dependency on concrete class
    {
        var relations = relationships.Relations; // Accessing internal implementation
        // ...
    }
}

// ✅ Follows DIP - Depend on abstraction
public interface IRelationshipBrowser
{
    IEnumerable<Person> FindAllChildrenOf(string name);
}

public class Research
{
    public Research(IRelationshipBrowser browser) // Depends on abstraction
    {
        foreach (var child in browser.FindAllChildrenOf("John"))
        {
            // ...
        }
    }
}
```

**Key Benefits**:
- Reduces coupling between modules
- Improves testability through dependency injection
- Enhances code flexibility and maintainability

## 🏭 Creational Patterns - Builder

The Builder pattern is used to construct complex objects step by step. It allows you to produce different types and representations of an object using the same construction code.

### 1. Basic Builder

**Purpose**: Provides a fluent interface for constructing complex objects.

**Implementation**: [`2-Creational.Builder/1-Builder`](./2-Creational.Builder/1-Builder)

**Example**:
```csharp
public class HtmlBuilder
{
    private HtmlElement root = new HtmlElement();
    
    public HtmlBuilder AddChildFluent(string childName, string childText)
    {
        var e = new HtmlElement(childName, childText);
        root.Elements.Add(e);
        return this; // Enables fluent interface
    }
}

// Usage
var builder = new HtmlBuilder("ul")
    .AddChildFluent("li", "Item 1")
    .AddChildFluent("li", "Item 2");
```

**Key Benefits**:
- Fluent, readable API
- Step-by-step object construction
- Separation of construction logic from representation

### 2. Builder Inheritance

**Purpose**: Demonstrates how to create builders that inherit from each other while maintaining fluency.

**Implementation**: [`2-Creational.Builder/2-BuilderInheritance`](./2-Creational.Builder/2-BuilderInheritance)

**Example**:
```csharp
public class PersonInfoBuilder<SELF> : PersonBuilder
    where SELF : PersonInfoBuilder<SELF>
{
    public SELF Called(string name)
    {
        person.Name = name;
        return (SELF)this;
    }
}

public class PersonJobBuilder<SELF> : PersonInfoBuilder<PersonJobBuilder<SELF>>
    where SELF : PersonJobBuilder<SELF>
{
    public SELF WorksAsA(string position)
    {
        person.Position = position;
        return (SELF)this;
    }
}

// Usage
var person = Person.New
    .Called("John")
    .WorksAsA("Developer")
    .Build();
```

**Key Benefits**:
- Maintains type safety in inheritance chains
- Enables complex builder hierarchies
- Preserves fluent interface across inheritance levels

### 3. Stepwise Builder

**Purpose**: Enforces a specific order of construction steps through interfaces.

**Implementation**: [`2-Creational.Builder/3-StepwiseBuilder`](./2-Creational.Builder/3-StepwiseBuilder)

**Example**:
```csharp
public interface ISpecifyCarType
{
    ISpecifyWheelSize OfType(CarType type);
}

public interface ISpecifyWheelSize
{
    IBuildCar WithWheels(int size);
}

public interface IBuildCar
{
    Car Build();
}

// Usage - enforced step order
var car = CarBuilder.Create()
    .OfType(CarType.Sedan)     // Must specify type first
    .WithWheels(16)            // Then wheel size
    .Build();                  // Finally build
```

**Key Benefits**:
- Enforces construction order at compile time
- Prevents invalid object states
- Clear, guided API

### 4. Functional Builder

**Purpose**: Uses functional programming concepts with actions/functions to build objects.

**Implementation**: [`2-Creational.Builder/4-FunctionalBuilder`](./2-Creational.Builder/4-FunctionalBuilder)

**Example**:
```csharp
public class PersonBuilder
{
    private readonly List<Func<Person, Person>> _actions = new();
    
    public PersonBuilder Called(string name) => Do(p => p.Name = name);
    
    public PersonBuilder Do(Action<Person> action)
    {
        _actions.Add(p => { action(p); return p; });
        return this;
    }
    
    public Person Build() => _actions.Aggregate(new Person(), (p, f) => f(p));
}

// Usage
var person = new PersonBuilder()
    .Called("John")
    .Do(p => p.Age = 30)
    .Build();
```

**Key Benefits**:
- Highly flexible and extensible
- Functional programming approach
- Easy to add new operations via extension methods

### 5. Functional Generic Builder

**Purpose**: Generic version of functional builder that can work with any type.

**Implementation**: [`2-Creational.Builder/4-FunctionalGenericBuilder`](./2-Creational.Builder/4-FunctionalGenericBuilder)

**Example**:
```csharp
public abstract class FunctionalBuilder<TSubject, TSelf>
    where TSubject : new()
    where TSelf : FunctionalBuilder<TSubject, TSelf>
{
    private readonly List<Func<TSubject, TSubject>> _actions = new();
    
    public TSubject Build() => _actions.Aggregate(new TSubject(), (p, f) => f(p));
    
    public TSelf Do(Action<TSubject> action) => AddAction(action);
}

public class PersonBuilder : FunctionalBuilder<Person, PersonBuilder>
{
    public PersonBuilder Called(string name) => Do(p => p.Name = name);
}
```

**Key Benefits**:
- Reusable across different types
- Type-safe generic implementation
- Combines flexibility with strong typing

### 6. Faceted Builder

**Purpose**: Allows building different aspects of an object using specialized sub-builders.

**Implementation**: [`2-Creational.Builder/5-FacetedBuilder`](./2-Creational.Builder/5-FacetedBuilder)

**Example**:
```csharp
public class PersonBuilder
{
    protected Person person = new Person();
    
    public PersonAddressBuilder Lives => new PersonAddressBuilder(person);
    public PersonJobBuilder Works => new PersonJobBuilder(person);
}

// Usage - different aspects
Person person = new PersonBuilder()
    .Lives.At("123 Main St").In("New York").WithPostcode("10001")
    .Works.At("Microsoft").AsA("Developer").Earning(100000);
```

**Key Benefits**:
- Separates concerns for different object aspects
- Maintains single object instance across builders
- Clean, organized API for complex objects

## 🏭 Creational Patterns - Factories

The Factory pattern provides an interface for creating objects without specifying their exact classes. It encapsulates object creation logic and promotes loose coupling between client code and concrete implementations.

### 1. Factory Method

**Purpose**: Provides static methods and properties for creating objects, avoiding constructor complexity.

**Implementation**: [`3-Creational.Factories/1-Factory`](./3-Creational.Factories/1-Factory)

**Example**:
```csharp
public class Point
{
    private double x, y;
    
    protected Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    
    // Factory methods
    public static Point NewCartesianPoint(double x, double y)
    {
        return new Point(x, y);
    }
    
    public static Point NewPolarPoint(double rho, double theta)
    {
        return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
    }
    
    // Factory property
    public static Point Origin => new Point(0, 0);
    
    // Nested factory class
    public static class Factory
    {
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }
    }
}

// Usage
var point1 = Point.NewCartesianPoint(2, 3);
var point2 = Point.NewPolarPoint(1, Math.PI/4);
var origin = Point.Origin;
var point3 = Point.Factory.NewCartesianPoint(1, 2);
```

**Key Benefits**:
- Clear, intention-revealing creation methods
- Avoids constructor parameter confusion
- Enables different creation strategies
- Supports object caching and reuse

### 2. Asynchronous Factory

**Purpose**: Handles object creation that requires asynchronous initialization.

**Implementation**: [`3-Creational.Factories/2-AsynchronousFactory`](./3-Creational.Factories/2-AsynchronousFactory)

**Example**:
```csharp
public class FooWithFactory
{
    private FooWithFactory()
    {
        // Private constructor
    }
    
    private async Task<FooWithFactory> InitAsync()
    {
        await Task.Delay(1000); // Simulate async initialization
        return this;
    }
    
    public static Task<FooWithFactory> CreateAsync()
    {
        var result = new FooWithFactory();
        return result.InitAsync();
    }
}

// Usage
FooWithFactory foo = await FooWithFactory.CreateAsync();
```

**Key Benefits**:
- Handles async initialization properly
- Prevents partially initialized objects
- Maintains clean API for async object creation
- Ensures proper resource initialization

### 3. Bulk Replacement Factory

**Purpose**: Allows bulk replacement of created objects, useful for themes, configurations, or global state changes.

**Implementation**: [`3-Creational.Factories/3-BulkReplacementFactory`](./3-Creational.Factories/3-BulkReplacementFactory)

**Example**:
```csharp
public class ReplaceableThemeFactory
{
    private readonly List<WeakReference<Ref<ITheme>>> themes = new();
    
    public Ref<ITheme> CreateTheme(bool dark)
    {
        var r = new Ref<ITheme>(createThemeImpl(dark));
        themes.Add(new(r));
        return r;
    }
    
    public void ReplaceTheme(bool dark)
    {
        foreach (var wr in themes)
        {
            if (wr.TryGetTarget(out var reference))
            {
                reference.Value = createThemeImpl(dark);
            }
        }
    }
    
    private ITheme createThemeImpl(bool dark)
    {
        return dark ? new DarkTheme() : new LightTheme();
    }
}

// Usage
var factory = new ReplaceableThemeFactory();
var theme = factory.CreateTheme(true);
Console.WriteLine(theme.Value.BgrColor); // "dark gray"
factory.ReplaceTheme(false); // Bulk replace all themes
Console.WriteLine(theme.Value.BgrColor); // "white"
```

**Key Benefits**:
- Enables bulk updates of created objects
- Useful for global theme/configuration changes
- Maintains object references while changing content
- Supports weak references to prevent memory leaks

### 4. Abstract Factory

**Purpose**: Provides an interface for creating families of related objects without specifying their concrete classes.

**Implementation**: [`3-Creational.Factories/4-AbstractFactory`](./3-Creational.Factories/4-AbstractFactory)

**Example**:
```csharp
public interface IHotDrinkFactory
{
    IHotDrink Prepare(int amount);
}

internal class TeaFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        Console.WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
        return new Tea();
    }
}

internal class CoffeeFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
        return new Coffee();
    }
}

public class HotDrinkMachine
{
    private List<Tuple<string, IHotDrinkFactory>> namedFactories = new();
    
    public HotDrinkMachine()
    {
        // Automatically discover and register factories
        foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
        {
            if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
            {
                namedFactories.Add(Tuple.Create(
                    t.Name.Replace("Factory", string.Empty), 
                    (IHotDrinkFactory)Activator.CreateInstance(t)));
            }
        }
    }
    
    public IHotDrink MakeDrink()
    {
        // Interactive selection and creation logic
        // ...
    }
}

// Usage
var machine = new HotDrinkMachine();
IHotDrink drink = machine.MakeDrink();
drink.Consume();
```

**Key Benefits**:
- Creates families of related objects
- Promotes consistency among products
- Supports easy addition of new product families
- Isolates concrete classes from client code
- Uses reflection for automatic factory discovery

## 🧬 Creational Patterns - Prototypes

The Prototype pattern allows you to create new objects by cloning existing instances, avoiding the overhead of creating objects from scratch. This pattern is particularly useful when object creation is expensive or when you need to create objects that are similar to existing ones.

### 1. ICloneable is Bad

**Purpose**: Demonstrates why the built-in `ICloneable` interface is problematic and should be avoided.

**Implementation**: [`2-Creational/3-Prototypes/1-ICloneableIsBad/`](2-Creational/3-Prototypes/1-ICloneableIsBad/)

**Example**:
```csharp
public class Person : ICloneable
{
    public readonly string[] Names;
    public readonly Address Address;

    public object Clone()
    {
        return new Person(Names, Address); // Shallow copy - problematic!
    }
}

// Usage
var john = new Person(new[] { "John", "Smith" }, new Address("London Road", 123));
var jane = (Person)john.Clone();
jane.Address.HouseNumber = 321; // Oops! John's address is also changed
```

**Key Issues**:
- Returns `object` instead of strongly-typed result
- Doesn't specify shallow vs deep copy behavior
- Can lead to unexpected shared references
- No compile-time type safety

### 2. Copy Constructors

**Purpose**: Shows how to implement proper deep copying using copy constructors.

**Implementation**: [`2-Creational/3-Prototypes/2-CopyConstructorsInsteadOfICloneable/`](2-Creational/3-Prototypes/2-CopyConstructorsInsteadOfICloneable/)

**Example**:
```csharp
public class Address
{
    public string StreetAddress, City, Country;

    public Address(Address other)
    {
        StreetAddress = other.StreetAddress;
        City = other.City;
        Country = other.Country;
    }
}

public class Employee
{
    public string Name;
    public Address Address;

    public Employee(Employee other)
    {
        Name = other.Name;
        Address = new Address(other.Address); // Deep copy
    }
}

// Usage
var john = new Employee("John", new Address("123 London Road", "London", "UK"));
var chris = new Employee(john); // Safe deep copy
```

**Key Benefits**:
- Type-safe copying
- Explicit deep copy behavior
- No shared references
- Clear constructor-based API

### 3. Inheritance

**Purpose**: Demonstrates how to implement deep copying with inheritance hierarchies using a custom interface.

**Implementation**: [`2-Creational/3-Prototypes/3-Inheritance/`](2-Creational/3-Prototypes/3-Inheritance/)

**Example**:
```csharp
public interface IDeepCopyable<T> where T : new()
{
    void CopyTo(T target);

    public T DeepCopy()
    {
        T t = new T();
        CopyTo(t);
        return t;
    }
}

public class Person : IDeepCopyable<Person>
{
    public string[] Names;
    public Address Address;

    public virtual void CopyTo(Person target)
    {
        target.Names = (string[])Names.Clone();
        target.Address = Address.DeepCopy();
    }
}

public class Employee : Person, IDeepCopyable<Employee>
{
    public int Salary;

    public void CopyTo(Employee target)
    {
        base.CopyTo(target);
        target.Salary = Salary;
    }
}

// Usage
var john = new Employee { Names = new[] { "John", "Doe" }, Salary = 321000 };
var copy = john.DeepCopy();
```

**Key Benefits**:
- Supports inheritance hierarchies
- Type-safe generic interface
- Extensible for derived classes
- Consistent deep copy behavior

### 4. Copy Through Serialization

**Purpose**: Shows how to implement deep copying using serialization mechanisms.

**Implementation**: [`2-Creational/3-Prototypes/4-CopyThroughSerialization/`](2-Creational/3-Prototypes/4-CopyThroughSerialization/)

**Example**:
```csharp
public static class ExtensionMethods
{
    public static T DeepCopyXml<T>(this T self)
    {
        using (var ms = new MemoryStream())
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(ms, self);
            ms.Position = 0;
            return (T)s.Deserialize(ms);
        }
    }
}

public class Foo
{
    public uint Stuff;
    public string Whatever;
}

// Usage
Foo foo = new Foo { Stuff = 42, Whatever = "abc" };
Foo foo2 = foo.DeepCopyXml(); // Complete deep copy via serialization
```

**Key Benefits**:
- Automatic deep copying of complex object graphs
- No manual implementation required
- Works with any serializable type
- Handles circular references (with proper serializer)

**Trade-offs**:
- Performance overhead from serialization
- Requires serializable types
- May not preserve object identity

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 or later
- Visual Studio 2022 or Visual Studio Code
- C# 12.0 or later

### Running the Examples

1. Clone the repository:
```bash
git clone https://github.com/yourusername/DesignPatterns.git
cd DesignPatterns
```

2. Open the solution in Visual Studio:
```bash
start DesignPatterns.sln
```

3. Build and run any specific pattern:
```bash
dotnet run --project "1-SOLID_Design_Principles/1-SingleResponsibility"
```

### Project Structure
```
DesignPatterns/
├── 1-SOLID_Design_Principles/
│   ├── 1-SingleResponsibility/
│   ├── 2-Open-Closed/
│   ├── 3-LiskovSubstitution/
│   ├── 4-Interface Segregation/
│   └── 5-DependencyInversion/
├── 2-Creational/
│   ├── 1-Builders/
│   │   ├── 1-Builder/
│   │   ├── 2-BuilderInheritance/
│   │   ├── 3-StepwiseBuilder/
│   │   ├── 4-FunctionalBuilder/
│   │   ├── 4-FunctionalGenericBuilder/
│   │   └── 5-FacetedBuilder/
│   ├── 2-Factories/
│   │   ├── 1-Factory/
│   │   ├── 2-AsynchronousFactory/
│   │   ├── 3-BulkReplacementFactory/
│   │   └── 4-AbstractFactory/
│   └── 3-Prototypes/
│       ├── 1-ICloneableIsBad/
│       ├── 2-CopyConstructorsInsteadOfICloneable/
│       ├── 3-Inheritance/
│       └── 4-CopyThroughSerialization/
└── DesignPatterns.sln
```

## 🤝 Contributing

Contributions are welcome! This repository aims to be a comprehensive resource for design patterns. Here's how you can contribute:

1. **Add New Patterns**: Implement additional design patterns following the existing structure
2. **Improve Examples**: Enhance existing examples with better use cases
3. **Documentation**: Improve explanations and add more detailed comments
4. **Bug Fixes**: Fix any issues in the existing implementations

### Contribution Guidelines

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-pattern`)
3. Follow the existing code style and structure
4. Add comprehensive comments and documentation
5. Include practical examples in the `Main` method
6. Update this README if adding new patterns
7. Commit your changes (`git commit -am 'Add new pattern: Factory Method'`)
8. Push to the branch (`git push origin feature/new-pattern`)
9. Create a Pull Request

## 📋 Planned Additions

- **Creational Patterns**: Singleton
- **Structural Patterns**: Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy
- **Behavioral Patterns**: Chain of Responsibility, Command, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor
- **Architectural Patterns**: MVC, MVP, MVVM, Repository, Unit of Work

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Design Patterns: Elements of Reusable Object-Oriented Software (Gang of Four)
- Clean Code: A Handbook of Agile Software Craftsmanship by Robert C. Martin
- The SOLID principles by Robert C. Martin

---

**Happy Coding! 🎉**

If you find this repository helpful, please consider giving it a ⭐ star!