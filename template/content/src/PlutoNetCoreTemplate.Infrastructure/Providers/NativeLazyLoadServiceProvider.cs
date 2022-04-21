﻿namespace PlutoNetCoreTemplate.Infrastructure.Providers
{
    using Domain.SeedWork;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class NativeLazyLoadServiceProvider : ILazyLoadServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly object _serviceProviderLock = new();

        public NativeLazyLoadServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);


        public TService LazyGetRequiredService<TService>()
        {
            lock (this._serviceProviderLock)
            {
                return this._serviceProvider.GetRequiredService<TService>();
            }
        }


        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if ((object)reference == null)
            {
                lock (this._serviceProviderLock)
                {
                    if ((object)reference == null)
                        reference = (TRef)this._serviceProvider.GetRequiredService(serviceType);
                }
            }
            return reference;
        }

    }
}