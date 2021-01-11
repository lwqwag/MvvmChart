﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DemoViewModel
{
    public class SeriesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate0 { get; set; }
        public DataTemplate DataTemplate1 { get; set; }
        public DataTemplate DataTemplate2 { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var obj = item as SomePointList;
            if (obj == null)
            {
                return null;
            }



            switch (obj.Index)
            {
                case 0:
                    return this.DataTemplate0;
                case 1:
                    return this.DataTemplate1;
                case 2:

                    return this.DataTemplate2;


                default:

                    return obj.Index % 2 == 0 ? this.DataTemplate0 : this.DataTemplate2;
            }

        }
    }
}