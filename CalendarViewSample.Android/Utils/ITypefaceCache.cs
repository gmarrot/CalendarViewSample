using Android.Graphics;

namespace CalendarViewSample.Droid.Utils {
    public interface ITypefaceCache {

        /// <summary>
        /// Removes typeface from cache
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="typeface">Typeface.</param>
        void StoreTypeface(string key, Typeface typeface);

        /// <summary>
        /// Removes the typeface.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveTypeface(string key);

        /// <summary>
        /// Retrieves the typeface.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Typeface.</returns>
        Typeface RetrieveTypeface(string key);

    }
}
