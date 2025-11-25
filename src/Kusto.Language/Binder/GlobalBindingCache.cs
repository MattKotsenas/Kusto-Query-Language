using System.Collections.Generic;

namespace Kusto.Language.Binding
{
    using Kusto.Language;
    using Symbols;
    using Utils;

    /// <summary>
    /// Binding state that persists across multiple bindings (lifetime of <see cref="KustoCache"/>)
    /// </summary>
    internal class GlobalBindingCache
    {
        public GlobalBindingCache()
        {
        }

        // PERF: Lazily initialize caches as they are needed (ConcurrentDictionary can be expensive to create)
        private ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> _unifiedNameColumnsMap;
        private ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> _unifiedNameAndTypeColumnsMap;
        private ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> _commonColumnsMap;
        private ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>> _callSiteToExpansionMap;
        private ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>> _callSiteToResultTypeMap;
        private ThreadSafeDictionary<Signature, FunctionBodyFacts> _databaseFunctionBodyFacts;

        public ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameColumnsMap
        {
            get
            {
                if (_unifiedNameColumnsMap == null)
                {
                    Interlocked.CompareExchange(
                        ref _unifiedNameColumnsMap,
                        new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default),
                        null);
                }
                return _unifiedNameColumnsMap;
            }
        }

        public ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameAndTypeColumnsMap
        {
            get
            {
                if (_unifiedNameAndTypeColumnsMap == null)
                {
                    Interlocked.CompareExchange(
                        ref _unifiedNameAndTypeColumnsMap,
                        new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default),
                        null);
                }
                return _unifiedNameAndTypeColumnsMap;
            }
        }

        public ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> CommonColumnsMap
        {
            get
            {
                if (_commonColumnsMap == null)
                {
                    Interlocked.CompareExchange(
                        ref _commonColumnsMap,
                        new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default),
                        null);
                }
                return _commonColumnsMap;
            }
        }

        public ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>> CallSiteToExpansionMap
        {
            get
            {
                if (_callSiteToExpansionMap == null)
                {
                    Interlocked.CompareExchange(
                        ref _callSiteToExpansionMap,
                        new ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>>(),
                        null);
                }
                return _callSiteToExpansionMap;
            }
        }

        public ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>> CallSiteToResultTypeMap
        {
            get
            {
                if (_callSiteToResultTypeMap == null)
                {
                    Interlocked.CompareExchange(
                        ref _callSiteToResultTypeMap,
                        new ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>>(),
                        null);
                }
                return _callSiteToResultTypeMap;
            }
        }

        public ThreadSafeDictionary<Signature, FunctionBodyFacts> DatabaseFunctionBodyFacts
        {
            get
            {
                if (_databaseFunctionBodyFacts == null)
                {
                    Interlocked.CompareExchange(
                        ref _databaseFunctionBodyFacts,
                        new ThreadSafeDictionary<Signature, FunctionBodyFacts>(),
                        null);
                }
                return _databaseFunctionBodyFacts;
            }
        }
    }
}
