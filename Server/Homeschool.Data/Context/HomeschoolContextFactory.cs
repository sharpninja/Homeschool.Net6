namespace Homeschool.Data.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public partial class HomeschoolContext
{
    public static IServiceProvider ServiceProvider
    {
        get;
        private set;
    } = null!;

    public HomeschoolContext(
        IServiceProvider serviceProvider,
        DbContextOptions<HomeschoolContext> options
    )
        : base(options)
        => ServiceProvider = serviceProvider;

    public static HomeschoolContext GetNewContext()
        => ServiceProvider?.GetRequiredService<HomeschoolContext>()!;
}
