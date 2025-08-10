# Design Patterns in C#

A comprehensive collection of design patterns implemented in C# with clear examples and explanations. This repository serves as a learning resource and reference guide for software developers looking to understand and implement design patterns in their projects.

## 📚 Table of Contents

- [SOLID Design Principles](#solid-design-principles)
  - [Single Responsibility Principle](#1-single-responsibility-principle)
  - [Open-Closed Principle](#2-open-closed-principle)
  - [Liskov Substitution Principle](#3-liskov-substitution-principle)
  - [Interface Segregation Principle](#4-interface-segregation-principle)
  - [Dependency Inversion Principle](#5-dependency-inversion-principle)
- [Creational Patterns - Builder](#creational-patterns---builder)
  - [Basic Builder](#1-basic-builder)
  - [Builder Inheritance](#2-builder-inheritance)
  - [Stepwise Builder](#3-stepwise-builder)
  - [Functional Builder](#4-functional-builder)
  - [Functional Generic Builder](#5-functional-generic-builder)
  - [Faceted Builder](#6-faceted-builder)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)

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
├── 2-Creational.Builder/
│   ├── 1-Builder/
│   ├── 2-BuilderInheritance/
│   ├── 3-StepwiseBuilder/
│   ├── 4-FunctionalBuilder/
│   ├── 4-FunctionalGenericBuilder/
│   └── 5-FacetedBuilder/
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

- **Creational Patterns**: Factory Method, Abstract Factory, Singleton, Prototype
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