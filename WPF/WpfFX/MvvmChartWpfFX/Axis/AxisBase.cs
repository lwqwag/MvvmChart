﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MvvmCharting.Axis;

namespace MvvmCharting.WpfFX.Axis
{
 

    /// <summary>
    /// The base class for <see cref="NumericAxis"/> and <see cref="CategoryAxis"/>.
    /// </summary>
    [TemplatePart(Name = "PART_AxisItemsControl", Type = typeof(Grid))]
    public abstract class AxisBase : Control, IAxisNS
    {
        static AxisBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisBase), new FrameworkPropertyMetadata(typeof(AxisBase)));
        }

        private static readonly string sPART_AxisItemsControl = "PART_AxisItemsControl";

        protected Grid PART_AxisItemsControl;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
 
            base.OnRenderSizeChanged(sizeInfo);
 
            if (this.Orientation == AxisType.X && sizeInfo.WidthChanged || 
                this.Orientation == AxisType.Y && sizeInfo.HeightChanged)
            {
                UpdateAxisDrawingSettings();
            }

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //if (this.PART_AxisItemsControl != null)
            //{
            //    this.PART_AxisItemsControl.ElementGenerated -= AxisItemsControlItemTemplateApplied;
            //}

            this.PART_AxisItemsControl = (Grid)GetTemplateChild(sPART_AxisItemsControl);
            if (this.PART_AxisItemsControl != null)
            {

                //this.PART_AxisItemsControl.ElementGenerated += AxisItemsControlItemTemplateApplied;

                //this.PART_AxisItemsControl.SetBinding(SlimItemsControl.ItemTemplateProperty,
                //    new Binding(nameof(this.ItemTemplate)) { Source = this });
                //this.PART_AxisItemsControl.SetBinding(SlimItemsControl.ItemTemplateSelectorProperty,
                //    new Binding(nameof(this.ItemTemplateSelector)) { Source = this });

                //this.PART_AxisItemsControl.ItemsSource = this.ItemDrawingParams;
                TryLoadAxisItems();
            }


        }


        protected IEnumerable<IAxisItem> GetAllAxisItems()
        {
            if (this.PART_AxisItemsControl == null)
            {
                return Enumerable.Empty<IAxisItem>();
            }

            return this.PART_AxisItemsControl.Children.OfType<IAxisItem>();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AxisBase), new PropertyMetadata(null));

        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }
        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register("TitleStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));


        public AxisPlacement Placement
        {
            get { return (AxisPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register("Placement", typeof(AxisPlacement), typeof(AxisBase), new PropertyMetadata(AxisPlacement.None, OnPlacementPropertyChange));

        private static void OnPlacementPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).OnPlacementChange();
        }

        private void OnPlacementChange()
        {
            this.AxisPlacementChanged?.Invoke(this);
        }

        public int TickCount
        {
            get { return (int)GetValue(TickCountProperty); }
            set { SetValue(TickCountProperty, value); }
        }
        public static readonly DependencyProperty TickCountProperty =
            DependencyProperty.Register("TickCount", typeof(int), typeof(AxisBase), new PropertyMetadata(10, OnTickCountPropertyChanged));

        private static void OnTickCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AxisBase)d).UpdateAxisDrawingSettings();
        }


        /// <summary>
        /// A double to string convert.
        /// The Axis only receive double values, so its the user's responsibility to provide a proper
        /// converter in order to correctly display the Label text. 
        /// If the double is converted from DateTime or DateTimeOffset, then it should be
        /// convert back to DateTime or DateTimeOffset first before it can be convert to a user-formatted string
        /// </summary>
        public IValueConverter LabelTextConverter
        {
            get { return (IValueConverter)GetValue(LabelTextConverterProperty); }
            set { SetValue(LabelTextConverterProperty, value); }
        }
        public static readonly DependencyProperty LabelTextConverterProperty =
            DependencyProperty.Register("LabelTextConverter", typeof(IValueConverter), typeof(AxisBase), new PropertyMetadata(null));

        public Style AxisItemStyle
        {
            get { return (Style)GetValue(AxisItemStyleProperty); }
            set { SetValue(AxisItemStyleProperty, value); }
        }
        public static readonly DependencyProperty AxisItemStyleProperty =
            DependencyProperty.Register("AxisItemStyle", typeof(Style), typeof(AxisBase), new PropertyMetadata(null));

 

        private IAxisOwner _owner;
        public IAxisOwner Owner
        {
            get { return this._owner; }
            set
            {

                if (this._owner != value)
                {
                    this._owner = value;
                    AttachHandler();
                }

            }
        }

        private AxisType _orientation;
        public AxisType Orientation
        {
            get { return this._orientation; }
            set
            {
                if (this._orientation != value)
                {
                    this._orientation = value;
                    AttachHandler();
                }

            }
        }

        private void AttachHandler()
        {
            if (this.Owner == null ||
                this.Orientation == AxisType.None)
            {
                return;
            }

            switch (this.Orientation)
            {
                case AxisType.X:
                    ((IXAxisOwner)this.Owner).HorizontalSettingChanged += AxisBase_CanvasSettingChanged;
                    break;
                case AxisType.Y:
                    ((IYAxisOwner)this.Owner).VerticalSettingChanged += AxisBase_CanvasSettingChanged;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            UpdateAxisDrawingSettings();
        }

        private void AxisBase_CanvasSettingChanged(PlottingRangeBase obj)
        {
            if (obj == null)
            {
                return;
            }

            this.PlottingRangeSetting = obj;
        }

        private PlottingRangeBase _plottingRangeSetting;
        protected PlottingRangeBase PlottingRangeSetting
        {
            get { return this._plottingRangeSetting; }

            set
            {
                if (this._plottingRangeSetting != value )
                {
                    this._plottingRangeSetting = value;
                    UpdateAxisDrawingSettings();
                }
            }
        }

        private IAxisDrawingSettingsBase _drawingSettings;
        internal IAxisDrawingSettingsBase DrawingSettings
        {
            get { return this._drawingSettings; }
            set
            {

                if (this._drawingSettings == null || !Equals(this._drawingSettings, value))
                {
                    this._drawingSettings = value;

                    TryLoadAxisItems();
                }


            }
        }

        protected abstract void UpdateAxisDrawingSettings();


        public abstract IEnumerable<double> GetAxisItemCoordinates();


        private void OnAxisItemCoordinateChanged()
        {

            var coordinates = GetAxisItemCoordinates();

            this.Owner.OnAxisItemsCoordinateChanged(this.Orientation, coordinates);

        }

        protected IAxisDrawingSettingsBase _currentDrawingSettings;

        protected abstract bool LoadAxisItems();

        public bool TryLoadAxisItems()
        {
            if (this.PART_AxisItemsControl == null)
            {
                return false;
            }

            bool succeed = LoadAxisItems();

            if (!succeed)
            {
                return false;
            }

            UpdateAxisItemsCoordinate();

            return true;
        }

        protected abstract void DoUpdateAxisItemsCoordinate();

        private void UpdateAxisItemsCoordinate()
        {

            if (!this.DrawingSettings.CanUpdateAxisItemsCoordinate())
            {
                throw new NotImplementedException();
            }

            DoUpdateAxisItemsCoordinate();

            OnAxisItemCoordinateChanged();
        }

        protected void DoUpdateAxisItems(IList<object> source)
        {
            var oldCt = this.PART_AxisItemsControl.Children.Count;
            var newCt = source.Count;
            if (oldCt > newCt)
            {
                this.PART_AxisItemsControl.Children.RemoveRange(newCt, oldCt - newCt);
            }

            for (int i = 0; i < source.Count; i++)
            {
                var newValue = source[i];
                if (i < oldCt)
                {
                    var item = this.PART_AxisItemsControl.Children[i] as AxisItem;
                    item.DataContext = newValue;
                }
                else
                {
                    AxisItem item = new AxisItem();
                    item.DataContext = newValue;
                    item.SetBinding(AxisItem.LabelTextConverterProperty, new Binding(nameof(this.LabelTextConverter)){Source = this});
                    item.SetBinding(StyleProperty, new Binding(nameof(this.AxisItemStyle)) { Source = this });
                    item.SetBinding(AxisItem.PlacementProperty, new Binding(nameof(this.Placement)) { Source = this });
 
                    this.PART_AxisItemsControl.Children.Add(item);
                }
            }

        }


        public event Action<IAxisNS> AxisPlacementChanged;



    }

}
