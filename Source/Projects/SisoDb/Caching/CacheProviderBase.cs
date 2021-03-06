using System;
using System.Collections.Concurrent;
using SisoDb.NCore.Collections;
using SisoDb.Resources;

namespace SisoDb.Caching
{
    public abstract class CacheProviderBase : ICacheProvider
    {
        protected readonly ConcurrentDictionary<Type, ICache> CacheEntries;

        protected CacheProviderBase()
        {
            CacheEntries = new ConcurrentDictionary<Type, ICache>();
            AutoEnable = false;
        }

        public virtual ICache this[Type structureType]
        {
            get
            {
                if (!AutoEnable && !CacheEntries.ContainsKey(structureType))
                    return null;

                return CacheEntries.GetOrAdd(structureType, OnCreate);
            }
        }

        public bool AutoEnable { get; set; }

        public virtual void Clear()
        {
            var exceptions = CacheEntries.Values.TryForAll(e => e.Clear());
            if (exceptions != null)
                throw new SisoDbException(ExceptionMessages.CacheProvider_Clear_failed, exceptions);
        }

        public virtual bool Handles(Type structureType)
        {
            return CacheEntries.ContainsKey(structureType);
        }

        public virtual void EnableFor(Type structureType)
        {
            CacheEntries.GetOrAdd(structureType, OnCreate);
        }

        public virtual void DisableFor(Type structureType)
        {
            if (!CacheEntries.ContainsKey(structureType))
                return;

            ICache temp;
            CacheEntries.TryRemove(structureType, out temp);
        }

        /// <summary>
        /// This is called in conjunction with <![CDATA[http://msdn.microsoft.com/en-us/library/ee378677.aspx]]>,
        /// hence, do not make it expensive. If, then implement <see cref="ICacheProvider"/> and make one from
        /// scratch, or override <see cref="EnableFor"/> and <see cref="this[Type]"/>.
        /// </summary>
        /// <param name="structureType"></param>
        /// <returns></returns>
        protected abstract ICache OnCreate(Type structureType);
    }
}