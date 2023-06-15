using Nimedicus.Utils.Attributes;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Nimedicus.Utils
{
    class CachingGridRegionAdapter : RegionAdapterBase<Grid>
    {
        public CachingGridRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, Grid regionTarget)
        {
            if (regionTarget == null)
            {
                throw new ArgumentNullException(nameof(regionTarget));
            }

            var contentIsSet = regionTarget.Children.Count > 0;
            if (contentIsSet)
            {
                throw new InvalidOperationException("ContentControlHasContentException");
            }

            region.ActiveViews.CollectionChanged += delegate
            {
                List<UIElement> removeList = new List<UIElement>();
                // Hide all views in this region
                foreach (UIElement existingChild in regionTarget.Children)
                {
                    existingChild.Visibility = Visibility.Collapsed;
                    if (Attribute.GetCustomAttribute(existingChild.GetType(), typeof(DoNotCacheViewAttribute)) != null)
                    {
                        removeList.Add(existingChild);
                    }
                }
                removeList.ForEach(item => regionTarget.Children.Remove(item));
                // No view active? don't bother
                var viewToActivate = region.ActiveViews.FirstOrDefault() as UIElement;
                if (viewToActivate == null)
                {
                    return;
                }

                var existingViewToActivate = regionTarget.Children.Cast<UIElement>().SingleOrDefault(x => ReferenceEquals(x, viewToActivate));
                if (existingViewToActivate == null)
                {
                    var existingBinding = BindingOperations.GetBindingExpressionBase(regionTarget, UIElement.VisibilityProperty);
                    if (existingBinding != null)
                        throw new NotSupportedException();
                    regionTarget.Children.Add(viewToActivate);
                    existingViewToActivate = viewToActivate;
                }

                // Make the active view visible again
                existingViewToActivate.Visibility = Visibility.Visible;
            };

            region.Views.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add && !region.ActiveViews.Any())
                {
                    region.Activate(e.NewItems[0]);
                }
            };
        }

        protected override IRegion CreateRegion()
        {
            return new SingleActiveRegion();
        }
    }
}
