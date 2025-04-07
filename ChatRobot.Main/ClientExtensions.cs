using System.Reflection;
using ChatRobot.Client.Client;
using ChatRobot.Main.IOServer.Handler;
using ChatRobot.Main.MessageOperate;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRobot.Main;

public static class ClientExtenstions
{
    /// <summary>
    /// 将实现了 IProcessor<> 接口的类注册到依赖注入容器中
    /// </summary>
    /// <param name="services"></param>
    internal static void AddProcessors(this IServiceCollection containerRegistry)
    {
        // 获取基类 ProcessorBase<> 的类型
        var processorBaseType = typeof(ProcessorBase<>);
        // 获取当前执行程序集中的所有类型，并筛选出继承了 ProcessorBase<> 基类的类
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == processorBaseType
                        && t.IsClass && !t.IsAbstract).ToList();

        // 遍历筛选出的类型
        foreach (var type in types)
        {
            // 获取该类型的基类 ProcessorBase<> 的泛型参数
            var genericArgument = type.BaseType.GetGenericArguments().First();
            // 构建 IProcessor<> 接口类型
            var interfaceType = typeof(IProcessor<>).MakeGenericType(genericArgument);
            // 将接口和实现类注册到依赖注入容器中
            containerRegistry.AddTransient(interfaceType, type);
        }
    }
}