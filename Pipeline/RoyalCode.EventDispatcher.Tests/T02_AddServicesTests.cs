using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.EventDispatcher.Tests;

public class T02_AddServicesTests
{
    [Fact]
    public void T01_AddEventDispatcherAndGetService()
    {
        var services = new ServiceCollection();

        services.AddEventDispatcher();

        var sp = services.BuildServiceProvider();

        var dispatcher = sp.GetService<IEventDispatcher>();

        Assert.NotNull(dispatcher);
    }

    [Fact]
    public async Task T02_AddEventDispatcherAndSendEvents()
    {
        var services = new ServiceCollection();

        services.AddEventDispatcher();

        var sp = services.BuildServiceProvider();

        var dispatcher = sp.GetService<IEventDispatcher>();

        Assert.NotNull(dispatcher);

        dispatcher!.DispatchInCurrentScope(new object());
        dispatcher!.DispatchInSeparetedScope(new object());

        await dispatcher!.DispatchInCurrentScopeAsync(new object());
        await dispatcher!.DispatchInSeparetedScopeAsync(new object());
    }
}

