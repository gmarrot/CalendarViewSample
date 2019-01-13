namespace CalendarViewSample.Droid.Utils {
    /// <summary>
    /// TypefaceCache caches used typefaces for performance and memory reasons. 
    /// Typeface cache is singleton shared through execution of the application.
    /// You can replace default implementation of the cache by implementing ITypefaceCache 
    /// interface and setting instance of your cache to static property SharedCache of this class
    /// </summary>
    public static class TypefaceCache {

        static ITypefaceCache _sharedCache;

        /// <summary>
        /// Returns the shared typeface cache.
        /// </summary>
        /// <value>The shared cache.</value>
        public static ITypefaceCache SharedCache {
            get {
                if (_sharedCache == null) {
                    _sharedCache = new DefaultTypefaceCache();
                }
                return _sharedCache;
            }
            set {
                if (_sharedCache != null && _sharedCache.GetType() == typeof(DefaultTypefaceCache)) {
                    ((DefaultTypefaceCache)_sharedCache).PurgeCache();
                }
                _sharedCache = value;
            }
        }

    }
}
