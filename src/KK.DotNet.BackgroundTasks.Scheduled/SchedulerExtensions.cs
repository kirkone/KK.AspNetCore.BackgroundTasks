﻿namespace KK.DotNet.BackgroundTasks.Scheduled
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;

    public static class SchedulerExtensions
    {
        public static IScheduledTaskOptions<TScheduledTask> AddScheduledTaskOptions<TScheduledTask>(
            this IServiceCollection services,
            IConfiguration configuration
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var options = new ScheduledTaskOptions<TScheduledTask>();
            configuration.Bind(options);
            services.AddScheduledTaskOptions(options);
            return options;
        }

        public static IScheduledTaskOptions<TScheduledTask> AddScheduledTaskOptions<TScheduledTask>(
            this IServiceCollection services,
            Action<IScheduledTaskOptions<TScheduledTask>> configureOptions
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            var options = new ScheduledTaskOptions<TScheduledTask>();
            configureOptions.Invoke(options);
            services.AddScheduledTaskOptions(options);
            return options;
        }

        public static IScheduledTaskOptions<TScheduledTask> AddScheduledTaskOptions<TScheduledTask>(
            this IServiceCollection services,
            IScheduledTaskOptions<TScheduledTask> options
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));

            services.AddSingleton<IScheduledTaskOptions<TScheduledTask>>(options);
            return options;
        }

        public static IServiceCollection AddScheduledTask<TScheduledTask>(
            this IServiceCollection services
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.AddService<TScheduledTask>(null);
        }

        public static IServiceCollection AddScheduledTask<TScheduledTask>(
            this IServiceCollection services,
            System.Action<IScheduledTaskOptions<TScheduledTask>> configureOptions
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

            var options = new ScheduledTaskOptions<TScheduledTask>();
            configureOptions.Invoke(options);
            return services.AddService<TScheduledTask>(options);
        }

        public static IServiceCollection AddScheduledTask<TScheduledTask>(
            this IServiceCollection services,
            IScheduledTaskOptions<TScheduledTask> options
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));

            return services.AddService<TScheduledTask>(options);
        }

        private static IServiceCollection AddService<TScheduledTask>(
            this IServiceCollection services,
            IScheduledTaskOptions<TScheduledTask> options
        )
            where TScheduledTask : class, IScheduledTask
        {
            if (options != null)
            {
                services.AddSingleton<IScheduledTaskOptions<TScheduledTask>>(options);
            }

            services.TryAddSingleton<ISchedulerHostedService, SchedulerHostedService>();
            return services.AddSingleton<IScheduledTask, TScheduledTask>();
        }
    }
}
