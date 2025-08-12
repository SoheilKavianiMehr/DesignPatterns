using System.Drawing;
using System.Threading.Tasks;

public class Foo
{
    public Foo()
    {
        
    }

    public async Task<Foo> InitAsync()
    {
        await Task.Delay(1000);
        return this;
    }
}

public class FooWithFactory
{
    private FooWithFactory()
    {

    }

    private async Task<FooWithFactory> InitAsync()
    {
        await Task.Delay(1000);
        return this;
    }

    public static Task<FooWithFactory> CreateAsync()
    {
        var result = new FooWithFactory();
        return result.InitAsync();
    }
}

class Demo
{
    static async Task Main(string[] args)
    {
        var foo = new Foo();
        await foo.InitAsync();
        

        FooWithFactory fooWithFactory = await FooWithFactory.CreateAsync(); 
    }
}