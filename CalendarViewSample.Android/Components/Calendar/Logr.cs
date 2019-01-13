using Android.Util;

namespace CalendarViewSample.Droid.Components.Calendar {
    public static class Logr {

        public static void D(string message) {
#if DEBUG
            Log.Debug("CalendarViewSample.Android", message);
#endif
        }

        public static void D(string message, params object[] args) {
#if DEBUG
            D(string.Format(message, args));
#endif
        }

    }
}
