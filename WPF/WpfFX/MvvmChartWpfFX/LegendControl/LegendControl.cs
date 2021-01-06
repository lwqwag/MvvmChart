﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace MvvmCharting.WpfFX
{

    /// <summary>
    /// Used to display the legend of <see cref="Chart"/>
    /// </summary>
    [TemplatePart(Name = "PART_LegendItemsControl", Type = typeof(SlimItemsControl))]
    public class LegendControl : Control
    {
        static LegendControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LegendControl), new FrameworkPropertyMetadata(typeof(LegendControl)));
        }

 
        private SlimItemsControl PART_LegendItemsControl;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.PART_LegendItemsControl = (SlimItemsControl)GetTemplateChild("PART_LegendItemsControl");
 
        }

 

 

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(LegendControl), new PropertyMetadata(null));


 

        public DataTemplate LegendItemTemplate
        {
            get { return (DataTemplate)GetValue(LegendItemTemplateProperty); }
            set { SetValue(LegendItemTemplateProperty, value); }
        }
        public static readonly DependencyProperty LegendItemTemplateProperty =
            DependencyProperty.Register("LegendItemTemplate", typeof(DataTemplate), typeof(LegendControl), new PropertyMetadata(null));



        public DataTemplateSelector LegendItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(LegendItemTemplateSelectorProperty); }
            set { SetValue(LegendItemTemplateSelectorProperty, value); }
        }
        public static readonly DependencyProperty LegendItemTemplateSelectorProperty =
            DependencyProperty.Register("LegendItemTemplateSelector", typeof(DataTemplateSelector), typeof(LegendControl), new PropertyMetadata(null));


        //public void OnItemHighlightChanged(object item, bool newValue)
        //{
        //    var legendItem = PART_LegendItemsControl?.TryGetElementForItem(item) as LegendItemControl;

 
        //    if (legendItem != null)
        //    {
        //        legendItem.IsHighlighted = newValue;
        //    }


        //}
    }
}
