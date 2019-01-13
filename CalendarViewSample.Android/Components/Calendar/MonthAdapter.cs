using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;

namespace CalendarViewSample.Droid.Components.Calendar {
    public class MonthAdapter : PagerAdapter {

        readonly LayoutInflater _inflater;

        readonly CalendarPickerView _calendar;

        MonthView _reusableMonthView;
        Dictionary<int, MonthView> _activeMonthViews;

        public MonthAdapter(Context context, CalendarPickerView calendar) {
            _calendar = calendar;
            _inflater = LayoutInflater.From(context);
            _activeMonthViews = new Dictionary<int, MonthView>();
        }

        public override int Count => _calendar.Months.Count;

        public override bool IsViewFromObject(View view, Java.Lang.Object @object) {
            return view == @object;
        }

        public override float GetPageWidth(int position) {
            return 1f;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position) {
            Java.Lang.Object obj = container;
            var pager = obj.JavaCast<ViewPager>();
            MonthView monthView = null;
            if (_reusableMonthView == null) {
                monthView = MonthView.Create(pager, _inflater, _calendar.WeekdayNameFormat, _calendar.Today,
                    _calendar.ClickHandler);
            } else {
                monthView = _reusableMonthView;
                _reusableMonthView = null;
            }
            monthView.Init(_calendar.Months[position], _calendar.Cells[position]);
            //monthView.SetBackgroundColor(global::Android.Graphics.Color.Orange);

            pager.AddView(monthView);
            _activeMonthViews[position] = monthView;
            return monthView;
        }

        public override void NotifyDataSetChanged() {
            foreach (var position in _activeMonthViews.Keys) {
                var view = _activeMonthViews[position];
                view.Init(_calendar.Months[position], _calendar.Cells[position]);
            }

            base.NotifyDataSetChanged();
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object) {
            //activePickerViews[position].OnDateSelected -= HandleOnDateSelected;
            //activePickerViews.Remove(position);
            var monthView = @object.JavaCast<MonthView>();
            container.JavaCast<ViewPager>().RemoveView(monthView);
            _reusableMonthView = monthView;
            _activeMonthViews.Remove(position);
        }

        //        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        //        {
        //          MonthView monthView = null;
        //
        //
        //          monthView = (MonthView)convertView ??
        //                             MonthView.Create(parent, _inflater, _calendar.WeekdayNameFormat, _calendar.Today,
        //                                 _calendar.ClickHandler);
        //          monthView.Init(_calendar.Months[position], _calendar.Cells[position]);
        //
        //          return monthView;
        //
        //        }

    }
}
